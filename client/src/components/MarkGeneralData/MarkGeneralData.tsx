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
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'
import truncateText from '../../util/truncate'
import SectionsSelectPopup from './SectionsSelectPopup'
import PointsSelectPopup from './PointsSelectPopup'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

const MarkGeneralData = () => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [selectedObject, setSelectedObject] = useState<GeneralDataModel>({
		section: null,
		point: null,
		pointText: '',
	})
	const [optionsObject, setOptionsObject] = useState({
		sections: [] as GeneralDataSection[],
		points: [] as GeneralDataPoint[],
	})

	const [isSectionsSelectionShown, setSectionsSelectionShown] = useState(
		false
	)
	const [isPointsSelectionShown, setPointsSelectionShown] = useState(false)
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

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sectionsResponse = await httpClient.get(
						`/marks/${mark.id}/general-data-sections`
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
		}
	}, [mark])

	const onSectionSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				sections: optionsObject.sections,
				points: [],
			})
			setSelectedObject({
				...selectedObject,
				section: null,
				point: null,
				pointText: '',
			})
			return
		}
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
				})
			} else {
				try {
					const pointsResponse = await httpClient.get(
						`/marks/${mark.id}/general-data-sections/${id}/general-data-points`
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
						pointText: '',
					})
				} catch (e) {
					setErrMsg('Произошла ошибка')
				}
			}
		}
	}

	const onPointSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				point: null,
			})
			return
		}
		const v = getFromOptions(id, optionsObject.points, selectedObject.point)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				point: v,
				pointText: v.text,
			})
			window.scrollTo(0, document.body.scrollHeight / 2)
		}
	}

	const onPointTextChange = (
		event: React.ChangeEvent<HTMLTextAreaElement>
	) => {
		setSelectedObject({
			...selectedObject,
			pointText: event.currentTarget.value,
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

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(
				`/marks/${mark.id}/general-data-sections/${selectedObject.section.id}/general-data-points/${id}`
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
			setPopup(defaultPopup)
		} catch (e) {
			setErrMsg('Произошла ошибка')
		}
	}

	const checkIfValid = () => {
		if (selectedObject.section === null) {
			setErrMsg('Пожалуйста, выберите раздел')
			return false
		}
		if (selectedObject.pointText === '') {
			setErrMsg('Пожалуйста, введите содержание пункта')
			return false
		}
		return true
	}

	const onUpdatePointButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(
					`/marks/${mark.id}/general-data-sections/${selectedObject.section.id}/general-data-points/${selectedObject.point.id}`,
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
				window.scrollTo(0, 0)
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Пункт с таким содержанием уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
			}
		}
	}

	const onCreatePointButtonClick = async () => {
		if (checkIfValid()) {
			try {
				const response = await httpClient.post(
					`/marks/${mark.id}/general-data-sections/${selectedObject.section.id}/general-data-points`,
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
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Пункт с таким содержанием уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
			}
		}
	}

	const onDownloadButtonClick = async () => {
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
			link.setAttribute('download', `${mark.code}_ОД.docx`)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
            if (e.response != null && e.response.status === 404) {
                setErrMsg('Пожалуйста, заполните условия эскплуатации у марки')
                return
            }
			setErrMsg('Произошла ошибка')
		}
	}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			{isSectionsSelectionShown ? (
				<SectionsSelectPopup
					defaultSelectedSectionIds={optionsObject.sections.map(
						(v) => v.id
					)}
					close={() => setSectionsSelectionShown(false)}
					optionsObject={optionsObject}
					setOptionsObject={setOptionsObject}
					selectedObject={selectedObject}
					setSelectedObject={setSelectedObject}
				/>
			) : null}
			{isPointsSelectionShown ? (
				<PointsSelectPopup
					sectionId={selectedObject.section.id}
					defaultSelectedPointTexts={optionsObject.points.map(
						(v) => v.text
					)}
					close={() => setPointsSelectionShown(false)}
					optionsObject={optionsObject}
					selectedObject={selectedObject}
					setSelectedObject={setSelectedObject}
				/>
			) : null}
			<h1 className="text-centered">Состав общих указаний марки</h1>

			<div className="flex">
				<div className="info-area shadow p-3 bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label className="bold" htmlFor="sections">
							Выбранный раздел
						</Form.Label>
						<Select
							inputId="sections"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder=""
							noOptionsMessage={() => 'Разделы не найдены'}
							onChange={(selectedOption) =>
								onSectionSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.section == null
									? null
									: {
											value: selectedObject.section.id,
											label: selectedObject.section.name,
									  }
							}
							options={optionsObject.sections.map((s) => {
								return {
									value: s.id,
									label: s.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<div className="full-width">
						<label className="bold no-bot-mrg">Разделы</label>
						<div className="flex-v general-data-selection mrg-top">
							{optionsObject.sections.map((s, index) => {
								return (
									<div
										className={
											selectedObject.section == null
												? 'pointer selection-text'
												: selectedObject.section.id ==
												  s.id
												? 'pointer selection-text selected-bg'
												: 'pointer selection-text'
										}
										onClick={() => onSectionSelect(s.id)}
										key={s.id}
									>
										<p className="no-bot-mrg">
											{(index + 1).toString() +
												'. ' +
												s.name}
										</p>
									</div>
								)
							})}
						</div>
					</div>

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={() => setSectionsSelectionShown(true)}
					>
						Изменить
					</Button>
				</div>

				<div className="shadow p-3 bg-white rounded mrg-left component-width component-cnt-div">
					<Form.Group>
						<Form.Label className="bold" htmlFor="points">
							Выбранный пункт
						</Form.Label>
						<Select
							inputId="points"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder=""
							noOptionsMessage={() => 'Пункты не найдены'}
							onChange={(selectedOption) =>
								onPointSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.point == null
									? null
									: {
											value: selectedObject.point.id,
											label: selectedObject.point.text,
									  }
							}
							options={optionsObject.points.map((p) => {
								return {
									value: p.id,
									label: p.text,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<div className="full-width">
						<label className="bold no-bot-mrg">Пункты</label>
						<div className="flex-v general-data-selection mrg-top">
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
											{truncateText(p.text, 100, null)}
										</p>
										<div
											onClick={() =>
												setPopup({
													isShown: true,
													msg: `Вы действительно хотите удалить пункт "${truncateText(
														p.text,
														33,
														null
													)}"?`,
													onAccept: () =>
														onDeleteClick(
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
					</div>

					{/* <Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
                        onClick={onCreatePointButtonClick}
                        disabled={selectedObject.section == null ? true : false}
					>
						Просмотр
					</Button> */}
					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={() => setPointsSelectionShown(true)}
						disabled={selectedObject.section == null ? true : false}
					>
						Изменить
					</Button>
				</div>
			</div>

			<div className="shadow p-3 mb-5 bg-white rounded component-width-4 component-cnt-div mrg-top-2">
				<Form.Group>
					<Form.Label className="bold" htmlFor="section_title">
						Название раздела
					</Form.Label>
					<Form.Control
						id="section_title"
						type="text"
						value={
							selectedObject.section == null
								? ''
								: selectedObject.section.name
						}
						readOnly={true}
					/>
				</Form.Group>
				<Form.Group className="flex-cent-v">
					<Form.Label
						className="bold no-bot-mrg"
						htmlFor="orderNum"
						style={{ marginRight: '2.9em' }}
					>
						Номер пункта
					</Form.Label>
					<Select
						inputId="orderNum"
						maxMenuHeight={250}
						isSearchable={true}
						placeholder=""
						noOptionsMessage={() => 'Номер не найден'}
						className="num-field-width"
						isDisabled={selectedObject.point == null ? true : false}
						onChange={(selectedOption) =>
							onPointNumChange((selectedOption as any)?.value)
						}
						value={
							selectedObject.point == null
								? null
								: {
										value: selectedObject.point.id,
										label: selectedObject.point.orderNum,
								  }
						}
						options={[
							...Array(optionsObject.points.length).keys(),
						].map((v) => {
							return {
								value: v + 1,
								label: v + 1,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>
				<Form.Group className="no-bot-mrg mrg-top-2">
					<Form.Label className="bold" htmlFor="text">
						Содержание пункта
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
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
				<div className="flex btn-mrg-top-2">
					<Button
						variant="secondary"
						className="flex-grow"
						onClick={onUpdatePointButtonClick}
						disabled={selectedObject.point == null ? true : false}
					>
						Сохранить изменения
					</Button>
					<Button
						variant="secondary"
						className="flex-grow mrg-left"
						onClick={onCreatePointButtonClick}
						disabled={createBtnDisabled ? true : false}
					>
						Добавить новый пункт
					</Button>
				</div>

				<Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
					onClick={onDownloadButtonClick}
				>
					Скачать документ
				</Button>
			</div>
		</div>
	)
}

export default MarkGeneralData
