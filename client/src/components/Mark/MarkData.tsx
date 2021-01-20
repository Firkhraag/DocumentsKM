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
		mainBuilders: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()
	const setMark = useSetMark()

	const [departmentHead, setDepartmentHead] = useState<Employee>(null)
	const [selectedObject, setSelectedObject] = useState<Mark>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

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
						mainBuilder: null,
					})
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
							mainBuilders:
								fetchedMainEmployeesResponse.data.mainBuilders,
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
				mainBuilder: null,
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
					mainBuilders: cachedMainEmployees.get(v.id).mainBuilders,
				})
				setSelectedObject({
					...selectedObject,
					department: v,
					chiefSpecialist: null,
					groupLeader: null,
					mainBuilder: null,
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
						mainBuilders: fetchedMainEmployees.mainBuilders,
					})
					setSelectedObject({
						...selectedObject,
						department: v,
						chiefSpecialist: null,
						groupLeader: null,
						mainBuilder: null,
					})
					setDepartmentHead(fetchedMainEmployees.departmentHead)
				} catch (e) {
					console.log('Failed to fetch the data')
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

	const onMainBuilderSelect = (id: number) => {
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
					departmentId: selectedObject.department.id,
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
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Марка с таким кодом уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
				console.log('Error')
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
					mainBuilderId:
						selectedObject.mainBuilder.id === mark.mainBuilder.id
							? undefined
							: selectedObject.mainBuilder.id,
				})
				setMark(selectedObject)
				history.push('/')
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Марка с таким кодом уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
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
					<Form.Group className="flex-cent-v">
						<Form.Label
							className="no-bot-mrg"
							style={{ marginRight: '7.62em' }}
						>
							ГИП
						</Form.Label>
						<Form.Control
							type="text"
							className="auto-width flex-grow"
							value={
								isCreateMode
									? selectedObject.subnode.node.chiefEngineer
											.name
									: mark.subnode.node.chiefEngineer.name
							}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group className="flex-cent-v no-bot-mrg">
						<Form.Label
							className="no-bot-mrg"
							style={{ marginRight: '1em' }}
						>
							Начальник отдела
						</Form.Label>
						<Form.Control
							type="text"
							className="auto-width flex-grow"
							value={
								departmentHead == null
									? ''
									: departmentHead.name
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

					<Form.Group className="flex-cent-v mrg-top-2">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="groupLeader"
							style={{ marginRight: '1em' }}
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
							className="auto-width flex-grow"
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
												selectedObject.groupLeader.name,
									  }
							}
							options={optionsObject.groupLeaders.map((e) => {
								return {
									value: e.id,
									label: e.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="flex-cent-v mrg-top-2">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="chiefSpecialist"
							style={{ marginRight: '1.15em' }}
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
							className="auto-width flex-grow"
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
													.name,
									  }
							}
							options={optionsObject.chiefSpecialists.map((e) => {
								return {
									value: e.id,
									label: e.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="flex-cent-v mrg-top-2 no-bot-mrg">
						<Form.Label
							className="no-bot-mrg"
							htmlFor="mainBuilder"
							style={{ marginRight: '1.3em' }}
						>
							Главный строитель?
						</Form.Label>
						<Select
							inputId="mainBuilder"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите главного строителя"
							noOptionsMessage={() => 'Сотрудники не найдены'}
							className="auto-width flex-grow"
							onChange={(selectedOption) =>
								onMainBuilderSelect(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.mainBuilder == null
									? null
									: {
											value:
												selectedObject.mainBuilder.id,
											label:
												selectedObject.mainBuilder.name,
									  }
							}
							options={optionsObject.mainBuilders.map((e) => {
								return {
									value: e.id,
									label: e.name,
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
					>
						{isCreateMode ? 'Создать марку' : 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default MarkData
