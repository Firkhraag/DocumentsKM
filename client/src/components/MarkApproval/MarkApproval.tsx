import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import Dropdown from '../Dropdown/Dropdown'
import MarkApprovals from '../../model/MarkApproval'
import getFromOptions from '../../util/get-from-options'
import { useMark } from '../../store/MarkStore'
import getNotRequiredFieldValue from '../../util/get-field-value'
import './MarkApproval.css'

const MarkApproval = () => {
	// Max lengths of input fields strings
	const departmentStringLength = 50
	const employeeStringLength = 50

	const history = useHistory()

	const mark = useMark()

	// Object that holds selected values
	const [selectedObject, setSelectedObject] = useState({
		departments: [] as Department[],
		employees: [] as Employee[],
	})
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState({
		departments: [] as Department[],
		employees: [[], [], [], [], [], [], []] as Employee[][],
	})
	// Original mark approvals
	const [markApprovalsObj, setMarkApprovalsObj] = useState<MarkApprovals>({
		approvalSpecialist1: null,
		approvalSpecialist2: null,
		approvalSpecialist3: null,
		approvalSpecialist4: null,
		approvalSpecialist5: null,
		approvalSpecialist6: null,
		approvalSpecialist7: null,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					// Fetch departments
					const departmentsFetchedResponse = await httpClient.get(
						'/departments'
					)
					const departmentsFetched = departmentsFetchedResponse.data
					// Fetch mark with approvals
					const markApprovalsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/approvals`
					)
					const markApprovalsFetched =
						markApprovalsFetchedResponse.data
                    setMarkApprovalsObj(markApprovalsFetched)
                    setSelectedObject({
                        departments: [
                            markApprovalsFetched.approvalSpecialist1?.department,
                            markApprovalsFetched.approvalSpecialist2?.department,
                            markApprovalsFetched.approvalSpecialist3?.department,
                            markApprovalsFetched.approvalSpecialist4?.department,
                            markApprovalsFetched.approvalSpecialist5?.department,
                            markApprovalsFetched.approvalSpecialist6?.department,
                            markApprovalsFetched.approvalSpecialist7?.department,
                        ],
                        employees: [
                            markApprovalsFetched.approvalSpecialist1,
                            markApprovalsFetched.approvalSpecialist2,
                            markApprovalsFetched.approvalSpecialist3,
                            markApprovalsFetched.approvalSpecialist4,
                            markApprovalsFetched.approvalSpecialist5,
                            markApprovalsFetched.approvalSpecialist6,
                            markApprovalsFetched.approvalSpecialist7,
                        ],
                    })

					const fetchEmployees = async (
						rowNumber: number,
						approvalSpecialist: Employee,
						options: Employee[][]
					) => {
						if (approvalSpecialist != null) {
							const fetchedEmployeesResponse = await httpClient.get(
								`/departments/${approvalSpecialist.department.number}/mark-approval-employees`
							)
							const fetchedEmployees =
								fetchedEmployeesResponse.data
							options[rowNumber] = fetchedEmployees
						}
					}
					const employeeOptions = optionsObject.employees
					await fetchEmployees(
						0,
						markApprovalsFetched.approvalSpecialist1,
						employeeOptions
					)
					await fetchEmployees(
						1,
						markApprovalsFetched.approvalSpecialist2,
						employeeOptions
					)
					await fetchEmployees(
						2,
						markApprovalsFetched.approvalSpecialist3,
						employeeOptions
					)
					await fetchEmployees(
						3,
						markApprovalsFetched.approvalSpecialist4,
						employeeOptions
					)
					await fetchEmployees(
						4,
						markApprovalsFetched.approvalSpecialist5,
						employeeOptions
					)
					await fetchEmployees(
						5,
						markApprovalsFetched.approvalSpecialist6,
						employeeOptions
					)
					await fetchEmployees(
						6,
						markApprovalsFetched.approvalSpecialist7,
						employeeOptions
					)
					setOptionsObject({
						departments: departmentsFetched,
						employees: employeeOptions,
                    })
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onDepartmentSelect = (rowNumber: number) => {
        if (rowNumber > 0 && selectedObject.employees[rowNumber - 1] == null) {
            return
        }
		return async (number: number) => {
			const v = getFromOptions(
				number,
				optionsObject.departments,
				selectedObject.departments[rowNumber],
				true
			)
			if (v != null) {
				try {
					const fetchedEmployeesResponse = await httpClient.get(
						`/departments/${number}/mark-approval-employees`
					)
                    let fetchedEmployees = fetchedEmployeesResponse.data as Employee[]
					for (let e of selectedObject.employees) {
                        console.log('1')
                        if (e != null) {
                            fetchedEmployees = fetchedEmployees.filter(
                                (e2) => e2.id !== e.id
                            )
                        }
                    }
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
				selectedObject.employees[rowNumber]
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

	// Removing approval is not supported right now
	const onChangeButtonClick = async () => {
		// DEBUG
		// console.log({
		// 	approvalSpecialist1Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[0],
		// 		markApprovalsObj.approvalSpecialist1
		// 	),
		// 	approvalSpecialist2Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[1],
		// 		markApprovalsObj.approvalSpecialist2
		// 	),
		// 	approvalSpecialist3Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[2],
		// 		markApprovalsObj.approvalSpecialist3
		// 	),
		// 	approvalSpecialist4Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[3],
		// 		markApprovalsObj.approvalSpecialist4
		// 	),
		// 	approvalSpecialist5Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[4],
		// 		markApprovalsObj.approvalSpecialist5
		// 	),
		// 	approvalSpecialist6Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[5],
		// 		markApprovalsObj.approvalSpecialist6
		// 	),
		// 	approvalSpecialist7Id: getNotRequiredFieldValue(
		// 		selectedObject.employees[5],
		// 		markApprovalsObj.approvalSpecialist7
		// 	),
		// })
		try {
			await httpClient.patch(`/marks/${mark.id}/approvals`, {
				approvalSpecialist1Id: getNotRequiredFieldValue(
					selectedObject.employees[0],
					markApprovalsObj.approvalSpecialist1
				),
				approvalSpecialist2Id: getNotRequiredFieldValue(
					selectedObject.employees[1],
					markApprovalsObj.approvalSpecialist2
				),
				approvalSpecialist3Id: getNotRequiredFieldValue(
					selectedObject.employees[2],
					markApprovalsObj.approvalSpecialist3
				),
				approvalSpecialist4Id: getNotRequiredFieldValue(
					selectedObject.employees[3],
					markApprovalsObj.approvalSpecialist4
				),
				approvalSpecialist5Id: getNotRequiredFieldValue(
					selectedObject.employees[4],
					markApprovalsObj.approvalSpecialist5
				),
				approvalSpecialist6Id: getNotRequiredFieldValue(
					selectedObject.employees[5],
					markApprovalsObj.approvalSpecialist6
				),
				approvalSpecialist7Id: getNotRequiredFieldValue(
					selectedObject.employees[5],
					markApprovalsObj.approvalSpecialist7
				),
			})
			history.push('/')
		} catch (e) {
			console.log('Fail')
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
					{[...Array(7).keys()].map((rowNumber) => {
						return (
							<tr key={rowNumber}>
								<td>{rowNumber + 1}</td>
								<td>
									<Dropdown
										cntStyle="flex-v"
										label=""
										placeholder={(rowNumber > 0 && selectedObject.employees[rowNumber - 1] == null) ? "" : "Выберите отдел"}
										maxInputLength={departmentStringLength}
										onClickFunc={onDepartmentSelect(
											rowNumber
										)}
										value={
											selectedObject.departments[
												rowNumber
											] == null
												? ''
												: selectedObject.departments[
														rowNumber
												  ].code
										}
										options={(rowNumber > 0 && selectedObject.employees[rowNumber - 1] == null) ? [] : optionsObject.departments.map(
											(d) => {
												return {
													id: d.number,
													val: d.code,
												}
											}
										)}
									/>
								</td>
								<td>
									<Dropdown
										cntStyle="flex-v"
										label=""
										placeholder={(rowNumber > 0 && selectedObject.employees[rowNumber - 1] == null) ? "" : "Выберите специалиста"}
										maxInputLength={employeeStringLength}
										onClickFunc={onEmployeeSelect(
											rowNumber
										)}
										value={
											selectedObject.employees[
												rowNumber
											] == null
												? ''
												: selectedObject.employees[
														rowNumber
												  ].fullName
										}
										options={optionsObject.employees[
											rowNumber
										].map((e) => {
											return {
												id: e.id,
												val: e.fullName,
											}
										})}
									/>
								</td>
							</tr>
						)
					})}
				</tbody>
			</table>
			<button
				onClick={onChangeButtonClick}
				className="final-btn input-border-radius pointer"
			>
				Сохранить изменения
			</button>
		</div>
	)
}

export default MarkApproval
