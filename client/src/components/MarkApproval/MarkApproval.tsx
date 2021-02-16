// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import getFromOptions from '../../util/get-from-options'
import { useMark } from '../../store/MarkStore'
import { removeValueFromArray } from '../../util/array'
import { reactSelectStyle } from '../../util/react-select-style'

const MarkApproval = () => {
	const mark = useMark()
	const history = useHistory()

	const [selectedObject, setSelectedObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})
	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [[], [], [], [], [], [], []] as Employee[][],
	})

	const cachedEmployees = useState(new Map<number, Employee[]>())[0]
	const employeesExcludedFromOptions = useState([] as number[])[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const departmentsResponse = await httpClient.get(
						'/departments'
					)
					const markApprovalsResponse = await httpClient.get(
						`/marks/${mark.id}/approvals`
					)
					const markApprovals = markApprovalsResponse.data as Employee[]
					for (let e of markApprovals) {
						if (e != null) {
							employeesExcludedFromOptions.push(e.id)
						}
					}
					setSelectedObject({
						departments: markApprovals.map((e) => e?.department),
						employees: markApprovals,
					})

					const fetchEmployees = async (
						rowNumber: number,
						approvalSpecialist: Employee,
						options: Employee[][]
					) => {
						const employeesResponse = await httpClient.get(
							`/departments/${approvalSpecialist.department.id}/mark-approval-employees`
						)
						options[
							rowNumber
						] = employeesResponse.data as Employee[]
					}
					for (const [i, e] of markApprovals.entries()) {
						if (e != null) {
							if (cachedEmployees.has(e.department.id)) {
								optionsObject.employees[
									i
								] = cachedEmployees.get(e.department.id)
							} else {
								await fetchEmployees(
									i,
									e,
									optionsObject.employees
								)
							}
						}
					}
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

	const onDepartmentSelect = async (rowNumber: number, number: number) => {
		if (rowNumber > 0 && selectedObject.employees[rowNumber - 1] == null) {
			return
		}
		if (number == null) {
			optionsObject.employees[rowNumber] = []
			selectedObject.departments[rowNumber] = null
			if (selectedObject.employees[rowNumber] != null) {
				removeValueFromArray(
					employeesExcludedFromOptions,
					selectedObject.employees[rowNumber].id
				)
			}
			selectedObject.employees[rowNumber] = null
			setSelectedObject({
				employees: selectedObject.employees,
				departments: selectedObject.departments,
			})
			return
		}
		const v = getFromOptions(
			number,
			optionsObject.departments,
			selectedObject.departments[rowNumber]
		)
		if (v != null) {
			if (cachedEmployees.has(v.id)) {
				optionsObject.employees[rowNumber] = cachedEmployees.get(v.id)
				selectedObject.departments[rowNumber] = v
				setSelectedObject({
					...selectedObject,
					departments: selectedObject.departments,
				})
			} else {
				try {
					const employeesResponse = await httpClient.get(
						`/departments/${number}/mark-approval-employees`
					)
					cachedEmployees.set(v.id, employeesResponse.data)
					optionsObject.employees[rowNumber] =
						employeesResponse.data
					selectedObject.departments[rowNumber] = v
					setSelectedObject({
						...selectedObject,
						departments: selectedObject.departments,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

	const onEmployeeSelect = (rowNumber: number, id: number) => {
		if (id == null) {
			if (selectedObject.employees[rowNumber] != null) {
				removeValueFromArray(
					employeesExcludedFromOptions,
					selectedObject.employees[rowNumber].id
				)
			}
			selectedObject.employees[rowNumber] = null
			setSelectedObject({
				...selectedObject,
				employees: selectedObject.employees,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.employees[rowNumber],
			selectedObject.employees[rowNumber]
		)
		if (v != null) {
			if (selectedObject.employees[rowNumber] != null) {
				removeValueFromArray(
					employeesExcludedFromOptions,
					selectedObject.employees[rowNumber].id
				)
			}
			selectedObject.employees[rowNumber] = v
			setSelectedObject({
				...selectedObject,
				employees: selectedObject.employees,
			})
			employeesExcludedFromOptions.push(v.id)
		}
	}

	const onChangeButtonClick = async () => {
		try {
			const employeeIdsToSend = [] as number[]
			for (let e of selectedObject.employees) {
				if (e != null) {
					employeeIdsToSend.push(e.id)
				}
			}
			await httpClient.patch(
				`/marks/${mark.id}/approvals`,
				employeeIdsToSend
			)
			history.push('/')
		} catch (e) {
			console.log('Error')
		}
	}

	return mark == null ? null : (
		<div className="component-cnt">
			<h1 className="text-centered">Согласования</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-cnt-div">
				<div className="flex">
					<label
						className="bold input-width no-bot-mrg"
						htmlFor="approvalDepartment0"
					>
						Отдел
					</label>
					<label
						className="bold input-width mrg-left no-bot-mrg"
						htmlFor="approvalEmployee0"
					>
						Специалист
					</label>
				</div>
				{[...Array(optionsObject.employees.length).keys()].map(
					(rowNumber) => {
						return (
							<div className="flex mrg-top" key={rowNumber}>
								<Select
									inputId={`approvalDepartment${rowNumber}`}
									maxMenuHeight={250}
									className="input-width"
									isClearable={true}
									isSearchable={true}
									placeholder={
										rowNumber > 0 &&
										selectedObject.employees[
											rowNumber - 1
										] == null
											? ''
											: 'Выберите отдел'
									}
									noOptionsMessage={() => 'Отделы не найдены'}
									onChange={(selectedOption) =>
										onDepartmentSelect(
											rowNumber,
											(selectedOption as any)?.value
										)
									}
									value={
										selectedObject.departments[rowNumber] ==
										null
											? null
											: {
													value:
														selectedObject
															.departments[
															rowNumber
														].id,
													label:
														selectedObject
															.departments[
															rowNumber
														].name,
											  }
									}
									options={
										rowNumber > 0 &&
										selectedObject.employees[
											rowNumber - 1
										] == null
											? []
											: optionsObject.departments.map(
													(d) => {
														return {
															value: d.id,
															label: d.name,
														}
													}
											  )
									}
									styles={reactSelectStyle}
								/>
								<Select
									inputId={`approvalEmployee${rowNumber}`}
									maxMenuHeight={250}
									className="input-width mrg-left"
									isClearable={true}
									isSearchable={true}
									placeholder={
										rowNumber > 0 &&
										selectedObject.employees[
											rowNumber - 1
										] == null
											? ''
											: 'Выберите специалиста'
									}
									noOptionsMessage={() =>
										'Специалисты не найдены'
									}
									onChange={(selectedOption) =>
										onEmployeeSelect(
											rowNumber,
											(selectedOption as any)?.value
										)
									}
									value={
										selectedObject.employees[rowNumber] ==
										null
											? null
											: {
													value:
														selectedObject
															.employees[
															rowNumber
														].id,
													label:
														selectedObject
															.employees[
															rowNumber
														].name,
											  }
									}
									options={optionsObject.employees[rowNumber]
										.filter(
											(e) =>
												!employeesExcludedFromOptions.includes(
													e.id
												)
										)
										.map((e) => {
											return {
												value: e.id,
												label: e.name,
											}
										})}
									styles={reactSelectStyle}
								/>
							</div>
						)
					}
				)}
				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={onChangeButtonClick}
				>
					Сохранить изменения
				</Button>
			</div>
		</div>
	)
}

export default MarkApproval
