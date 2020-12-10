// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import GeneralDataModel from '../../model/GeneralDataModel'
import GeneralDataSection from '../../model/GeneralDataSection'
import GeneralDataPoint from '../../model/GeneralDataPoint'
import { useMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
import './SectionsSelectPopup.css'

// export type IGenerealDataPopupObj = {
// 	isShown: boolean
// 	msg: string
// 	onAccept: () => void
// 	onCancel: () => void
// }

// export const defaultPopupObj = {
// 	isShown: false,
// 	msg: '',
// 	onAccept: null,
// 	onCancel: null,
// } as IGenerealDataPopupObj

type PopupProps = {
    isShown: boolean
    close: () => void
}

const SectionsSelectPopup = ({ isShown, close }: PopupProps) => {
	const user = useUser()
	const mark = useMark()

	const [section, setSection] = useState<GeneralDataSection>(null)
	const [sections, setSections] = useState<GeneralDataSection[]>([])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sectionsResponse = await httpClient.get(
						`/general-data-sections`
					)
					setSections(sectionsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	return isShown ? (
		<div className="div-container component-cnt-div white-bg sections-popup shadow p-3 mb-5 rounded">
			<div className="full-width">
				<label className="bold no-bot-mrg">Разделы</label>
				<div className="flex-v general-data-selection mrg-top">
					{sections.map((s) => {
						return (
                            <div
                                className="pointer selection-text flex-cent-v"
                                key={s.id}
                                onClick={() => console.log('11')}
                            >
                                <p className="no-bot-mrg" style={{flex: 1}}>
                                    {s.name}
                                </p>
                                <div
                                    className="check-area"
                                >
                                    <Form.Check
                                        id="flangedConnections"
                                        type="checkbox"
                                        className="checkmark"
                                        onChange={null}
                                    />
                                </div>
                            </div>
							// <div
							// 	className={
							// 		selectedObject.section == null
							// 			? 'pointer selection-text'
							// 			: selectedObject.section.id == s.id
							// 			? 'pointer selection-text selected-bg'
							// 			: 'pointer selection-text'
							// 	}
							// 	onClick={null}
							// 	key={s.id}
							// >
							// 	<p className="no-bot-mrg">{s.name}</p>
							// </div>
						)
					})}
				</div>
			</div>
            <div className="flex btns-mrg full-width mrg-top-2">
				<Button
					variant="secondary"
					className="flex-grow"
					onClick={null}
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
	) : null
}

export default SectionsSelectPopup
