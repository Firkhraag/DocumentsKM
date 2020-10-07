import React, { useState, useEffect } from 'react'
import { useSpring, animated } from 'react-spring'
import httpClient from '../../axios'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import Dropdown from '../Dropdown/Dropdown'
import Mark from '../../model/Mark'
import { useMark, useSetMark } from '../../store/MarkStore'
import {
	makeMarkOrSubnodeName,
	makeComplexAndObjectName,
} from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import './MarkData.css'

type MarkDataProps = {
	// -1 - создание новой марки
	markId: number
}

const MarkData = () => {
	const bottomDivHeight = 302

	const specialistNameStringLength = 50

	// Default state objects
	const defaultOptionsObject = {
		departments: [] as Department[],
		chiefSpecialists: [] as Employee[],
		groupLeaders: [] as Employee[],
		mainBuilders: [] as Employee[],
	}

	const mark = useMark()
	const setMark = useSetMark()

	// Object that holds selected values
	const [selectedMark, setSelectedMark] = useState<Mark>({
		id: 0,
		code: '',
		name: '',
		subnode: null,
		department: null,
		chiefSpecialist: null,
		groupLeader: null,
		mainBuilder: null,
	})
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	useEffect(() => {
		// Cannot use async func as callback in useEffect
		// Function for fetching data
		const selectedMarkId = localStorage.getItem('selectedMarkId')
		if (selectedMarkId != null) {
			const fetchData = async () => {
				try {
					const departmentsFetchedResponse = await httpClient.get(
						'/departments'
					)
					const departmentsFetched = departmentsFetchedResponse.data

					const markFetchedResponse = await httpClient.get(
						`/marks/${selectedMarkId}`
					)
					setMark(markFetchedResponse.data)

					// Set fetched objects as select options
					setSelectedMark({ ...markFetchedResponse.data })
					setOptionsObject({
						...defaultOptionsObject,
						departments: departmentsFetched,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [])

	const springStyle = useSpring({
		from: {
			opacity: 0 as any,
			height: 0,
			overflowY: 'hidden' as any,
		},
		to: {
			opacity: selectedMark.department == null ? (0 as any) : 1,
			height: selectedMark.department == null ? 0 : bottomDivHeight,
			overflowY:
				selectedMark.department == null
					? ('hidden' as any)
					: ('visible' as any),
		},
	})

	const onMarkCodeChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedMark({
			...selectedMark,
			code: event.currentTarget.value,
		})
	}

	const onMarkNameChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedMark({
			...selectedMark,
			name: event.currentTarget.value,
		})
	}

	const onDepartmentSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.departments,
			selectedMark.department,
			true
		)
		if (v != null) {
			try {
				const fetchedMainEmployeesResponse = await httpClient.get(
					`departments/${id}/mark-main-employees`
				)
				const fetchedMainEmployees = fetchedMainEmployeesResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					departments: optionsObject.departments,
					chiefSpecialists: fetchedMainEmployees.chiefSpecialists,
					groupLeaders: fetchedMainEmployees.groupLeaders,
					mainBuilders: fetchedMainEmployees.mainBuilders,
				})
				setSelectedMark({
					...selectedMark,
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

	const onGroupLeaderSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.groupLeaders,
			selectedMark.groupLeader
		)
		if (v != null) {
			setSelectedMark({
				...selectedMark,
				groupLeader: v,
			})
		}
	}

	const onChiefSpecialistSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.chiefSpecialists,
			selectedMark.chiefSpecialist
		)
		if (v != null) {
			setSelectedMark({
				...selectedMark,
				chiefSpecialist: v,
			})
		}
	}

	const onMainBuilderSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.mainBuilders,
			selectedMark.mainBuilder
		)
		if (v != null) {
			setSelectedMark({
				...selectedMark,
				mainBuilder: v,
			})
		}
	}

	return mark == null ? null : (
		<div className="component-cnt component-width">
			<h1 className="text-centered">Данные марки</h1>
			<div>
				<p className="text-centered data-section-label label-mrgn-top-1">
					Общая информация
				</p>

				<div className="flex-v mrg-bot-info">
					<p className="label-area">Обозначение марки</p>
					<div className="info-area">
						{makeMarkOrSubnodeName(
							mark.subnode.node.project.baseSeries,
							mark.subnode.node.code,
							mark.subnode.code,
							mark.code
						)}
					</div>
				</div>
				<div className="flex-v mrg-bot-info">
					<p className="label-area">Наименование комплекса</p>
					<div className="info-area">
						{
							makeComplexAndObjectName(
								mark.subnode.node.project.name,
								mark.subnode.node.name,
								mark.subnode.name,
								mark.name
							).complexName
						}
					</div>
				</div>
				<div className="flex-v mrg-bot-info">
					<p className="label-area">Наименование объекта</p>
					<div className="info-area">
						{
							makeComplexAndObjectName(
								mark.subnode.node.project.name,
								mark.subnode.node.name,
								mark.subnode.name,
								mark.name
							).objectName
						}
					</div>
				</div>
				<div className="flex-v mrg-bot-info">
					<p className="label-area">Главный инженер проекта</p>
					<div className="info-area">
						{mark.subnode.node.chiefEngineer.fullName}
					</div>
				</div>

				<p className="text-centered data-section-label label-mrgn-top-2">
					Информация марки
				</p>

				<div className="flex-v mrg-bot">
					<p className="label-area">Шифр марки</p>
					<div>
						<input
							value={selectedMark.code}
							onChange={onMarkCodeChange}
							type="text"
							className="input-area"
							placeholder="Введите шифр марки"
						/>
					</div>
				</div>
				<div className="flex-v mrg-bot">
					<p className="label-area">Наименование марки</p>
					<div>
						<input
							value={selectedMark.name}
							onChange={onMarkNameChange}
							type="text"
							className="input-area"
							placeholder="Введите наименование марки"
						/>
					</div>
				</div>
				<Dropdown
					cntStyle="flex-v mrg-bot"
					label="Отдел"
					placeholder={'Выберите отдел'}
					maxInputLength={specialistNameStringLength}
					onClickFunc={onDepartmentSelect}
					value={
						selectedMark.department == null
							? ''
							: selectedMark.department.code
					}
					options={optionsObject.departments.map((d) => {
						return {
							id: d.number,
							val: d.code,
						}
					})}
				/>
				<animated.div style={springStyle}>
					<div className="flex-v mrg-bot">
						<p className="label-area">Начальник отдела</p>
						<div className="info-area">
							{selectedMark.department == null ||
							selectedMark.department.departmentHead == null
								? '-'
								: selectedMark.department.departmentHead
										.fullName}
						</div>
					</div>
					<Dropdown
						cntStyle="flex-v mrg-bot"
						label="Заведующий группы"
						placeholder={'Не выбрано'}
						maxInputLength={specialistNameStringLength}
						onClickFunc={onGroupLeaderSelect}
						value={
							selectedMark.groupLeader == null
								? ''
								: selectedMark.groupLeader.fullName
						}
						options={optionsObject.groupLeaders.map((gl) => {
							return {
								id: gl.id,
								val: gl.fullName,
							}
						})}
					/>
					<Dropdown
						cntStyle="flex-v mrg-bot"
						label="Главный специалист"
						placeholder={'Не выбрано'}
						maxInputLength={specialistNameStringLength}
						onClickFunc={onChiefSpecialistSelect}
						value={
							selectedMark.chiefSpecialist == null
								? ''
								: selectedMark.chiefSpecialist.fullName
						}
						options={optionsObject.chiefSpecialists.map((cs) => {
							return {
								id: cs.id,
								val: cs.fullName,
							}
						})}
					/>
					<Dropdown
						cntStyle="flex-v"
						label="Главный строитель"
						placeholder={'Выберите главного строителя'}
						maxInputLength={specialistNameStringLength}
						onClickFunc={onMainBuilderSelect}
						value={
							selectedMark.mainBuilder == null
								? ''
								: selectedMark.mainBuilder.fullName
						}
						options={optionsObject.mainBuilders.map((mb) => {
							return {
								id: mb.id,
								val: mb.fullName,
							}
						})}
					/>
				</animated.div>
				<button className="final-btn input-border-radius pointer">
					Сохранить изменения
				</button>
			</div>
		</div>
	)
}

export default MarkData
