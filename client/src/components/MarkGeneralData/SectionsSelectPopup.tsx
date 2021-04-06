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
import { useUser } from '../../store/UserStore'
import ErrorMsg from '../ErrorMsg/ErrorMsg'

type IOptionsObject = {
	sections: GeneralDataSection[]
	points: GeneralDataPoint[]
}

type ISelectionObject = {
	section: GeneralDataSection
	point: GeneralDataPoint
	sectionText: string
	pointText: string
}

type PopupProps = {
	defaultSelectedSectionNames: string[]
	close: () => void
	optionsObject: IOptionsObject
	setOptionsObject: (optionObject: IOptionsObject) => void
	selectedObject: ISelectionObject
	setSelectedObject: (selectionObject: ISelectionObject) => void
    refresh: boolean
    setRefresh: (r: boolean) => void
}

const SectionsSelectPopup = ({
	defaultSelectedSectionNames,
	close,
	optionsObject,
	setOptionsObject,
	selectedObject,
	setSelectedObject,
    refresh,
    setRefresh,
}: PopupProps) => {
	const mark = useMark()
    const user = useUser()

	const [sections, setSections] = useState<GeneralDataSection[]>([])
	const [selectedSections, setSelectedSections] = useState<
		GeneralDataSection[]
	>([])

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const refs = useState([] as React.MutableRefObject<undefined>[])[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
            if (sections.length > 0) {
				for (const [i, s] of sections.entries()) {
					if (defaultSelectedSectionNames.includes(s.name)) {
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
						`/users/${user.id}/general-data-sections`
					)
					for (let _ of sectionsResponse.data) {
						refs.push(createRef())
					}
                    if (sectionsResponse.data.length > 0) {
                        setSections(sectionsResponse.data)
                    }
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
		setProcessIsRunning(true)
		try {
			await httpClient.patch(
				`users/${user.id}/marks/${mark.id}/mark-general-data-sections`,
				selectedSections.map((v) => v.id)
			)
            setSelectedObject({
                ...selectedObject,
                point: null,
                section: null,
            })
            setOptionsObject({
                sections: [],
                points: [],
            })
            setRefresh(!refresh)
			close()
		} catch (e) {
			setErrMsg('Произошла ошибка')
			setProcessIsRunning(false)
		}
	}

	return (
		<div className="div-container component-cnt-div white-bg selection-popup shadow p-3 mb-5 rounded">
			<div className="full-width">
				<label className="bold no-bot-mrg">Разделы</label>
				<div className="flex-v general-data-selection mrg-top" style={{height: 333}}>
					{sections.map((s, index) => {
						return (
							<div
								className="pointer selection-text flex"
								key={s.id}
								onClick={() => onSectionClick(index, s.id)}
							>
								<p className="no-bot-mrg" style={{ flex: 1 }}>
									{(index + 1).toString() + '. ' + s.name}
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
			<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
			<div className="flex btns-mrg full-width mrg-top-2">
				<Button
					variant="secondary"
					className="flex-grow"
					onClick={onSaveButtonClick}
					disabled={processIsRunning}
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
