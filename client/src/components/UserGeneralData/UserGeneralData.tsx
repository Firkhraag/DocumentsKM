// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import GeneralDataModel from '../../model/GeneralDataModel'
import GeneralDataSection from '../../model/GeneralDataSection'
import GeneralDataPoint from '../../model/GeneralDataPoint'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'
import truncateText from '../../util/truncate'
import { useUser } from '../../store/UserStore'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

const UserGeneralData = () => {
	const user = useUser()
	const setPopup = useSetPopup()

	const [selectedObject, setSelectedObject] = useState<GeneralDataModel>({
		section: null,
		point: null,
		sectionText: '',
		pointText: '',
	})
	const [optionsObject, setOptionsObject] = useState({
		sections: [] as GeneralDataSection[],
		points: [] as GeneralDataPoint[],
	})
	const cachedPoints = useState(new Map<number, GeneralDataPoint[]>())[0]

	let createBtnDisabled = false
	if (
		optionsObject.points.length > 0 &&
		optionsObject.points
			.map((v) => v.text)
			.includes(selectedObject.pointText)
	) {
		createBtnDisabled = true
	}

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')
	const [isLeftErrMsg, setLeftErrMsg] = useState(true)

	useEffect(() => {
		const fetchData = async () => {
			try {
				const sectionsResponse = await httpClient.get(
					`/users/${user.id}/general-data-sections`
				)
				setOptionsObject({
					...optionsObject,
					sections: sectionsResponse.data,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
	}, [])

	const onSectionSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.sections,
			selectedObject.section
		)
		if (v != null) {
			if (cachedPoints.has(v.id)) {
				setOptionsObject({
					...optionsObject,
					points: cachedPoints.get(v.id),
				})
				setSelectedObject({
					...selectedObject,
					section: v,
					point: null,
					pointText: '',
					sectionText: v.name,
				})
			} else {
				try {
					const pointsResponse = await httpClient.get(
						`/general-data-sections/${id}/general-data-points`
					)
					cachedPoints.set(v.id, pointsResponse.data)
					setOptionsObject({
						...optionsObject,
						points: pointsResponse.data,
					})
					setSelectedObject({
						...selectedObject,
						section: v,
						point: null,
						sectionText: v.name,
					})
				} catch (e) {
					setLeftErrMsg(true)
					setErrMsg('Произошла ошибка')
				}
			}
		}
	}

	const onPointSelect = async (id: number) => {
		const v = getFromOptions(id, optionsObject.points, selectedObject.point)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				point: v,
				pointText: v.text,
			})
		}
	}

	const onSectionTextChange = (
		event: React.ChangeEvent<HTMLInputElement>
	) => {
		setSelectedObject({
			...selectedObject,
			sectionText: event.currentTarget.value,
		})
	}

	const onPointTextChange = (
		event: React.ChangeEvent<HTMLTextAreaElement>
	) => {
		setSelectedObject({
			...selectedObject,
			pointText: event.currentTarget.value,
		})
	}

	const onSectionNumChange = (num: number) => {
		const s = { ...selectedObject.section }
		s.orderNum = num
		setSelectedObject({
			...selectedObject,
			section: s,
		})
	}

	const onPointNumChange = (num: number) => {
		const p = { ...selectedObject.point }
		p.orderNum = num
		setSelectedObject({
			...selectedObject,
			point: p,
		})
	}

	const onSectionDeleteClick = async (row: number, id: number) => {
		setProcessIsRunning(true)
		try {
			await httpClient.delete(
				`/users/${user.id}/general-data-sections/${id}`
			)

			for (let s of optionsObject.sections) {
				if (s.orderNum > optionsObject.sections[row].orderNum) {
					s.orderNum = s.orderNum - 1
				}
			}

			var arr = [...optionsObject.sections]
			arr.splice(row, 1)
			setOptionsObject({
				...optionsObject,
				sections: arr,
				points: [],
			})

			if (
				selectedObject.section != null &&
				selectedObject.section.id == id
			) {
				setSelectedObject({
					...selectedObject,
					section: null,
					point: null,
				})
			}
			setPopup(defaultPopup)
		} catch (e) {
			setLeftErrMsg(true)
			setErrMsg('Произошла ошибка')
		}
		setProcessIsRunning(false)
	}

	const checkIfSectionValid = () => {
		if (selectedObject.sectionText === '') {
			setLeftErrMsg(true)
			setErrMsg('Пожалуйста, введите название раздела')
			return false
		}
		return true
	}

	const onUpdateSectionButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfSectionValid()) {
			try {
				await httpClient.patch(
					`/users/${user.id}/general-data-sections/${selectedObject.section.id}`,
					{
						name: selectedObject.sectionText,
						orderNum: selectedObject.section.orderNum,
					}
				)
				const s = { ...selectedObject.section }
				s.name = selectedObject.sectionText
				const foundSection = optionsObject.sections.find(
					(v) => v.id === s.id
				)
				foundSection.name = selectedObject.sectionText
				foundSection.orderNum = selectedObject.section.orderNum

				var num = 1
				for (let s of optionsObject.sections) {
					if (s.id == selectedObject.section.id) continue
					if (num == selectedObject.section.orderNum) {
						num = num + 1
						s.orderNum = num
						num = num + 1
						continue
					}
					s.orderNum = num
					num = num + 1
				}

				const compareFunc = (a: any, b: any) => {
					if (a.orderNum < b.orderNum) {
						return -1
					}
					if (a.orderNum > b.orderNum) {
						return 1
					}
					return 0
				}

				optionsObject.sections.sort(compareFunc)

				setSelectedObject({
					...selectedObject,
					section: s,
				})
			} catch (e) {
				setLeftErrMsg(true)
				if (e.response.status === 409) {
					setErrMsg('Раздел с таким содержанием уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
			}
		}
		setProcessIsRunning(false)
	}

	const onCreateSectionButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfSectionValid()) {
			try {
				const response = await httpClient.post(
					`/users/${user.id}/general-data-sections`,
					{
						name: selectedObject.sectionText,
					}
				)
				optionsObject.sections.push(response.data)
				setSelectedObject({
					...selectedObject,
					section: response.data,
				})
				setOptionsObject({
					...optionsObject,
					points: [],
				})
			} catch (e) {
				setLeftErrMsg(true)
				if (e.response.status === 409) {
					setErrMsg('Раздел с таким содержанием уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
			}
		}
		setProcessIsRunning(false)
	}

	const onPointDeleteClick = async (row: number, id: number) => {
		setProcessIsRunning(true)
		try {
			await httpClient.delete(
				`/general-data-sections/${selectedObject.section.id}/general-data-points/${id}`
			)

			for (let p of optionsObject.points) {
				if (p.orderNum > optionsObject.points[row].orderNum) {
					p.orderNum = p.orderNum - 1
				}
			}

			var arr = [...optionsObject.points]
			arr.splice(row, 1)
			setOptionsObject({
				...optionsObject,
				points: arr,
			})

			if (selectedObject.point != null && selectedObject.point.id == id) {
				setSelectedObject({
					...selectedObject,
					point: null,
				})
			}

			cachedPoints.delete(selectedObject.section.id)
		} catch (e) {
			setLeftErrMsg(false)
			setErrMsg('Произошла ошибка')
		}
		setPopup(defaultPopup)
		setProcessIsRunning(false)
	}

	const checkIfPointValid = () => {
		if (selectedObject.section === null) {
			setLeftErrMsg(false)
			setErrMsg('Пожалуйста, выберите раздел')
			return false
		}
		if (selectedObject.pointText === '') {
			setLeftErrMsg(false)
			setErrMsg('Пожалуйста, введите содержание пункта')
			return false
		}
		return true
	}

	const onUpdatePointButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfPointValid()) {
			try {
				await httpClient.patch(
					`/general-data-sections/${selectedObject.section.id}/general-data-points/${selectedObject.point.id}`,
					{
						text: selectedObject.pointText,
						orderNum: selectedObject.point.orderNum,
					}
				)
				const p = { ...selectedObject.point }
				p.text = selectedObject.pointText
				const foundPoint = optionsObject.points.find(
					(v) => v.id === p.id
				)
				foundPoint.text = selectedObject.pointText
				foundPoint.orderNum = selectedObject.point.orderNum

				var num = 1
				for (let p of optionsObject.points) {
					if (p.id == selectedObject.point.id) continue
					if (num == selectedObject.point.orderNum) {
						num = num + 1
						p.orderNum = num
						num = num + 1
						continue
					}
					p.orderNum = num
					num = num + 1
				}

				const compareFunc = (a: any, b: any) => {
					if (a.orderNum < b.orderNum) {
						return -1
					}
					if (a.orderNum > b.orderNum) {
						return 1
					}
					return 0
				}

				optionsObject.points.sort(compareFunc)

				setSelectedObject({
					...selectedObject,
					point: p,
				})
			} catch (e) {
				setLeftErrMsg(false)
				if (e.response.status === 409) {
					setErrMsg('Пункт с таким содержанием уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
			}
		}
		setProcessIsRunning(false)
	}

	const onCreatePointButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfPointValid()) {
			try {
				const response = await httpClient.post(
					`/general-data-sections/${selectedObject.section.id}/general-data-points`,
					{
						text: selectedObject.pointText,
					}
				)
				optionsObject.points.push(response.data)
				setSelectedObject({
					...selectedObject,
					point: response.data,
				})
			} catch (e) {
				setLeftErrMsg(false)
				if (e.response.status === 409) {
					setErrMsg('Пункт с таким содержанием уже существует')
				} else {
					setErrMsg('Произошла ошибка')
				}
			}
		}
		setProcessIsRunning(false)
	}

	return (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Шаблоны общих указаний</h1>
			<div className="flex">
				<div className="info-area shadow p-3 bg-white rounded component-width component-cnt-div">
					<div className="full-width">
						<div className="flex-v general-data-selection">
							{optionsObject.sections.map((s, index) => {
								return (
									<div
										className={
											selectedObject.section == null
												? 'pointer selection-text flex'
												: selectedObject.section.id ==
												  s.id
												? 'pointer selection-text selected-bg flex'
												: 'pointer selection-text flex'
										}
										onClick={() => onSectionSelect(s.id)}
										key={s.id}
									>
										<p
											className="no-bot-mrg"
											style={{ flex: 1 }}
										>
											{(index + 1).toString() +
												'. ' +
												s.name}
										</p>
										{
											<div
												onClick={() =>
													setPopup({
														isShown: true,
														msg: `Вы действительно хотите удалить раздел № ${
															index + 1
														}?`,
														onAccept: () =>
															onSectionDeleteClick(
																index,
																s.id
															),
														onCancel: () =>
															setPopup(
																defaultPopup
															),
													})
												}
												className="trash-area"
											>
												<Trash color="#666" size={22} />
											</div>
										}
									</div>
								)
							})}
						</div>
					</div>

					<Form.Group className="flex-cent-v mrg-top-2">
						<Form.Label
							className="bold no-bot-mrg"
							htmlFor="sectionOrderNum"
							style={{ marginRight: '1em' }}
						>
							Номер
						</Form.Label>
						<Select
							inputId="sectionOrderNum"
							maxMenuHeight={250}
							isSearchable={true}
							placeholder=""
							noOptionsMessage={() => 'Номер не найден'}
							className="num-field-width"
							isDisabled={
								selectedObject.section == null ? true : false
							}
							onChange={(selectedOption) =>
								onSectionNumChange(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.section == null
									? null
									: {
											value: selectedObject.section.id,
											label:
												selectedObject.section.orderNum,
									  }
							}
							options={[
								...Array(optionsObject.sections.length).keys(),
							].map((v) => {
								return {
									value: v + 1,
									label: v + 1,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label className="bold" htmlFor="section_title">
							Раздел
						</Form.Label>
						<Form.Control
							id="section_title"
							type="text"
							value={selectedObject.sectionText}
							onChange={onSectionTextChange}
							autoComplete="off"
						/>
					</Form.Group>
					{isLeftErrMsg ? (
						<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
					) : null}
					<div className="flex btn-mrg-top-2">
						<Button
							variant="secondary"
							className="flex-grow"
							onClick={onUpdateSectionButtonClick}
							disabled={
								selectedObject.section == null ||
								processIsRunning
							}
						>
							Изменить
						</Button>
						<Button
							variant="secondary"
							className="flex-grow mrg-left"
							onClick={onCreateSectionButtonClick}
							disabled={processIsRunning}
						>
							Добавить
						</Button>
					</div>
				</div>

				<div className="shadow p-3 bg-white rounded mrg-left component-width component-cnt-div">
					<div className="full-width">
						<div className="flex-v general-data-selection">
							{optionsObject.points.map((p, index) => {
								return (
									<div
										className={
											selectedObject.point == null
												? 'pointer selection-text flex'
												: selectedObject.point.id ==
												  p.id
												? 'pointer selection-text selected-bg flex'
												: 'pointer selection-text flex'
										}
										key={p.id}
									>
										<p
											className="no-bot-mrg"
											style={{ flex: 1 }}
											onClick={() => onPointSelect(p.id)}
										>
											{(index + 1).toString() +
												'. ' +
												truncateText(p.text, 55, null)}
										</p>
										<div
											onClick={() =>
												setPopup({
													isShown: true,
													msg: `Вы действительно хотите удалить пункт № ${
														index + 1
													}?`,
													onAccept: () =>
														onPointDeleteClick(
															index,
															p.id
														),
													onCancel: () =>
														setPopup(defaultPopup),
												})
											}
											className="trash-area"
										>
											<Trash color="#666" size={22} />
										</div>
									</div>
								)
							})}
						</div>

						<div className="space-between">
							<Form.Group className="flex-cent-v mrg-top-2">
								<Form.Label
									className="bold no-bot-mrg"
									htmlFor="orderNum"
									style={{ marginRight: '1em' }}
								>
									Номер
								</Form.Label>
								<Select
									inputId="orderNum"
									maxMenuHeight={250}
									isSearchable={true}
									placeholder=""
									noOptionsMessage={() => 'Номер не найден'}
									className="num-field-width"
									isDisabled={
										selectedObject.point == null
											? true
											: false
									}
									onChange={(selectedOption) =>
										onPointNumChange(
											(selectedOption as any)?.value
										)
									}
									value={
										selectedObject.point == null
											? null
											: {
													value:
														selectedObject.point.id,
													label:
														selectedObject.point
															.orderNum,
											  }
									}
									options={[
										...Array(
											optionsObject.points.length
										).keys(),
									].map((v) => {
										return {
											value: v + 1,
											label: v + 1,
										}
									})}
									styles={reactSelectStyle}
								/>
							</Form.Group>
							<div style={{ marginTop: '1.66em' }}>
								<span className="bold">Символы:</span> °C –
							</div>
						</div>
						<Form.Group className="no-bot-mrg">
							<Form.Label className="bold" htmlFor="text">
								Пункт
							</Form.Label>
							<Form.Control
								id="text"
								as="textarea"
								rows={8}
								style={{ resize: 'none' }}
								value={selectedObject.pointText}
								onChange={onPointTextChange}
							/>
						</Form.Group>
						{isLeftErrMsg ? null : (
							<ErrorMsg
								errMsg={errMsg}
								hide={() => setErrMsg('')}
							/>
						)}
						<div className="flex btn-mrg-top-2">
							<Button
								variant="secondary"
								className="flex-grow"
								onClick={onUpdatePointButtonClick}
								disabled={
									selectedObject.point == null ||
									processIsRunning
								}
							>
								Изменить
							</Button>
							<Button
								variant="secondary"
								className="flex-grow mrg-left"
								onClick={onCreatePointButtonClick}
								disabled={createBtnDisabled || processIsRunning}
							>
								Добавить
							</Button>
						</div>
					</div>
				</div>
			</div>
		</div>
	)
}

export default UserGeneralData
