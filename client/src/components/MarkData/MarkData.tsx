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
import Mark from '../../model/Mark'
import { useMark, useSetMark } from '../../store/MarkStore'
import { makeMarkName, makeComplexAndObjectName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

type MarkDataProps = {
	isCreateMode: boolean
}

const MarkData = ({ isCreateMode }: MarkDataProps) => {
	const defaultOptionsObject = {
		departments: [] as Department[],
		chiefSpecialists: [] as Employee[],
		groupLeaders: [] as Employee[],
		mainBuilders: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()
	const setMark = useSetMark()

	const [selectedObject, setSelectedObject] = useState<Mark>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [errMsg, setErrMsg] = useState('')

	const cachedMainEmployees = useState(new Map<number, any>())[0]

	useEffect(() => {
		if (isCreateMode) {
			const recentMarkIdsStr = localStorage.getItem('recentMarkIds')
			const recentMarkIds = JSON.parse(recentMarkIdsStr) as number[]
			const selectedMarkId = recentMarkIds[0]
			if (selectedMarkId != null) {
				const fetchData = async () => {
					try {
						const departmentsFetchedResponse = await httpClient.get(
							'/departments'
						)
						const departmentsFetched =
							departmentsFetchedResponse.data

						setOptionsObject({
							...defaultOptionsObject,
							departments: departmentsFetched,
						})

						const markFetchedResponse = await httpClient.get(
							`/marks/${selectedMarkId}`
						)
						setSelectedObject({
							id: 0,
							code: '',
							name: '',
							subnode: markFetchedResponse.data.subnode,
							department: null,
							chiefSpecialist: null,
							groupLeader: null,
							mainBuilder: null,
						})
					} catch (e) {
						console.log('Failed to fetch departments')
					}
				}
				fetchData()
			}
		} else {
			const selectedMarkId = localStorage.getItem('selectedMarkId')
			if (selectedMarkId != null) {
				const fetchData = async () => {
					try {
						const markFetchedResponse = await httpClient.get(
							`/marks/${selectedMarkId}`
						)
						setMark(markFetchedResponse.data)
						setSelectedObject({ ...markFetchedResponse.data })

						const departmentsFetchedResponse = await httpClient.get(
							'/departments'
						)
						const departmentsFetched =
							departmentsFetchedResponse.data
						const fetchedMainEmployeesResponse = await httpClient.get(
							`departments/${markFetchedResponse.data.department.number}/mark-main-employees`
						)
						const fetchedMainEmployees =
							fetchedMainEmployeesResponse.data

						setOptionsObject({
							...defaultOptionsObject,
							departments: departmentsFetched,
							chiefSpecialists:
								fetchedMainEmployees.chiefSpecialists,
							groupLeaders: fetchedMainEmployees.groupLeaders,
							mainBuilders: fetchedMainEmployees.mainBuilders,
						})
					} catch (e) {
						console.log('Failed to fetch the mark')
					}
				}
				fetchData()
			}
		}
	}, [])

	const onMarkCodeChange = (event: React.FormEvent<HTMLInputElement>) => {
        setSelectedObject({
            ...selectedObject,
            code: event.currentTarget.value,
        })
	}

	const onMarkNameChange = (event: React.FormEvent<HTMLInputElement>) => {
        setSelectedObject({
            ...selectedObject,
            name: event.currentTarget.value,
        })
	}

	const onDepartmentSelect = async (number: number) => {
        if (number == null) {
            setOptionsObject({
                ...defaultOptionsObject,
                departments: optionsObject.departments,
            })
            setSelectedObject({
                ...selectedObject,
                department: null,
                chiefSpecialist: null,
                groupLeader: null,
                mainBuilder: null,
            })
        }
		const v = getFromOptions(
			number,
			optionsObject.departments,
			selectedObject.department,
			true
		)
		if (v != null) {
			if (cachedMainEmployees.has(v.number)) {
				setOptionsObject({
					...defaultOptionsObject,
					departments: optionsObject.departments,
					chiefSpecialists: cachedMainEmployees.get(v.number)
						.chiefSpecialists,
					groupLeaders: cachedMainEmployees.get(v.number)
						.groupLeaders,
					mainBuilders: cachedMainEmployees.get(v.number)
						.mainBuilders,
				})
				setSelectedObject({
					...selectedObject,
					department: v,
					chiefSpecialist: null,
					groupLeader: null,
					mainBuilder: null,
				})
			} else {
				try {
					const fetchedMainEmployeesResponse = await httpClient.get(
						`departments/${number}/mark-main-employees`
					)
					const fetchedMainEmployees =
						fetchedMainEmployeesResponse.data
					cachedMainEmployees.set(v.number, fetchedMainEmployees)
					setOptionsObject({
						...defaultOptionsObject,
						departments: optionsObject.departments,
						chiefSpecialists: fetchedMainEmployees.chiefSpecialists,
						groupLeaders: fetchedMainEmployees.groupLeaders,
						mainBuilders: fetchedMainEmployees.mainBuilders,
					})
					setSelectedObject({
						...selectedObject,
						department: v,
						chiefSpecialist: null,
						groupLeader: null,
						mainBuilder: null,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

	const onGroupLeaderSelect = async (id: number) => {
        if (id == null) {
            setSelectedObject({
				...selectedObject,
				groupLeader: null,
			})
        }
		const v = getFromOptions(
			id,
			optionsObject.groupLeaders,
			selectedObject.groupLeader
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				groupLeader: v,
			})
		}
	}

	const onChiefSpecialistSelect = async (id: number) => {
        if (id == null) {
            setSelectedObject({
				...selectedObject,
				chiefSpecialist: null,
			})
        }
		const v = getFromOptions(
			id,
			optionsObject.chiefSpecialists,
			selectedObject.chiefSpecialist
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				chiefSpecialist: v,
			})
		}
	}

	const onMainBuilderSelect = async (id: number) => {
        if (id == null) {
            setSelectedObject({
				...selectedObject,
				mainBuilder: null,
			})
        }
		const v = getFromOptions(
			id,
			optionsObject.mainBuilders,
			selectedObject.mainBuilder
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				mainBuilder: v,
			})
		}
	}

	const checkIfValid = () => {
		if (selectedObject.code === '') {
			setErrMsg('Пожалуйста, введите шифр марки')
			return false
		}
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование марки')
			return false
		}
		if (selectedObject.subnode == null) {
			setErrMsg('Ошибка')
			return false
		}
		if (selectedObject.department == null) {
			setErrMsg('Пожалуйста, выберите отдел')
			return false
		}
		if (selectedObject.mainBuilder == null) {
			setErrMsg('Пожалуйста, выберите главного строителя')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				const response = await httpClient.post('/marks', {
					code: selectedObject.code,
					name: selectedObject.name,
					subnodeId: selectedObject.subnode.id,
					departmentNumber: selectedObject.department.number,
					chiefSpecialistId: selectedObject.chiefSpecialist?.id,
					groupLeaderId: selectedObject.groupLeader?.id,
					mainBuilderId: selectedObject.mainBuilder.id,
				})
				localStorage.setItem('selectedMarkId', response.data.id)

				const recentMarkIdsStr = localStorage.getItem('recentMarkIds')
				if (recentMarkIdsStr != null) {
					const recentMarkIds = JSON.parse(
						recentMarkIdsStr
					) as number[]

					if (recentMarkIds.length >= 5) {
						recentMarkIds.shift()
					}
					recentMarkIds.unshift(response.data.id)
					let resStr = JSON.stringify(recentMarkIds)
					localStorage.setItem('recentMarkIds', resStr)
					setMark(response.data)
					history.push('/')
				}
			} catch (e) {
				console.log('Fail')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(`/marks/${selectedObject.id}`, {
					code:
						selectedObject.code === mark.code
							? undefined
							: selectedObject.code,
					name:
						selectedObject.name === mark.name
							? undefined
							: selectedObject.name,
					departmentNumber:
						selectedObject.department.number ===
						mark.department.number
							? undefined
							: selectedObject.department.number,
					chiefSpecialistId: getNullableFieldValue(
						selectedObject.chiefSpecialist,
						mark.chiefSpecialist
					),
					groupLeaderId: getNullableFieldValue(
						selectedObject.groupLeader,
						mark.groupLeader
					),
					mainBuilderId:
						selectedObject.mainBuilder.id === mark.mainBuilder.id
							? undefined
							: selectedObject.mainBuilder.id,
				})
				setMark(selectedObject)
				history.push('/')
			} catch (e) {
				console.log('Fail')
			}
		}
	}

	return selectedObject == null || (mark == null && !isCreateMode) ? null : (
		<div className="component-cnt">
			<h1 className="text-centered">
				{isCreateMode ? 'Создание марки' : 'Данные марки'}
			</h1>
			<div className="flex">
				<div className="info-area shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label>Обозначение марки</Form.Label>
						<Form.Control
							type="text"
							value={isCreateMode ? makeMarkName(
								selectedObject.subnode.node.project.baseSeries,
								selectedObject.subnode.node.code,
								selectedObject.subnode.code,
								selectedObject.code
							) : makeMarkName(
								mark.subnode.node.project.baseSeries,
								mark.subnode.node.code,
								mark.subnode.code,
								mark.code
							)}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Наименование комплекса</Form.Label>
						<Form.Control
							type="text"
							value={
								isCreateMode ? makeComplexAndObjectName(
									selectedObject.subnode.node.project.name,
									selectedObject.subnode.node.name,
									selectedObject.subnode.name,
									selectedObject.name
								).complexName : makeComplexAndObjectName(
									mark.subnode.node.project.name,
									mark.subnode.node.name,
									mark.subnode.name,
									mark.name
								).complexName
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Наименование объекта</Form.Label>
						<Form.Control
							type="text"
							value={
								isCreateMode ? makeComplexAndObjectName(
									selectedObject.subnode.node.project.name,
									selectedObject.subnode.node.name,
									selectedObject.subnode.name,
									selectedObject.name
								).objectName : makeComplexAndObjectName(
									mark.subnode.node.project.name,
									mark.subnode.node.name,
									mark.subnode.name,
									mark.name
								).objectName
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Главный инженер проекта</Form.Label>
						<Form.Control
							type="text"
							value={isCreateMode ? selectedObject.subnode.node.chiefEngineer.fullName : mark.subnode.node.chiefEngineer.fullName}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Начальник отдела</Form.Label>
						<Form.Control
							type="text"
							value={
								selectedObject.department == null || selectedObject.department.departmentHead == null
									? ''
									: selectedObject.department.departmentHead
											.fullName
							}
							readOnly={true}
						/>
					</Form.Group>
				</div>

				<div className="shadow p-3 mb-5 bg-white rounded mrg-left component-width component-cnt-div">
                    <ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
					<Form.Group>
						<Form.Label>Шифр марки</Form.Label>
						<Form.Control
							type="text"
							placeholder="Введите шифр марки"
							defaultValue={selectedObject.code}
							onBlur={onMarkCodeChange}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label>Наименование марки</Form.Label>
						<Form.Control
							type="text"
							placeholder="Введите наименование марки"
							defaultValue={selectedObject.name}
							onBlur={onMarkNameChange}
						/>
					</Form.Group>

					<div className="bold">Отдел</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите отдел"
						noOptionsMessage={() => 'Отделы не найдены'}
						className="mrg-top"
						onChange={(selectedOption) =>
							onDepartmentSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.department == null
								? null
								: {
										value: selectedObject.department.number,
										label: selectedObject.department.code,
								  }
						}
						options={optionsObject.departments.map((d) => {
							return {
								value: d.number,
								label: d.code,
							}
						})}
						styles={reactSelectstyle}
					/>
					<div className="bold mrg-top-2">Заведующий группы</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите заведующего группы"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="mrg-top"
						onChange={(selectedOption) =>
							onGroupLeaderSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.groupLeader == null
								? null
								: {
										value: selectedObject.groupLeader.id,
										label:
											selectedObject.groupLeader.fullName,
								  }
						}
						options={optionsObject.groupLeaders.map((e) => {
							return {
								value: e.id,
								label: e.fullName,
							}
						})}
						styles={reactSelectstyle}
					/>

					<div className="bold mrg-top-2">Главный специалист</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите главного специалиста"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="mrg-top"
						onChange={(selectedOption) =>
							onChiefSpecialistSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.chiefSpecialist == null
								? null
								: {
										value:
											selectedObject.chiefSpecialist.id,
										label:
											selectedObject.chiefSpecialist
												.fullName,
								  }
						}
						options={optionsObject.chiefSpecialists.map((e) => {
							return {
								value: e.id,
								label: e.fullName,
							}
						})}
						styles={reactSelectstyle}
					/>

					<div className="bold mrg-top-2">Главный строитель</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите главного строителя"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="mrg-top"
						onChange={(selectedOption) =>
							onMainBuilderSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.mainBuilder == null
								? null
								: {
										value: selectedObject.mainBuilder.id,
										label:
											selectedObject.mainBuilder.fullName,
								  }
						}
						options={optionsObject.mainBuilders.map((e) => {
							return {
								value: e.id,
								label: e.fullName,
							}
						})}
						styles={reactSelectstyle}
					/>

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={isCreateMode ? onCreateButtonClick : onChangeButtonClick}
					>
						{isCreateMode ? 'Создать марку' : 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default MarkData
