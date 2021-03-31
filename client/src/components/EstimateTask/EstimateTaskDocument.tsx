// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import EstimateTask from '../../model/EstimateTask'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'
import { makeMarkName } from '../../util/make-name'

const EstimateTaskDocument = () => {
	const history = useHistory()
	const mark = useMark()

	const [
		defaultSelectedObject,
		setDefaultSelectedObject,
	] = useState<EstimateTask>(null)

	const [selectedObject, setSelectedObject] = useState<EstimateTask>(null)
	const [selectedDepartment, setSelectedDepartment] = useState<Department>(
		null
	)

	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})
	const cachedEmployees = useState(new Map<number, Employee[]>())[0]

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const departmentsResponse = await httpClient.get(
						'/departments'
					)
					setOptionsObject({
						departments: departmentsResponse.data,
						employees: optionsObject.employees,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}

				try {
					const estimateTaskResponse = await httpClient.get(
						`/marks/${mark.id}/estimate-task`
					)
					setSelectedObject(estimateTaskResponse.data)
					setSelectedDepartment(
						estimateTaskResponse.data.approvalEmployee?.department
					)
					setDefaultSelectedObject(estimateTaskResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onTaskTextChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			taskText: event.currentTarget.value,
		})
	}

	const onAdditionalTextChange = (
		event: React.FormEvent<HTMLTextAreaElement>
	) => {
		setSelectedObject({
			...selectedObject,
			additionalText: event.currentTarget.value,
		})
	}

	const onDepartmentSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...optionsObject,
				employees: [],
			})
			setSelectedDepartment(null)
			setSelectedObject({
				...selectedObject,
				approvalEmployee: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.departments,
			selectedDepartment
		)
		if (v != null) {
			if (cachedEmployees.has(v.id)) {
				setOptionsObject({
					...optionsObject,
					employees: cachedEmployees.get(v.id),
				})
				setSelectedDepartment(v)
				setSelectedObject({
					...selectedObject,
					approvalEmployee: null,
				})
			} else {
				try {
					const employeesResponse = await httpClient.get(
						`/departments/${id}/mark-approval-employees`
					)
					cachedEmployees.set(v.id, employeesResponse.data)
					setOptionsObject({
						...optionsObject,
						employees: employeesResponse.data,
					})
					setSelectedDepartment(v)
					setSelectedObject({
						...selectedObject,
						approvalEmployee: null,
					})
				} catch (e) {
					setErrMsg('Произошла ошибка')
				}
			}
		}
	}

	const onEmployeeSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				approvalEmployee: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.approvalEmployee
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				approvalEmployee: v,
			})
		}
	}

	const checkIfValid = () => {
		if (selectedObject.taskText == null) {
			setErrMsg('Пожалуйста, введите текст задания')
			return false
		}
		return true
	}

	const onChangeButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const object = {
					taskText:
						selectedObject.taskText ===
						defaultSelectedObject.taskText
							? undefined
							: selectedObject.taskText,
					additionalText:
						selectedObject.additionalText ===
						defaultSelectedObject.additionalText
							? undefined
							: selectedObject.additionalText,
					approvalEmployeeId: getNullableFieldValue(
						selectedObject.approvalEmployee,
						defaultSelectedObject.approvalEmployee
					),
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/marks/${mark.id}/estimate-task`,
					object
				)
				history.push('/')
			} catch (e) {
				setErrMsg('Произошла ошибка')
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	const onDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/estimate-task-document`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_ЗдСМ.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
			history.push('/')
		} catch (e) {
			if (e.response != null && e.response.status === 404) {
				setErrMsg('Пожалуйста, заполните условия эскплуатации у марки')
			} else {
				setErrMsg('Произошла ошибка')
			}
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Задание на смету</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<Form.Group>
					<Form.Label htmlFor="text">Текст задания</Form.Label>
					<Form.Control
						id="text"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите текст задания"
						defaultValue={selectedObject.taskText}
						onBlur={onTaskTextChange}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label htmlFor="additional">Дополнительно</Form.Label>
					<Form.Control
						id="additional"
						as="textarea"
						rows={8}
						style={{ resize: 'none' }}
						placeholder="Дополнительная информация отсутствует"
						defaultValue={selectedObject.additionalText}
						onBlur={onAdditionalTextChange}
					/>
				</Form.Group>
				<div className="flex">
					<label
						className="bold input-width no-bot-mrg"
						htmlFor="approvalDepartment"
					>
						Отдел для согласования
					</label>
					<label
						className="bold input-width mrg-left no-bot-mrg"
						htmlFor="approvalEmployee"
					>
						Специалист для согласования
					</label>
				</div>
				<div className="flex mrg-top">
					<Select
						inputId="approvalDepartment"
						maxMenuHeight={250}
						className="input-width"
						isClearable={true}
						isSearchable={true}
						placeholder="Отсутствует"
						noOptionsMessage={() => 'Отделы не найдены'}
						onChange={(selectedOption) =>
							onDepartmentSelect((selectedOption as any)?.value)
						}
						value={
							selectedDepartment == null
								? null
								: {
										value: selectedDepartment.id,
										label: selectedDepartment.name,
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
					<Select
						inputId="approvalEmployee"
						maxMenuHeight={250}
						className="input-width mrg-left"
						isClearable={true}
						isSearchable={true}
						placeholder="Отсутствует"
						noOptionsMessage={() => 'Специалисты не найдены'}
						onChange={(selectedOption) =>
							onEmployeeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.approvalEmployee == null
								? null
								: {
										value:
											selectedObject.approvalEmployee.id,
										label:
											selectedObject.approvalEmployee
												.fullname,
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
				</div>
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
				<div className="flex btn-mrg-top-2">
					<Button
						variant="secondary"
						className="full-width"
						onClick={onChangeButtonClick}
						disabled={processIsRunning}
					>
						Сохранить изменения
					</Button>
					<Button
						variant="secondary"
						className="full-width mrg-left"
						onClick={onDownloadButtonClick}
						disabled={processIsRunning}
					>
						Скачать документ
					</Button>
				</div>
			</div>
		</div>
	)
}

export default EstimateTaskDocument
