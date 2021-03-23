// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Employee from '../../model/Employee'
import Department from '../../model/Department'
import DefaultValues from '../../model/DefaultValues'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useUser } from '../../store/UserStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

const DefaultValuesData = () => {
	const history = useHistory()
	const user = useUser()

	const [
		defaultSelectedObject,
		setDefaultSelectedObject,
	] = useState<DefaultValues>(null)
	const [selectedObject, setSelectedObject] = useState<DefaultValues>({
		department: null,
		creator: null,
		inspector: null,
		normContr: null,
	})
	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		const fetchData = async () => {
			try {
				const departmentsResponse = await httpClient.get(`/departments`)
				const defaultValuesResponse = await httpClient.get(
					`/users/${user.id}/default-values`
				)
				if (defaultValuesResponse.data.department != null) {
					const employeesResponse = await httpClient.get(
						`/departments/${defaultValuesResponse.data.department.id}/employees`
					)
					setOptionsObject({
						departments: departmentsResponse.data,
						employees: employeesResponse.data,
					})
				} else {
					setOptionsObject({
						...optionsObject,
						departments: departmentsResponse.data,
					})
				}
				setSelectedObject(defaultValuesResponse.data)
				setDefaultSelectedObject(defaultValuesResponse.data)
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
	}, [])

	const onDepartmentSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				department: null,
				creator: null,
				inspector: null,
				normContr: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.departments,
			selectedObject.department
		)
		if (v != null) {
			try {
				const employeesResponse = await httpClient.get(
					`departments/${id}/employees`
				)
				setOptionsObject({
					...optionsObject,
					employees: employeesResponse.data,
				})
				setSelectedObject({
					...selectedObject,
					department: v,
					creator: null,
					inspector: null,
					normContr: null,
				})
			} catch (e) {
				setErrMsg('Произошла ошибка')
			}
			
		}
	}

	const onCreatorSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				creator: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.creator
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				creator: v,
			})
		}
	}

	const onInspectorSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				inspector: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.inspector
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				inspector: v,
			})
		}
	}

	const onNormControllerSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				normContr: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.normContr
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				normContr: v,
			})
		}
	}

	const onChangeButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const object = {
				departmentId: getNullableFieldValue(
					selectedObject.department,
					defaultSelectedObject.department
				),
				creatorId: getNullableFieldValue(
					selectedObject.creator,
					defaultSelectedObject.creator
				),
				inspectorId: getNullableFieldValue(
					selectedObject.inspector,
					defaultSelectedObject.inspector
				),
				normContrId: getNullableFieldValue(
					selectedObject.normContr,
					defaultSelectedObject.normContr
				),
			}
			if (!Object.values(object).some((x) => x !== undefined)) {
				setErrMsg('Изменения осутствуют')
				setProcessIsRunning(false)
				return
			}
			await httpClient.patch(`/users/${user.id}/default-values`, object)
			history.push('/')
		} catch (e) {
			setErrMsg('Произошла ошибка')
			console.log(e)
			setProcessIsRunning(false)
		}
	}

	return (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Значения по умолчанию</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="department"
						style={{ marginRight: '6.4em' }}
					>
						Отдел
					</Form.Label>
					<Select
						inputId="department"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите отдел"
						noOptionsMessage={() => 'Отделы не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onDepartmentSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.department == null
								? null
								: {
										value: selectedObject.department.id,
										label: selectedObject.department.name,
								  }
						}
						options={optionsObject.departments.map((d) => {
							return {
								value: d.id,
								label: d.name,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>
				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="creator"
						style={{ marginRight: '3.9em' }}
					>
						Разработал
					</Form.Label>
					<Select
						inputId="creator"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор разработчика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onCreatorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.creator == null
								? null
								: {
										value: selectedObject.creator.id,
										label: selectedObject.creator.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="inspector"
						style={{ marginRight: '4.5em' }}
					>
						Проверил
					</Form.Label>
					<Select
						inputId="inspector"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор проверщика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onInspectorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.inspector == null
								? null
								: {
										value: selectedObject.inspector.id,
										label: selectedObject.inspector.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="normContr"
						style={{ marginRight: '1em' }}
					>
						Нормоконтролер
					</Form.Label>
					<Select
						inputId="normContr"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор нормоконтролера"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onNormControllerSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.normContr == null
								? null
								: {
										value: selectedObject.normContr.id,
										label: selectedObject.normContr.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={onChangeButtonClick}
					disabled={processIsRunning}
				>
					{'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default DefaultValuesData
