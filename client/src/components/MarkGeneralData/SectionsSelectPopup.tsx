// Global
import React, { useState, useEffect, createRef } from 'react'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import GeneralDataSection from '../../model/GeneralDataSection'
import GeneralDataPoint from '../../model/GeneralDataPoint'
import { useMark } from '../../store/MarkStore'

type IOptionsObject = {
	sections: GeneralDataSection[]
	points: GeneralDataPoint[]
}

type ISelectionObject = {
	section: GeneralDataSection
	point: GeneralDataPoint
	pointText: string
}

type PopupProps = {
	defaultSelectedSectionIds: number[]
	close: () => void
	optionsObject: IOptionsObject
	setOptionsObject: (optionObject: IOptionsObject) => void
	selectedObject: ISelectionObject
	setSelectedObject: (selectionObject: ISelectionObject) => void
}

const SectionsSelectPopup = ({
	defaultSelectedSectionIds,
	close,
	optionsObject,
	setOptionsObject,
	selectedObject,
	setSelectedObject,
}: PopupProps) => {
	const mark = useMark()

	const [sections, setSections] = useState<GeneralDataSection[]>([])
	const [selectedSections, setSelectedSections] = useState<
		GeneralDataSection[]
	>([])

	const refs = useState([] as React.MutableRefObject<undefined>[])[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (refs.length > 0 && sections.length > 0) {
				for (const [i, s] of sections.entries()) {
					if (defaultSelectedSectionIds.includes(s.id)) {
						const inputElement = refs[i].current as any
						if (inputElement) {
							inputElement.checked = true
							selectedSections.push(sections[i])
						}
					}
				}
				return
			}
			const fetchData = async () => {
				try {
					const sectionsResponse = await httpClient.get(
						`/general-data-sections`
					)
					for (let _ of sectionsResponse.data) {
						refs.push(createRef())
					}
					setSections(sectionsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark, sections])

	const onSectionClick = (row: number, id: number) => {
		const inputElement = refs[row].current as any
		if (inputElement) {
			inputElement.checked = !inputElement.checked
			if (inputElement.checked) {
				selectedSections.push(sections[row])
			} else {
				const index = selectedSections.map((v) => v.id).indexOf(id)
				selectedSections.splice(index, 1)
			}
		}
	}

	const onSaveButtonClick = async () => {
		try {
			await httpClient.patch(
				`/marks/${mark.id}/general-data-points`,
				selectedSections.map((v) => v.id)
			)
			setOptionsObject({
				...optionsObject,
				sections: selectedSections.sort((a, b) => a.id - b.id),
				points: [],
			})
			setSelectedObject({
				...selectedObject,
				section: null,
				point: null,
			})
			close()
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="div-container component-cnt-div white-bg selection-popup shadow p-3 mb-5 rounded">
			<div className="full-width">
				<label className="bold no-bot-mrg">Разделы</label>
				<div className="flex-v general-data-selection mrg-top">
					{sections.map((s, index) => {
						return (
							<div
								className="pointer selection-text flex"
								key={s.id}
								onClick={() => onSectionClick(index, s.id)}
							>
								<p className="no-bot-mrg" style={{ flex: 1 }}>
									{s.name}
								</p>
								<div className="check-area">
									<Form.Check
										ref={refs[index]}
										type="checkbox"
										className="checkmark"
										style={{ pointerEvents: 'none' }}
									/>
								</div>
							</div>
						)
					})}
				</div>
			</div>
			<div className="flex btns-mrg full-width mrg-top-2">
				<Button
					variant="secondary"
					className="flex-grow"
					onClick={onSaveButtonClick}
				>
					ОК
				</Button>
				<Button
					variant="secondary"
					className="flex-grow mrg-left"
					onClick={close}
				>
					Отмена
				</Button>
			</div>
		</div>
	)
}

export default SectionsSelectPopup
