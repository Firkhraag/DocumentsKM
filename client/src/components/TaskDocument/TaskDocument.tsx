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
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'

const TaskDocument = () => {
	const history = useHistory()
	const mark = useMark()

	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})
	const [selectedApproval, setSelectedApproval] = useState({
		department: null as Department,
		employee: null as Employee,
	})

	const cachedEmployees = useState(new Map<number, Employee[]>())[0]
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
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onTaskTextChange = (event: React.FormEvent<HTMLTextAreaElement>) => {}

	const onDepartmentSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...optionsObject,
				employees: [],
			})
			setSelectedApproval({
				department: null,
				employee: null,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.departments,
			selectedApproval.department
		)
		if (v != null) {
			if (cachedEmployees.has(v.id)) {
				setOptionsObject({
					...optionsObject,
					employees: cachedEmployees.get(v.id),
				})
				setSelectedApproval({
					department: v,
					employee: null,
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
					setSelectedApproval({
						department: v,
						employee: null,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

    const onEmployeeSelect = (id: number) => {
		if (id == null) {
			setSelectedApproval({
				...selectedApproval,
				employee: null,
			})
			return
		}
		const v = getFromOptions(id, optionsObject.employees, selectedApproval.employee)
		if (v != null) {
			setSelectedApproval({
				...selectedApproval,
				employee: v,
			})
		}
	}

	const checkIfValid = () => {
		// if (selectedObject.envAggressiveness == null) {
		// 	setErrMsg('Пожалуйста, введите агрессивность среды')
		// 	return false
		// }
		return true
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onTaskDocumentDownloadButtonClick = async () => {
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/task-doc`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute('download', `${mark.code}_ЗдСМ.docx`)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
	}

	return mark == null ? null : (
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
						defaultValue={''}
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
						defaultValue={''}
						onBlur={onTaskTextChange}
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
                            onDepartmentSelect((selectedOption as any)?.value)}
						value={
							selectedApproval.department == null
								? null
								: {
										value: selectedApproval.department.id,
										label: selectedApproval.department.name,
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
                            onEmployeeSelect((selectedOption as any)?.value)}
						value={selectedApproval.employee == null
                            ? null
                            : {
                                    value: selectedApproval.employee.id,
                                    label: selectedApproval.employee.name,
                              }}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.name,
							}
						})}
						styles={reactSelectStyle}
					/>
				</div>
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
				{/* <div className="flex btn-mrg-top-2"> */}
				<Button
					variant="secondary"
					// className="flex-grow"
					className="full-width btn-mrg-top-2"
					onClick={onChangeButtonClick}
				>
					Сохранить изменения
				</Button>
				<Button
					variant="secondary"
					// className="flex-grow mrg-left"
					className="full-width btn-mrg-top"
					onClick={onChangeButtonClick}
				>
					Скачать документ
				</Button>
				{/* </div> */}
			</div>
		</div>
	)
}

export default TaskDocument
