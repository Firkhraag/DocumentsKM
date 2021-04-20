// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { X } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import AdditionalWork from '../../model/AdditionalWork'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'

type IAdditionalWorkDataProps = {
	additionalWork: AdditionalWork
	isCreateMode: boolean
}

type AdditionalWorkDataProps = {
	additionalWorkData: IAdditionalWorkDataProps
	setAdditionalWorkData: (d: IAdditionalWorkDataProps) => void
	additionalWorkArray: AdditionalWork[]
	setAdditionalWorkArray: (a: AdditionalWork[]) => void
}

const AdditionalWorkData = ({
	additionalWorkData,
	setAdditionalWorkData,
	additionalWorkArray,
	setAdditionalWorkArray,
}: AdditionalWorkDataProps) => {
	const mark = useMark()

	const defaultSelectedObject = {
		id: 0,
		employee: null,
		valuation: 0,
		metalOrder: 0,
		drawingsCompleted: 0,
		drawingsCheck: 0,
	} as AdditionalWork

	const [selectedObject, setSelectedObject] = useState<AdditionalWork>(null)
	const [employees, setEmployees] = useState([] as Employee[])

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const [fetched, setFetched] = useState(false)

	useEffect(() => {
		if (!fetched) {
			const fetchData = async () => {
				try {
					const employeesResponse = await httpClient.get(
						`/departments/${mark.department.id}/employees`
					)
					setEmployees(employeesResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
			setFetched(true)
		}
		if (additionalWorkData.additionalWork != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...additionalWorkData.additionalWork,
			})
		} else {
			setSelectedObject({
				...defaultSelectedObject,
			})
		}
	}, [additionalWorkData])

	const onEmployeeSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				employee: null,
			})
		}
		const v = getFromOptions(id, employees, selectedObject.employee)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				employee: v,
			})
		}
	}

	const onValuationChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			valuation: parseInt(event.currentTarget.value),
		})
	}

	const onOrderChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			metalOrder: parseInt(event.currentTarget.value),
		})
	}

	const checkIfValid = () => {
		if (selectedObject.employee == null) {
			setErrMsg('Пожалуйста, выберите исполнителя')
			return false
		}
		if (isNaN(selectedObject.valuation)) {
			setErrMsg('Пожалуйста, введите расчет')
			return false
		}
		if (isNaN(selectedObject.metalOrder)) {
			setErrMsg('Пожалуйста, введите заказ металла')
			return false
		}
		if (
			selectedObject.valuation < 0 ||
			selectedObject.valuation > 1000000
		) {
			setErrMsg('Пожалуйста, введите правильный расчет')
			return false
		}
		if (
			selectedObject.metalOrder < 0 ||
			selectedObject.metalOrder > 1000000
		) {
			setErrMsg('Пожалуйста, введите правильный заказ металла')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(`/marks/${mark.id}/additional-work`, {
					employeeId: selectedObject.employee.id,
					valuation: selectedObject.valuation,
					metalOrder: selectedObject.metalOrder,
				})
				const arr = [...additionalWorkArray]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setAdditionalWorkArray(arr)
				setAdditionalWorkData({
					additionalWork: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Исполнитель уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	const onChangeButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const object = {
					employeeId:
						selectedObject.employee.id ===
						additionalWorkData.additionalWork.employee.id
							? undefined
							: selectedObject.employee.id,
					valuation:
						selectedObject.valuation === additionalWorkData.additionalWork.valuation
							? undefined
							: selectedObject.valuation,
					metalOrder:
						selectedObject.metalOrder === additionalWorkData.additionalWork.metalOrder
							? undefined
							: selectedObject.metalOrder,
				}
				await httpClient.patch(
					`/additional-work/${selectedObject.id}`,
					object
				)

				const arr = []
				for (const v of additionalWorkArray) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setAdditionalWorkArray(arr)
				setAdditionalWorkData({
					additionalWork: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Исполнитель уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<div className="shadow custom-p-3 mb-5 bg-white rounded component-width component-cnt-div relative">
				<div className="pointer absolute"
					style={{top: 5, right: 8}}
					onClick={() => setAdditionalWorkData({
						additionalWork: null,
						isCreateMode: false,
					})}
				>
					<X color="#666" size={33} />
				</div>
				<Form.Group className="space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="code"
					>
						Исполнитель
					</Form.Label>
					<Select
						inputId="code"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор исполнителя"
						noOptionsMessage={() => 'Исполнители не найдены'}
						className="additional-work-input-width"
						onChange={(selectedOption) =>
							onEmployeeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.employee == null
								? null
								: {
										value: selectedObject.employee.id,
										label: selectedObject.employee.fullname,
								  }
						}
						options={employees.map((v) => {
							return {
								value: v.id,
								label: v.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="numOfPages"
					>
						Расчет, лист А4
					</Form.Label>
					<Form.Control
						id="numOfPages"
						type="text"
						placeholder="Введите число листов"
						autoComplete="off"
						className="additional-work-input-width"
						value={
							isNaN(selectedObject.valuation)
								? ''
								: selectedObject.valuation
						}
						onChange={onValuationChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="format"
					>
						Заказ металла, строк
					</Form.Label>
					<Form.Control
						id="format"
						type="text"
						placeholder="Введите число строк"
						autoComplete="off"
						className="additional-work-input-width"
						value={
							isNaN(selectedObject.metalOrder)
								? ''
								: selectedObject.metalOrder
						}
						onChange={onOrderChange}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						additionalWorkData.isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
					disabled={processIsRunning || (!additionalWorkData.isCreateMode && !Object.values({
						employeeId:
							selectedObject.employee.id ===
							additionalWorkData.additionalWork.employee.id
								? undefined
								: selectedObject.employee.id,
						valuation:
							selectedObject.valuation === additionalWorkData.additionalWork.valuation
								? undefined
								: selectedObject.valuation,
						metalOrder:
							selectedObject.metalOrder === additionalWorkData.additionalWork.metalOrder
								? undefined
								: selectedObject.metalOrder,
					}).some((x) => x !== undefined))}
				>
					{additionalWorkData.isCreateMode
						? 'Добавить исполнителя'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default AdditionalWorkData
