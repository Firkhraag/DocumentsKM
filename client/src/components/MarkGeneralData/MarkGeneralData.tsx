// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
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
import { reactSelectstyle } from '../../util/react-select-style'
import truncateText from '../../util/truncate'
import SectionsSelectPopup from './SectionsSelectPopup'
import './MarkGeneralData.css'

const MarkGeneralData = () => {
	const history = useHistory()
	const mark = useMark()

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
	const cachedPoints = useState(new Map<number, GeneralDataPoint[]>())[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					// const sectionsResponse = await httpClient.get(
					// 	`/general-data-sections`
					// )
					// setOptionsObject({
					// 	...optionsObject,
					// 	sections: sectionsResponse.data,
					// })
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
				})
			} else {
				try {
					// const pointsResponse = await httpClient.get(
					// 	`/general-data-sections/${id}/general-data-points`
					// )
					// cachedPoints.set(v.id, pointsResponse.data)
					// setOptionsObject({
					// 	...optionsObject,
					// 	points: pointsResponse.data,
					// })
					// setSelectedObject({
					//     ...selectedObject,
					// 	section: v,
					//     point: null,
					// })
				} catch (e) {
					console.log('Failed to fetch the data')
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

	const onSaveButtonClick = async () => {
		if (selectedObject.pointText != '') {
			try {
				// await httpClient.patch(
				// 	`/marks/${mark.id}/mark-operating-conditions`,
				// 	{
				// 		safetyCoeff:
				// 			selectedObject.safetyCoeff ===
				// 			defaultSelectedObject.safetyCoeff
				// 				? undefined
				// 				: selectedObject.safetyCoeff,
				// 		temperature:
				// 			selectedObject.temperature ===
				// 			defaultSelectedObject.temperature
				// 				? undefined
				// 				: selectedObject.temperature,
				// 		envAggressivenessId:
				// 			selectedObject.envAggressiveness.id ===
				// 			defaultSelectedObject.envAggressiveness.id
				// 				? undefined
				// 				: selectedObject.envAggressiveness.id,
				// 		operatingAreaId:
				// 			selectedObject.operatingArea.id ===
				// 			defaultSelectedObject.operatingArea.id
				// 				? undefined
				// 				: selectedObject.operatingArea.id,
				// 		gasGroupId:
				// 			selectedObject.gasGroup.id ===
				// 			defaultSelectedObject.gasGroup.id
				// 				? undefined
				// 				: selectedObject.gasGroup.id,
				// 		constructionMaterialId:
				// 			selectedObject.constructionMaterial.id ===
				// 			defaultSelectedObject.constructionMaterial.id
				// 				? undefined
				// 				: selectedObject.constructionMaterial.id,
				// 		paintworkTypeId:
				// 			selectedObject.paintworkType.id ===
				// 			defaultSelectedObject.paintworkType.id
				// 				? undefined
				// 				: selectedObject.paintworkType.id,
				// 		highTensileBoltsTypeId:
				// 			selectedObject.highTensileBoltsType.id ===
				// 			defaultSelectedObject.highTensileBoltsType.id
				// 				? undefined
				// 				: selectedObject.highTensileBoltsType.id,
				// 	}
				// )
				// history.push('/')
			} catch (e) {
				// setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onCreateSectionButtonClick = async () => {}

	const onCreatePointButtonClick = async () => {}

	const onDownloadButtonClick = async () => {
		// node-latex
		// Node worker that will be doing this task

		// const input = fs.createReadStream('input.tex')
		// const output = fs.createWriteStream('output.pdf')
		// const pdf = latex(input)

		// pdf.pipe(output)
		// pdf.on('error', err => console.error(err))
		// pdf.on('finish', () => console.log('PDF generated!'))
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/general-data`
			)
			const blob = new Blob([response.data], {
				type: 'application/x-tex',
			})
			const link = document.createElement('a')
			link.href = window.URL.createObjectURL(blob)
			link.download = 'Общие данные.tex'
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
	}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<SectionsSelectPopup
				isShown={isSectionsSelectionShown}
				close={() => setSectionsSelectionShown(false)}
			/>
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
							styles={reactSelectstyle}
						/>
					</Form.Group>

					<div className="full-width">
						<label className="bold no-bot-mrg">Разделы</label>
						<div className="flex-v general-data-selection mrg-top">
							{optionsObject.sections.map((s) => {
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
										<p className="no-bot-mrg">{s.name}</p>
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
							styles={reactSelectstyle}
						/>
					</Form.Group>

					<div className="full-width">
						<label className="bold no-bot-mrg">Пункты</label>
						<div className="flex-v general-data-selection mrg-top">
							{optionsObject.points.map((p) => {
								return (
									<div
										className={
											selectedObject.point == null
												? 'pointer selection-text space-between'
												: selectedObject.point.id ==
												  p.id
												? 'pointer selection-text selected-bg space-between'
												: 'pointer selection-text space-between'
										}
										onClick={() => onPointSelect(p.id)}
										key={p.id}
									>
										<p className="no-bot-mrg">
											{truncateText(p.text, 100, null)}
										</p>
										<div className="trash-area">
											<Trash color="#666" size={22} />
										</div>
									</div>
								)
							})}
						</div>
					</div>

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={onCreatePointButtonClick}
					>
						Добавить
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
				<div className="flex btn-mrg-top-2">
					<Button
						variant="secondary"
						className="flex-grow"
						onClick={onSaveButtonClick}
						disabled={selectedObject.point == null ? true : false}
					>
						Сохранить изменения
					</Button>
					<Button
						variant="secondary"
						className="flex-grow mrg-left"
						onClick={onDownloadButtonClick}
					>
						Скачать документ
					</Button>
				</div>
			</div>
		</div>
	)
}

export default MarkGeneralData
