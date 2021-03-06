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
import Subnode from '../../model/Subnode'
import Mark from '../../model/Mark'
import { useMark, useSetMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
import { makeMarkName, makeComplexAndObjectName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

type MarkDataProps = {
	subnodeForCreate: Subnode
	isCreateMode: boolean
}

const MarkData = ({ isCreateMode, subnodeForCreate }: MarkDataProps) => {
	const defaultOptionsObject = {
		departments: [] as Department[],
		chiefSpecialists: [] as Employee[],
		groupLeaders: [] as Employee[],
		normContrs: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()
	const setMark = useSetMark()
	const user = useUser()

	const [departmentHead, setDepartmentHead] = useState<Employee>(null)
	const [selectedObject, setSelectedObject] = useState<Mark>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const cachedMainEmployees = useState(new Map<number, any>())[0]

	useEffect(() => {
		if (isCreateMode) {
			if (subnodeForCreate == null) {
				history.push('/marks')
				return
			}
			const fetchData = async () => {
				try {
					const departmentsResponse = await httpClient.get(
						'/departments'
					)
					const newMarkCodeResponse = await httpClient.get(
						`/subnodes/${subnodeForCreate.id}/new-mark-code`
					)
					setOptionsObject({
						...defaultOptionsObject,
						departments: departmentsResponse.data,
					})
					setSelectedObject({
						id: 0,
						code: newMarkCodeResponse.data,
						name: '',
						subnode: subnodeForCreate,
						department: null,
						chiefSpecialist: null,
						groupLeader: null,
						normContr: null,
					})
                    if (isCreateMode) {
                        const defaultValuesResponse = await httpClient.get(
                            `/users/${user.id}/default-values`
                        )
                        setSelectedObject({
                            ...selectedObject,
                            department: defaultValuesResponse.data.department,
                        })
                    }
				} catch (e) {
					console.log('Failed to fetch departments')
				}
			}
			fetchData()
		} else {
			const selectedMarkId = localStorage.getItem('selectedMarkId')
			if (selectedMarkId != null) {
				const fetchData = async () => {
					try {
						const markResponse = await httpClient.get(
							`/marks/${selectedMarkId}`
						)
						const departmentsResponse = await httpClient.get(
							'/departments'
						)
						const fetchedMainEmployeesResponse = await httpClient.get(
							`departments/${markResponse.data.department.id}/mark-main-employees`
						)
						setMark(markResponse.data)
						setSelectedObject({ ...markResponse.data })
						setOptionsObject({
							...defaultOptionsObject,
							departments: departmentsResponse.data,
							chiefSpecialists:
								fetchedMainEmployeesResponse.data
									.chiefSpecialists,
							groupLeaders:
								fetchedMainEmployeesResponse.data.groupLeaders,
							normContrs:
								fetchedMainEmployeesResponse.data.normContrs,
						})
						setDepartmentHead(
							fetchedMainEmployeesResponse.data.departmentHead
						)
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

	const onMarkNameChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
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
				normContr: null,
			})
		}
		const v = getFromOptions(
			number,
			optionsObject.departments,
			selectedObject.department
		)
		if (v != null) {
			if (cachedMainEmployees.has(v.id)) {
				setOptionsObject({
					...defaultOptionsObject,
					departments: optionsObject.departments,
					chiefSpecialists: cachedMainEmployees.get(v.id)
						.chiefSpecialists,
					groupLeaders: cachedMainEmployees.get(v.id).groupLeaders,
					normContrs: cachedMainEmployees.get(v.id).normContrs,
				})
				setSelectedObject({
					...selectedObject,
					department: v,
					chiefSpecialist: null,
					groupLeader: null,
					normContr: null,
				})
				setDepartmentHead(cachedMainEmployees.get(v.id).departmentHead)
			} else {
				try {
					const fetchedMainEmployeesResponse = await httpClient.get(
						`departments/${number}/mark-main-employees`
					)
					const fetchedMainEmployees =
						fetchedMainEmployeesResponse.data
					cachedMainEmployees.set(v.id, fetchedMainEmployees)
					setOptionsObject({
						...defaultOptionsObject,
						departments: optionsObject.departments,
						chiefSpecialists: fetchedMainEmployees.chiefSpecialists,
						groupLeaders: fetchedMainEmployees.groupLeaders,
						normContrs: fetchedMainEmployees.normContrs,
					})
					setSelectedObject({
						...selectedObject,
						department: v,
						chiefSpecialist: null,
						groupLeader: null,
						normContr: null,
					})
					setDepartmentHead(fetchedMainEmployees.departmentHead)
				} catch (e) {
					setErrMsg('Произошла ошибка')
				}
			}
		}
	}

	const onGroupLeaderSelect = (id: number) => {
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

	const onChiefSpecialistSelect = (id: number) => {
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

	const onNormContrSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				normContr: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.normContrs,
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
		if (selectedObject.code === '') {
			setErrMsg('Пожалуйста, введите шифр марки')
			return false
		}
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование марки')
			return false
		}
		if (selectedObject.department == null) {
			setErrMsg('Пожалуйста, выберите отдел')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const response = await httpClient.post(`/users/${user.id}/marks`, {
					code: selectedObject.code,
					name: selectedObject.name,
					subnodeId: selectedObject.subnode.id,
					departmentId: selectedObject.department.id,
					chiefSpecialistId: selectedObject.chiefSpecialist?.id,
					groupLeaderId: selectedObject.groupLeader?.id,
					normContrId: selectedObject.normContr.id,
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
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Марка с таким кодом уже существует')
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
		if (checkIfValid()) {
			try {
				const object = {
					code:
						selectedObject.code === mark.code
							? undefined
							: selectedObject.code,
					name:
						selectedObject.name === mark.name
							? undefined
							: selectedObject.name,
					departmentId:
						selectedObject.department.id === mark.department.id
							? undefined
							: selectedObject.department.id,
					chiefSpecialistId: getNullableFieldValue(
						selectedObject.chiefSpecialist,
						mark.chiefSpecialist
					),
					groupLeaderId: getNullableFieldValue(
						selectedObject.groupLeader,
						mark.groupLeader
					),
					normContrId: getNullableFieldValue(
						selectedObject.normContr,
						mark.normContr
					),
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(`/marks/${selectedObject.id}`, object)
				setMark(selectedObject)
				history.push('/')
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Марка с таким кодом уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || (mark == null && !isCreateMode) ? null : (
		<div className="component-cnt">
			<h1 className="text-centered">
				{isCreateMode ? 'Создание марки' : 'Данные марки'}
			</h1>
			<div className="flex">
				<div className="info-area shadow p-3 bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label>Обозначение марки</Form.Label>
						<Form.Control
							type="text"
							value={
								isCreateMode
									? makeMarkName(
											selectedObject.subnode.node.project
												.baseSeries,
											selectedObject.subnode.node.code,
											selectedObject.subnode.code,
											selectedObject.code
									  )
									: makeMarkName(
											mark.subnode.node.project
												.baseSeries,
											mark.subnode.node.code,
											mark.subnode.code,
											mark.code
									  )
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Наименование комплекса</Form.Label>
						<Form.Control
							as="textarea"
							rows={4}
							style={{ resize: 'none' }}
							value={
								isCreateMode
									? makeComplexAndObjectName(
											selectedObject.subnode.node.project
												.name,
											selectedObject.subnode.node.name,
											selectedObject.subnode.name,
											selectedObject.name
									  ).complexName
									: makeComplexAndObjectName(
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
							as="textarea"
							rows={4}
							style={{ resize: 'none' }}
							value={
								isCreateMode
									? makeComplexAndObjectName(
											selectedObject.subnode.node.project
												.name,
											selectedObject.subnode.node.name,
											selectedObject.subnode.name,
											selectedObject.name
									  ).objectName
									: makeComplexAndObjectName(
											mark.subnode.node.project.name,
											mark.subnode.node.name,
											mark.subnode.name,
											mark.name
									  ).objectName
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group className="space-between-cent-v">
						<Form.Label
							className="no-bot-mrg"
						>
							ГИП
						</Form.Label>
						<Form.Control
							type="text"
							className="mark-data-input-width1"
							value={
								isCreateMode
									? selectedObject.subnode.node.chiefEngineer
											.fullname
									: mark.subnode.node.chiefEngineer.fullname
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group className="space-between-cent-v no-bot-mrg">
						<Form.Label
							className="no-bot-mrg"
						>
							Начальник отдела
						</Form.Label>
						<Form.Control
							type="text"
							className="mark-data-input-width1"
							value={
								departmentHead == null
									? ''
									: departmentHead.fullname
							}
							readOnly={true}
						/>
					</Form.Group>
				</div>

				<div className="shadow p-3 bg-white rounded mrg-left component-width component-cnt-div">
					<Form.Group className="flex-cent-v">
						<Form.Label
							className="no-bot-mrg"
							style={{ marginRight: '1em' }}
							htmlFor="code"
						>
							Шифр
						</Form.Label>
						<Form.Control
							id="code"
							type="text"
							className="auto-width flex-grow"
							placeholder="Введите шифр марки"
							autoComplete="off"
							defaultValue={selectedObject.code}
							onBlur={onMarkCodeChange}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label htmlFor="name">Наименование</Form.Label>
						<Form.Control
							id="name"
							as="textarea"
							rows={4}
							style={{ resize: 'none' }}
							placeholder="Введите наименование марки"
							defaultValue={selectedObject.name}
							onBlur={onMarkNameChange}
						/>
					</Form.Group>

					<Form.Group className="flex-cent-v mrg-top-2">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="department"
							style={{ marginRight: '1em' }}
						>
							Отдел
						</Form.Label>
						<Select
							inputId="department"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите отдел"
							noOptionsMessage={() => 'Отделы не найдены'}
							className="auto-width flex-grow"
							onChange={(selectedOption) =>
								onDepartmentSelect(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.department == null
									? null
									: {
											value: selectedObject.department.id,
											label:
												selectedObject.department.name,
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
					</Form.Group>

					<Form.Group className="space-between-cent-v mrg-top-2">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="groupLeader"
						>
							Заведующий группы
						</Form.Label>
						<Select
							inputId="groupLeader"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите заведующего группы"
							noOptionsMessage={() => 'Сотрудники не найдены'}
							className="mark-data-input-width2"
							onChange={(selectedOption) =>
								onGroupLeaderSelect(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.groupLeader == null
									? null
									: {
											value:
												selectedObject.groupLeader.id,
											label:
												selectedObject.groupLeader.fullname,
									  }
							}
							options={optionsObject.groupLeaders.map((e) => {
								return {
									value: e.id,
									label: e.fullname,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="space-between-cent-v mrg-top-2">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="chiefSpecialist"
						>
							Главный специалист
						</Form.Label>
						<Select
							inputId="chiefSpecialist"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите главного специалиста"
							noOptionsMessage={() => 'Сотрудники не найдены'}
							className="mark-data-input-width2"
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
												selectedObject.chiefSpecialist
													.id,
											label:
												selectedObject.chiefSpecialist
													.fullname,
									  }
							}
							options={optionsObject.chiefSpecialists.map((e) => {
								return {
									value: e.id,
									label: e.fullname,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="space-between-cent-v mrg-top-2 no-bot-mrg">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="normContr"
						>
							Нормоконтроль
						</Form.Label>
						<Select
							inputId="normContr"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите нормоконтролера"
							noOptionsMessage={() => 'Сотрудники не найдены'}
							className="mark-data-input-width2"
							onChange={(selectedOption) =>
								onNormContrSelect(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.normContr == null
									? null
									: {
											value:
												selectedObject.normContr.id,
											label:
												selectedObject.normContr.fullname,
									  }
							}
							options={optionsObject.normContrs.map((e) => {
								return {
									value: e.id,
									label: e.fullname,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							isCreateMode
								? onCreateButtonClick
								: onChangeButtonClick
						}
						disabled={processIsRunning}
					>
						{isCreateMode ? 'Создать марку' : 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default MarkData
