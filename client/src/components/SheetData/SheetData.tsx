// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import Sheet from '../../model/Sheet'
import SheetName from '../../model/SheetName'
import { useMark, useSetMark } from '../../store/MarkStore'
import { makeMarkName, makeComplexAndObjectName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

type SheetDataProps = {
	isCreateMode: boolean
}

const SheetData = ({ isCreateMode }: SheetDataProps) => {
	const defaultOptionsObject = {
		sheetNames: [] as SheetName[],
		employees: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<Sheet>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (isCreateMode) {
				const fetchData = async () => {
					try {
						const sheetNamesFetchedResponse = await httpClient.get(
							`/sheet-names`
						)
						const sheetNamesFetched = sheetNamesFetchedResponse.data
						const employeesFetchedResponse = await httpClient.get(
							`/departments/${mark.department.id}/employees`
						)
						const employeesFetched = employeesFetchedResponse.data
						setOptionsObject({
							sheetNames: sheetNamesFetched,
							employees: employeesFetched,
						})
						setSelectedObject({
							id: 0,
							num: 1,
							form: 1.0,
							name: '',
							creator: null,
							inspector: null,
							normContr: null,
							releaseNum: 0,
							numOfPages: 0,
							note: '',
						})
					} catch (e) {
						console.log('Failed to fetch the data')
					}
				}
				fetchData()
			} else {
                // TO DO
			}
		}
	}, [mark])

	const onNameSelect = async (id: number) => {
        let v = null
        for (let el of optionsObject.sheetNames) {
            if (el.id === id) {
                v = el
                break
            }
        }
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				name: v.name,
			})
		}
	}

	const onNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onFormatChange = (event: React.FormEvent<HTMLInputElement>) => {
		try {
			const v = parseFloat(event.currentTarget.value)
			setSelectedObject({
				...selectedObject,
				form: v,
			})
		} catch (e) {
			setSelectedObject({
				...selectedObject,
				form: null,
			})
		}
    }

	const onNoteChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	const onCreatorSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				creator: null,
			})
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

	const onInspectorSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				inspector: null,
			})
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

	const onNormControllerSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				normContr: null,
			})
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

	const checkIfValid = () => {
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование листа')
			return false
		}
		if (selectedObject.form == null) {
			setErrMsg('Пожалуйста, введите формат листа')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
                await httpClient.post(`/marks/${mark.id}/sheets/basic`, {
                    num: selectedObject.num,
                    name: selectedObject.name,
                    form: selectedObject.form,
                    creatorId: selectedObject.creator?.id,
                    inspectorId: selectedObject.inspector?.id,
                    normContrId: selectedObject.normContr?.id,
                    note: selectedObject.note,
                })
                history.push('/sheets')
			} catch (e) {
                setErrMsg('Произошла ошибка')
				console.log('Fail', e)
			}
		}
	}

	const onChangeButtonClick = async () => {}

	return selectedObject == null || (mark == null && !isCreateMode) ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
				{isCreateMode
					? 'Создание листа основного комплекта'
					: 'Данные листа основного комплекта'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group>
					<Form.Label>Наименование</Form.Label>
					<Form.Control
						type="text"
						placeholder="Введите наименование"
						value={selectedObject.name}
						onChange={onNameChange}
					/>
				</Form.Group>

				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Типовые наименования"
					noOptionsMessage={() => 'Наименования не найдены'}
					onChange={(selectedOption) =>
						onNameSelect((selectedOption as any)?.value)
					}
					value={null}
					options={optionsObject.sheetNames.map((s) => {
						return {
							value: s.id,
							label: s.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<Form.Group className="mrg-top-2">
					<Form.Label>Формат</Form.Label>
					<Form.Control
						type="text"
						placeholder="Введите формат"
						defaultValue={selectedObject.form.toFixed(1)}
						onBlur={onFormatChange}
					/>
				</Form.Group>

				<div className="bold mrg-top-2">Разработал</div>
				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Выбор разработчика"
					noOptionsMessage={() => 'Сотрудники не найдены'}
					className="mrg-top"
					onChange={(selectedOption) =>
						onCreatorSelect((selectedOption as any)?.value)
					}
					value={
						selectedObject.creator == null
							? null
							: {
									value: selectedObject.creator.id,
									label: selectedObject.creator.name,
							  }
					}
					options={optionsObject.employees.map((e) => {
						return {
							value: e.id,
							label: e.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<div className="bold mrg-top-2">Проверил</div>
				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Выбор проверщика"
					noOptionsMessage={() => 'Сотрудники не найдены'}
					className="mrg-top"
					onChange={(selectedOption) =>
						onInspectorSelect((selectedOption as any)?.value)
					}
					value={
						selectedObject.inspector == null
							? null
							: {
									value: selectedObject.inspector.id,
									label: selectedObject.inspector.name,
							  }
					}
					options={optionsObject.employees.map((e) => {
						return {
							value: e.id,
							label: e.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<div className="bold mrg-top-2">Нормоконтролер</div>
				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Выбор нормоконтролера"
					noOptionsMessage={() => 'Сотрудники не найдены'}
					className="mrg-top"
					onChange={(selectedOption) =>
						onNormControllerSelect((selectedOption as any)?.value)
					}
					value={
						selectedObject.normContr == null
							? null
							: {
									value: selectedObject.normContr.id,
									label: selectedObject.normContr.name,
							  }
					}
					options={optionsObject.employees.map((e) => {
						return {
							value: e.id,
							label: e.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<Form.Group className="mrg-top-2" style={{ marginBottom: 0 }}>
					<Form.Label>Примечание</Form.Label>
					<Form.Control
						as="textarea"
                        rows={4}
                        style={{resize: 'none'}}
						placeholder="Не введено"
						defaultValue={selectedObject.note}
						onBlur={onNoteChange}
					/>
				</Form.Group>

                <ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
				>
					{isCreateMode
						? 'Создать лист основного комплекта'
						: 'Сохранить изменения'}
				</Button>
                
			</div>
		</div>
	)
}

export default SheetData
