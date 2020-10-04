import React, { useState, useEffect, useRef } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import Dropdown from '../Dropdown/Dropdown'
import { useMark } from '../../store/MarkStore'
import './MarkData.css'

const MarkData = () => {
	const specialistNameStringLength = 50

	// Default state objects
	const defaultSelectedObject = {
		department: null as Department,
		chiefSpecialist: null as Employee,
		groupLeader: null as Employee,
		mainBuilder: null as Employee,
	}
	const defaultOptionsObject = {
		departments: [] as Department[],
		chiefSpecialists: [] as Employee[],
		groupLeaders: [] as Employee[],
		mainBuilders: [] as Employee[],
    }
    
    const mark = useMark()

	// Object that holds selected values
	const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [height, setHeight] = useState(0)
	const heightRef = useRef()

	useEffect(() => {
		// Cannot use async func as callback in useEffect
		// Function for fetching data
		const fetchData = async () => {
			try {
				const departmentsFetchedResponse = await httpClient.get(
					'/departments'
				)
				const departmentsFetched = departmentsFetchedResponse.data
				console.log(departmentsFetched)

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

		// Observe the heights
		const heightObserver = new ResizeObserver(([entry]) => {
			setHeight(entry.target.scrollHeight)
		})
		if (heightRef.current) {
			heightObserver.observe(heightRef.current)
		}
		return () => {
			heightObserver.disconnect()
		}
	}, [heightRef])

	const springStyle = useSpring({
		from: {
			opacity: 0 as any,
			height: 0,
			overflowY: 'hidden' as any,
		},
		to: {
			opacity: selectedObject.department == null ? (0 as any) : 1,
			height: selectedObject.department == null ? 0 : height,
			overflowY:
				selectedObject.department == null
					? ('hidden' as any)
					: ('visible' as any),
		},
	})

	const onDepartmentSelect = async (number: number) => {
		let d: Department = null
		for (let department of optionsObject.departments) {
			if (department.number === number) {
				d = department
				break
			}
		}
		if (d == null) {
			return
		}
		if (
			selectedObject.department !== null &&
			d.number === selectedObject.department.number
		) {
			return
		}
		try {
			const fetchedMainEmployeesResponse = await httpClient.get(
				`departments/${number}/mark-main-employees`
			)
            const fetchedMainEmployees = fetchedMainEmployeesResponse.data
			setOptionsObject({
				...defaultOptionsObject,
				departments: optionsObject.departments,
				chiefSpecialists: fetchedMainEmployees.chiefSpecialists,
				groupLeaders: fetchedMainEmployees.groupLeaders,
				mainBuilders: fetchedMainEmployees.mainBuilders,
			})
			setSelectedObject({
				...defaultSelectedObject,
				department: d,
			})
		} catch (e) {
			console.log('Failed to fetch the data')
		}
    }
    
    const onGroupLeaderSelect = async (id: number) => {
		let e: Employee = null
		for (let employee of optionsObject.groupLeaders) {
			if (employee.id === id) {
				e = employee
				break
			}
		}
		if (e == null) {
			return
		}
		if (
			selectedObject.groupLeader !== null &&
			e.id === selectedObject.groupLeader.id
		) {
			return
		}
		setSelectedObject({
            ...selectedObject,
            groupLeader: e,
        })
    }

    const onChiefSpecialistSelect = async (id: number) => {
		let e: Employee = null
		for (let employee of optionsObject.chiefSpecialists) {
			if (employee.id === id) {
				e = employee
				break
			}
		}
		if (e == null) {
			return
		}
		if (
			selectedObject.chiefSpecialist !== null &&
			e.id === selectedObject.chiefSpecialist.id
		) {
			return
		}
		setSelectedObject({
            ...selectedObject,
            chiefSpecialist: e,
        })
    }
    
    const onMainBuilderSelect = async (id: number) => {
		let e: Employee = null
		for (let employee of optionsObject.mainBuilders) {
			if (employee.id === id) {
				e = employee
				break
			}
		}
		if (e == null) {
			return
		}
		if (
			selectedObject.mainBuilder !== null &&
			e.id === selectedObject.mainBuilder.id
		) {
			return
		}
		setSelectedObject({
            ...selectedObject,
            mainBuilder: e,
        })
	}

	return (
		<div className="component-cnt component-width">
			<h1 className="text-centered">Данные марки</h1>
			<div>
				<p className="text-centered data-section-label label-mrgn-top-1">
					Общая информация
				</p>
				<div className="flex-v mrg-bot">
					<p className="label-area">Обозначение марки</p>
					<p>Тест</p>
				</div>

				<div className="flex-v mrg-bot">
					<p className="label-area">Наименование комплекса</p>
					<p>Тест</p>
				</div>
				<div className="flex-v mrg-bot">
					<p className="label-area">Наименование объекта</p>
					<p>Тест</p>
				</div>
				<div className="flex-v mrg-bot">
					<p className="label-area">Главный инженер проекта</p>
					<p>Тест</p>
				</div>

				<p className="text-centered data-section-label label-mrgn-top-2">
					Редактируемая информация
				</p>

				<div className="flex-v mrg-bot">
					<p className="label-area">Шифр марки</p>
					<div className="info-area">Тест</div>
				</div>
				<div className="flex-v mrg-bot">
					<p className="label-area">Наименование марки</p>
					<div className="info-area">Тест</div>
				</div>
				<Dropdown
					cntStyle="flex-v mrg-bot"
					label="Отдел"
					maxInputLength={specialistNameStringLength}
					onClickFunc={onDepartmentSelect}
					value={
						selectedObject.department == null
							? ''
							: selectedObject.department.code
					}
					options={optionsObject.departments.map((d) => {
						return {
							id: d.number,
							val: d.code,
						}
					})}
				/>

				<animated.div style={springStyle}>
					<div ref={heightRef}>
						<div className="flex-v mrg-bot">
							<p className="label-area">Начальник отдела</p>
							<div className="info-area">
								{selectedObject.department == null ||
								selectedObject.department.departmentHead == null
									? '-'
									: selectedObject.department.departmentHead
											.fullName}
							</div>
						</div>

						<Dropdown
							cntStyle="flex-v mrg-bot"
							label="Заведующий группы"
							maxInputLength={specialistNameStringLength}
							onClickFunc={onGroupLeaderSelect}
							value={selectedObject.groupLeader == null ? '' : selectedObject.groupLeader.fullName}
							options={optionsObject.groupLeaders.map(gl => {
                                return {
                                    id: gl.id,
                                    val: gl.fullName,
                                }
                            })}
						/>
						<Dropdown
							cntStyle="flex-v mrg-bot"
							label="Главный специалист"
							maxInputLength={specialistNameStringLength}
							onClickFunc={onChiefSpecialistSelect}
							value={selectedObject.chiefSpecialist == null ? '' : selectedObject.chiefSpecialist.fullName}
							options={optionsObject.chiefSpecialists.map(cs => {
                                return {
                                    id: cs.id,
                                    val: cs.fullName,
                                }
                            })}
						/>
						{/* <Dropdown
                            cntStyle="flex-v mrg-bot"
                            label="Начальник отдела"
                            maxInputLength={specialistNameStringLength}
                            onClickFunc={null}
                            value={''}
                            options={[{
                                id: 0,
                                val: 'Test',
                            }]}
                        /> */}
						<Dropdown
							cntStyle="flex-v"
							label="Главный строитель"
							maxInputLength={specialistNameStringLength}
							onClickFunc={onMainBuilderSelect}
							value={selectedObject.mainBuilder == null ? '' : selectedObject.mainBuilder.fullName}
							options={optionsObject.mainBuilders.map(mb => {
                                return {
                                    id: mb.id,
                                    val: mb.fullName,
                                }
                            })}
						/>
					</div>
				</animated.div>

				<button className="final-btn input-border-radius pointer">
					Сохранить изменения
				</button>
			</div>
		</div>
	)
}

export default MarkData
