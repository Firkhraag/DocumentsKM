import React, { useState, useEffect } from 'react'
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import Dropdown from '../Dropdown/Dropdown'
import './MarkApproval.css'

const MarkApproval = () => {
	// Max lengths of input fields strings
	const departmentStringLength = 50
	const employeeStringLength = 50

	// Default state objects
	const defaultSelectedObject = {
		departments: [] as Department[],
		employees: [] as Employee[],
	}
	const defaultOptionsObject = {
		departments: [] as Department[],
		employees: [[]] as Array<Employee[]>,
	}

	// Object that holds selected values
	const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

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
					...defaultOptionsObject,
					departments: departmentsFetched,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
    }, [])
    
    const onDepartmentSelect = async (number: number) => {
        let v: Department = null
        for (let department of optionsObject.departments) {
            if (department.number === number) {
                v = department
                break
            }
        }
        if (v == null) {
            return
        }
        // if ((selectedObject.subnode !== null) && (v.id === selectedObject.subnode.id)) {
        //     return
        // }
        // try {
        //     const fetchedMarksResponse = await axios.get(protocol + '://' + host + `/subnodes/${id}/marks`)
        //     const fetchedMarks = fetchedMarksResponse.data
        //     setOptionsObject({
        //         ...defaultOptionsObject,
        //         recentMarks: optionsObject.recentMarks,
        //         recentSubnodes: optionsObject.recentSubnodes,
        //         projects: optionsObject.projects,
        //         nodes: optionsObject.nodes,
        //         subnodes: optionsObject.subnodes,
        //         marks: fetchedMarks
        //     })
        //     setSelectedObject({
        //         ...defaultSelectedObject,
        //         project: selectedObject.project,
        //         node: selectedObject.node,
        //         subnode: v
        //     })
        // } catch (e) {
        //     console.log('Failed to fetch the data')
        // }
    }

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Согласования</h1>
			<table className="agreements-table white-bg">
				<tbody>
					<tr className="head-tr">
						<td>Отдел</td>
						<td>Специалист</td>
					</tr>
					<tr>
						<td className="td-no-border">
							<Dropdown
								cntStyle="flex-v"
                                label=""
                                placeholder="Выберите отдел"
								maxInputLength={departmentStringLength}
								onClickFunc={null}
								value={''}
								options={optionsObject.departments.map((d) => {
									return {
										id: d.number,
										val: d.code,
									}
								})}
							/>
						</td>
						<td className="td-no-border">
							<Dropdown
								cntStyle="flex-v"
                                label=""
                                placeholder="Выберите специалиста"
								maxInputLength={employeeStringLength}
								onClickFunc={null}
								value={''}
								options={[
									{
										id: 0,
										val: 'test',
									},
								]}
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
