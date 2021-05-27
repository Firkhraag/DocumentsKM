// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
import Scroll from 'react-scroll'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { Trash } from 'react-bootstrap-icons'
import { Files } from 'react-bootstrap-icons'
import { PlusCircle } from 'react-bootstrap-icons'
import { ClipboardPlus } from 'react-bootstrap-icons'
import { ArrowUpCircle } from 'react-bootstrap-icons'
import { ArrowDownCircle } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import GeneralDataModel from '../../model/GeneralDataModel'
import GeneralDataSection from '../../model/GeneralDataSection'
import GeneralDataPoint from '../../model/GeneralDataPoint'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'
import truncateText from '../../util/truncate'
import SectionsSelectPopup from './SectionsSelectPopup'
import PointsSelectPopup from './PointsSelectPopup'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'
import table1 from '../../../assets/table1.png'
import table2 from '../../../assets/table2.png'
import table3 from '../../../assets/table3.png'

type MarkGeneralDataProps = {
	copiedSectionId: number
	setCopiedSectionId: (id: number) => void
}

const MarkGeneralData = ({ copiedSectionId, setCopiedSectionId }: MarkGeneralDataProps) => {
	const corrProtSectionName = "Антикоррозионная защита"

	const history = useHistory()
	const mark = useMark()
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

	const [isSectionsSelectionShown, setSectionsSelectionShown] = useState(
		false
	)
	const [isPointsSelectionShown, setPointsSelectionShown] = useState(false)

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

	const [refresh, setRefresh] = useState(false)

	const currentPointFromArray = selectedObject.point == null ? null : optionsObject.points.find(
		(v) => v.id === selectedObject.point.id
	)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sectionsResponse = await httpClient.get(
						`/marks/${mark.id}/mark-general-data-sections`
					)
					setOptionsObject({
						...optionsObject,
						sections: sectionsResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark, refresh])

	const onSectionSelect = async (id: number, name: string) => {
		const v = getFromOptions(
			id,
			optionsObject.sections,
			selectedObject.section
		)
		if (v != null) {
			var pText = selectedObject.pointText
            if (name == corrProtSectionName) {
                const pointText = await httpClient.get(
                    `/marks/${mark.id}/corr-prot-point`
                )
                pText = pointText.data.result
            }
			if (cachedPoints.has(v.id)) {
				setOptionsObject({
					...optionsObject,
					points: cachedPoints.get(v.id),
				})
				setSelectedObject({
					...selectedObject,
					section: v,
					point: null,
					pointText: pText,
					sectionText: v.name,
				})
			} else {
				try {
					const pointsResponse = await httpClient.get(
						`/mark-general-data-sections/${id}/mark-general-data-points`
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
						pointText: pText,
						sectionText: v.name,
					})
				} catch (e) {
					setLeftErrMsg(true)
					setErrMsg('Произошла ошибка')
				}
			}
		}
	}

	const onPointSelect = (id: number, scroll: boolean) => {
		const v = getFromOptions(id, optionsObject.points, selectedObject.point)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				point: v,
				pointText: v.text,
			})
			if (scroll) {
				Scroll.scroller.scrollTo(`point${id}`, {
					containerId: "srollable-points",
				})
			}
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

	const onLineBreakCheck = () => {
		setSelectedObject({
			...selectedObject,
			point: {
				...selectedObject.point,
				hasLineBreak: !selectedObject.point.hasLineBreak,
			}
		})
	}

	const onPasteSectionClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.post(
				`/marks/${mark.id}/mark-general-data-sections/copy`,
				{
					id: copiedSectionId,
				}
			)
			var arr = [...optionsObject.sections]
			arr.push(response.data)
			// arr.sort((v) => v.type.id)
			setOptionsObject({
                ...optionsObject,
                sections: arr,
            })
		} catch (e) {
			setLeftErrMsg(true)
			if (e.response.status === 409) {
				setErrMsg('Данный раздел уже существует')
			} else {
				setErrMsg('Произошла ошибка')
			}
		}
		setProcessIsRunning(false)
	}

	const onSectionDeleteClick = async (row: number, id: number) => {
		setProcessIsRunning(true)
		try {
			await httpClient.delete(
				`/marks/${mark.id}/mark-general-data-sections/${id}`
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
		} catch (e) {
			setLeftErrMsg(true)
			setErrMsg('Произошла ошибка')
		}
		setPopup(defaultPopup)
		setProcessIsRunning(false)
	}

	const onArrowUpClick = async () => {
		if (selectedObject.section == null) {
			return
		}
		if (selectedObject.point == null) {
			if (optionsObject.points.length > 0) {
				onPointSelect(optionsObject.points[optionsObject.points.length - 1].id, true)
				return
			}
		}
		let found = false
		for (let i = optionsObject.points.length - 1; i >= 0; i--) {
			if (found) {
				onPointSelect(optionsObject.points[i].id, true)
				return
			}
			if (selectedObject.point.id == optionsObject.points[i].id) {
				found = true
			}
		}

		found = false
		for (let i = optionsObject.sections.length - 1; i >= 0; i--) {
			if (found) {
				const v = optionsObject.sections[i]
				var pText = ''
				if (optionsObject.sections[i].name == corrProtSectionName) {
					const pointText = await httpClient.get(
						`/marks/${mark.id}/corr-prot-point`
					)
					pText = pointText.data.result
				}
				if (cachedPoints.has(v.id)) {
					const points = cachedPoints.get(v.id)
					setOptionsObject({
						...optionsObject,
						points: points,
					})
					
					if (points.length > 0) {
						setSelectedObject({
							...selectedObject,
							section: v,
							point: points[points.length - 1],
							pointText: pText === '' ? points[points.length - 1].text : pText,
							sectionText: v.name,
						})
						setTimeout(() => Scroll.scroller.scrollTo(`point${points[points.length - 1].id}`, {
							containerId: "srollable-points",
						}), 10)
					} else {
						setSelectedObject({
							...selectedObject,
							section: v,
							point: null,
							pointText: pText,
							sectionText: v.name,
						})
					}
					Scroll.scroller.scrollTo(`section${v.id}`, {
						containerId: "srollable-sections",
					})
				} else {
					try {
						const pointsResponse = await httpClient.get(
							`/mark-general-data-sections/${optionsObject.sections[i].id}/mark-general-data-points`
						)
						const points = pointsResponse.data
						cachedPoints.set(v.id, points)
						setOptionsObject({
							...optionsObject,
							points: points,
						})
						if (points.length > 0) {
							setSelectedObject({
								...selectedObject,
								section: v,
								point: points[points.length - 1],
								pointText: pText === '' ? points[points.length - 1].text : pText,
								sectionText: v.name,
							})
							setTimeout(() => Scroll.scroller.scrollTo(`point${points[points.length - 1].id}`, {
								containerId: "srollable-points",
							}), 10)
						} else {
							setSelectedObject({
								...selectedObject,
								section: v,
								point: null,
								pointText: pText,
								sectionText: v.name,
							})
						}
						Scroll.scroller.scrollTo(`section${v.id}`, {
							containerId: "srollable-sections",
						})
					} catch (e) {
						setLeftErrMsg(true)
						setErrMsg('Произошла ошибка')
					}
				}
				return
			}
			if (selectedObject.section.id == optionsObject.sections[i].id) {
				found = true
			}
		}
	}

	const onArrowDownClick = async () => {
		if (selectedObject.section == null) {
			return
		}
		if (selectedObject.point == null) {
			if (optionsObject.points.length > 0) {
				onPointSelect(optionsObject.points[0].id, true)
				return
			}
		}
		let found = false
		for (let i = 0; i < optionsObject.points.length; i++) {
			if (found) {
				onPointSelect(optionsObject.points[i].id, true)
				return
			}
			if (selectedObject.point.id == optionsObject.points[i].id) {
				found = true
			}
		}

		found = false
		for (let i = 0; i < optionsObject.sections.length; i++) {
			if (found) {
				const v = optionsObject.sections[i]
				var pText = ''
				if (optionsObject.sections[i].name == corrProtSectionName) {
					const pointText = await httpClient.get(
						`/marks/${mark.id}/corr-prot-point`
					)
					pText = pointText.data.result
				}
				if (cachedPoints.has(v.id)) {
					const points = cachedPoints.get(v.id)
					setOptionsObject({
						...optionsObject,
						points: points,
					})
					
					if (points.length > 0) {
						setSelectedObject({
							...selectedObject,
							section: v,
							point: points[0],
							pointText: pText === '' ? points[0].text : pText,
							sectionText: v.name,
						})
						setTimeout(() => Scroll.scroller.scrollTo(`point${points[0].id}`, {
							containerId: "srollable-points",
						}), 10)
					} else {
						setSelectedObject({
							...selectedObject,
							section: v,
							point: null,
							pointText: pText,
							sectionText: v.name,
						})
					}
					Scroll.scroller.scrollTo(`section${v.id}`, {
						containerId: "srollable-sections",
					})
				} else {
					try {
						const pointsResponse = await httpClient.get(
							`/mark-general-data-sections/${optionsObject.sections[i].id}/mark-general-data-points`
						)
						const points = pointsResponse.data
						cachedPoints.set(v.id, points)
						setOptionsObject({
							...optionsObject,
							points: points,
						})
						if (points.length > 0) {
							setSelectedObject({
								...selectedObject,
								section: v,
								point: points[0],
								pointText: pText === '' ? points[0].text : pText,
								sectionText: v.name,
							})
							setTimeout(() => Scroll.scroller.scrollTo(`point${points[0].id}`, {
								containerId: "srollable-points",
							}), 10)
						} else {
							setSelectedObject({
								...selectedObject,
								section: v,
								point: null,
								pointText: pText,
								sectionText: v.name,
							})
						}
						Scroll.scroller.scrollTo(`section${v.id}`, {
							containerId: "srollable-sections",
						})
					} catch (e) {
						setLeftErrMsg(true)
						setErrMsg('Произошла ошибка')
					}
				}
				return
			}
			if (selectedObject.section.id == optionsObject.sections[i].id) {
				found = true
			}
		}
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
					`/marks/${mark.id}/mark-general-data-sections/${selectedObject.section.id}`,
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
					`/marks/${mark.id}/mark-general-data-sections`,
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
				`/mark-general-data-sections/${selectedObject.section.id}/mark-general-data-points/${id}`
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
			setPopup(defaultPopup)
		} catch (e) {
			setLeftErrMsg(false)
			setErrMsg('Произошла ошибка')
		}
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
					`/mark-general-data-sections/${selectedObject.section.id}/mark-general-data-points/${selectedObject.point.id}`,
					{
						text: selectedObject.pointText,
						orderNum: selectedObject.point.orderNum,
						hasLineBreak: selectedObject.point.hasLineBreak,
					}
				)
				const p = { ...selectedObject.point }
				p.text = selectedObject.pointText
				const foundPoint = optionsObject.points.find(
					(v) => v.id === p.id
				)
				foundPoint.text = selectedObject.pointText
				foundPoint.orderNum = selectedObject.point.orderNum
				foundPoint.hasLineBreak = selectedObject.point.hasLineBreak

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
					`/mark-general-data-sections/${selectedObject.section.id}/mark-general-data-points`,
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

	const onDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/general-data-document`,
				{
					responseType: 'blob',
				}
			)
			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_ОД.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
			history.push('/')
		} catch (e) {
			if (e.response != null && e.response.status === 404) {
				setErrMsg('Пожалуйста, заполните условия эскплуатации у марки')
			} else {
				setErrMsg('Произошла ошибка')
			}
			setProcessIsRunning(false)
		}
	}

	return (
		<div className="component-cnt flex-v-cent-h">
			{isSectionsSelectionShown ? (
				<SectionsSelectPopup
					defaultSelectedSectionNames={optionsObject.sections.map(
						(v) => v.name
					)}
					close={() => setSectionsSelectionShown(false)}
					optionsObject={optionsObject}
					setOptionsObject={setOptionsObject}
					selectedObject={selectedObject}
					setSelectedObject={setSelectedObject}
					refresh={refresh}
					setRefresh={setRefresh}
				/>
			) : null}
			{isPointsSelectionShown ? (
				<PointsSelectPopup
					sectionId={selectedObject.section.id}
					sectionName={selectedObject.section.name}
					defaultSelectedPointTexts={optionsObject.points.map(
						(v) => v.text
					)}
					close={() => setPointsSelectionShown(false)}
					optionsObject={optionsObject}
					setOptionsObject={setOptionsObject}
					selectedObject={selectedObject}
					setSelectedObject={setSelectedObject}
					cachedPoints={cachedPoints}
				/>
			) : null}
			<h1 className="text-centered">Состав общих указаний марки</h1>
			<div className="flex">
				<div className="info-area shadow p-3 bg-white rounded component-width component-cnt-div">
					<div className="full-width">
						<div className="flex-v general-data-selection" id="srollable-sections">
							{optionsObject.sections.map((s, index) => {
								return (
									<Scroll.Element key={s.id} name={`section${s.id}`}>
										<div
											className={
												selectedObject.section == null
													? 'pointer selection-text flex'
													: selectedObject.section.id ==
													s.id
													? 'pointer selection-text selected-bg flex'
													: 'pointer selection-text flex'
											}
											onMouseDown={() => onSectionSelect(s.id, s.name)}
										>
											<p
												className="no-bot-mrg"
												style={{ flex: 1 }}
											>
												{(index + 1).toString() +
													'. ' +
													s.name}
											</p>
											<div className="flex">
												<div
													onClick={() => {
														if (selectedObject.section != null) {
															setCopiedSectionId(selectedObject.section.id)
															alert("Раздел скопирован")
														}
													}}
													className="trash-area"
												>
													<Files color="#666" size={22} />
												</div>
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
														<Trash
															color="#666"
															size={22}
														/>
													</div>
												}
											</div>
										</div>
									</Scroll.Element>
								)
							})}
						</div>
					</div>

					<div className="space-between">
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
									selectedObject.section == null
										? true
										: false
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
												value:
													selectedObject.section.id,
												label:
													selectedObject.section
														.orderNum,
										  }
								}
								options={[
									...Array(
										optionsObject.sections.length
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
						{/* <div
							onClick={() => setSectionsSelectionShown(true)}
							className="pointer"
						>
							<PlusCircle
								color={'#666'}
								size={28}
								style={{ marginTop: '1.5em' }}
							/>
						</div> */}


						<div className="flex-cent-v">
							<div
								onClick={
									copiedSectionId === -1
										? null
										: onPasteSectionClick
								}
								className="pointer"
							>
								<ClipboardPlus
									color={copiedSectionId === -1 ? '#ccc' : '#666'}
									size={28}
									className={
										copiedSectionId === -1
											? 'mrg-top'
											: 'pointer mrg-top'
									}
									style={{ marginRight: 10, marginTop: 20 }}
								/>
							</div>
							<div
								onClick={() => setSectionsSelectionShown(true)}
								className="pointer"
							>
								<PlusCircle
									color={'#666'}
									size={28}
									style={{ marginTop: '1.5em' }}
								/>
							</div>
						</div>


					</div>
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

				<div className="flex-v mrg-left">
					<div className="pointer p-1" onClick={onArrowUpClick}>
						<ArrowUpCircle color="#666" size={30} />
					</div>
					<div className="pointer p-1" onClick={onArrowDownClick}>
						<ArrowDownCircle color="#666" size={30} />
					</div>
				</div>

				<div className="shadow p-3 bg-white rounded mrg-left component-width component-cnt-div">
					<div className="full-width">
						<div className="flex-v general-data-selection" id="srollable-points">
							{optionsObject.points.map((p, index) => {
								return (
									<Scroll.Element key={p.id} name={`point${p.id}`}>
										<div
											className={
												selectedObject.point == null
													? 'pointer selection-text flex'
													: selectedObject.point.id ==
													p.id
													? 'pointer selection-text selected-bg flex'
													: 'pointer selection-text flex'
											}
										>
											<p
												className="no-bot-mrg"
												style={{ flex: 1 }}
												onClick={() => onPointSelect(p.id, false)}
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
									</Scroll.Element>
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
							<div
								onClick={
									selectedObject.section == null
										? null
										: () => setPointsSelectionShown(true)
								}
								className={
									selectedObject.section == null
										? ''
										: 'pointer'
								}
							>
								<PlusCircle
									color={
										selectedObject.section == null
											? '#ccc'
											: '#666'
									}
									size={28}
									style={{ marginTop: '1.5em' }}
								/>
							</div>
						</div>
						<Form.Group className="no-bot-mrg">
							<div className="space-between">
								<div className="flex">
									<Form.Label className="bold" htmlFor="text">
										Пункт
									</Form.Label>
									<p style={{ marginLeft: 10 }}>(°C)</p>
								</div>
								<div className="flex">
									<Form.Check
										id="break"
										type="checkbox"
										className="checkmark"
										checked={selectedObject.point != null ? selectedObject.point.hasLineBreak : false }
										disabled={selectedObject.point == null}
										onChange={onLineBreakCheck}
									/>
									<label style={{marginLeft: 10}} htmlFor="break">Разрыв страницы перед пунктом</label>
								</div>
							</div>

							{selectedObject.pointText == "Таблица 1" && selectedObject.point != null ? <img onClick={() => {
								setSelectedObject({
									...selectedObject,
									pointText: "Таблица 1 ",
								})
							}} src={table1} style={{width: "100%", height: "auto"}} /> :
								selectedObject.pointText == "Таблица 2" && selectedObject.point != null ? <img onClick={() => {
									setSelectedObject({
										...selectedObject,
										pointText: "Таблица 2 ",
									})
								}} src={table2} style={{width: "100%", height: "auto"}} /> :
									selectedObject.pointText == "Таблица 3" && selectedObject.point != null ? <img onClick={() => {
										setSelectedObject({
											...selectedObject,
											pointText: "Таблица 3 ",
										})
									}} src={table3} style={{width: "100%", height: "auto"}} /> :
										<Form.Control
										id="text"
										as="textarea"
										rows={8}
										style={{ resize: 'none' }}
										value={selectedObject.pointText}
										onChange={onPointTextChange}
									/>}
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
									processIsRunning || !Object.values({
										text:
											currentPointFromArray == null || selectedObject.pointText === currentPointFromArray.text
												? undefined
												: selectedObject.point.text,
										orderNum:
											currentPointFromArray == null || selectedObject.point.orderNum === currentPointFromArray.orderNum
												? undefined
												: selectedObject.point.orderNum,
										hasLineBreak:
											currentPointFromArray == null || selectedObject.point.hasLineBreak === currentPointFromArray.hasLineBreak
												? undefined
												: selectedObject.point.hasLineBreak,
									}).some((x) => x !== undefined)}
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
			<Button
				variant="secondary"
				className="full-width btn-mrg-top-2"
				onClick={onDownloadButtonClick}
				disabled={processIsRunning}
			>
				Скачать документ
			</Button>
		</div>
	)
}

export default MarkGeneralData
