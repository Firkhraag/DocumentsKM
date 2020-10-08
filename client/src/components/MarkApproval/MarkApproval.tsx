import React, { useState, useEffect } from 'react'
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import Dropdown from '../Dropdown/Dropdown'
import getFromOptions from '../../util/get-from-options'
import './MarkApproval.css'

const MarkApproval = () => {
	// Max lengths of input fields strings
	const departmentStringLength = 50
	const employeeStringLength = 50

	// Object that holds selected values
	const [selectedObject, setSelectedObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [[]] as Array<Employee[]>,
	})

	useEffect(() => {
		// Cannot use async func as callback in useEffect
		// Function for fetching data
		const fetchData = async () => {
			try {
				// Fetch departments
				const departmentsFetchedResponse = await httpClient.get(
					'/departments'
				)
				const departmentsFetched = departmentsFetchedResponse.data

				// Set fetched objects as select options
				setOptionsObject({
					...optionsObject,
					departments: departmentsFetched,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
	}, [])

	const onDepartmentSelect = (rowNumber: number) => {
		return async (number: number) => {
			const v = getFromOptions(
				number,
				optionsObject.departments,
				selectedObject.departments[rowNumber],
				true
			)
			if (v != null) {
				try {
					const fetchedmployeesResponse = await httpClient.get(
						`/departments/${number}/mark-approval-employees`
					)
					const fetchedEmployees = fetchedmployeesResponse.data

					const e = optionsObject.employees
					e[rowNumber] = fetchedEmployees
					setOptionsObject({
						...optionsObject,
						employees: e,
					})
					const d = selectedObject.departments
					d[rowNumber] = v
					setSelectedObject({
						...selectedObject,
						departments: d,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

	const onEmployeeSelect = (rowNumber: number) => {
		return async (id: number) => {
			const v = getFromOptions(
				id,
				optionsObject.employees[rowNumber],
				selectedObject.employees[rowNumber],
            )
			if (v != null) {
				const e = selectedObject.employees
				e[rowNumber] = v
				setSelectedObject({
					...selectedObject,
					employees: e,
				})
			}
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Согласования</h1>
			<table className="agreements-table white-bg">
				<tbody>
					<tr className="head-tr">
						<td>№</td>
						<td>Отдел</td>
						<td>Специалист</td>
					</tr>
					<tr>
						<td>1</td>
						<td>
							<Dropdown
								cntStyle="flex-v"
								label=""
								placeholder="Выберите отдел"
								maxInputLength={departmentStringLength}
								onClickFunc={onDepartmentSelect(0)}
								value={
									selectedObject.departments[0] == null
										? ''
										: selectedObject.departments[0].code
								}
								options={optionsObject.departments.map((d) => {
									return {
										id: d.number,
										val: d.code,
									}
								})}
							/>
						</td>
						<td>
							<Dropdown
								cntStyle="flex-v"
								label=""
								placeholder="Выберите специалиста"
								maxInputLength={employeeStringLength}
								onClickFunc={onEmployeeSelect(0)}
								value={
									selectedObject.employees[0] == null
										? ''
										: selectedObject.employees[0].fullName
								}
								options={optionsObject.employees[0].map((e) => {
									return {
										id: e.id,
										val: e.fullName,
									}
								})}
							/>
						</td>
					</tr>
				</tbody>
			</table>
			<button className="final-btn input-border-radius pointer">
				Сохранить изменения
			</button>
		</div>
	)
}

export default MarkApproval
