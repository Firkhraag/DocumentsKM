// Global
import React, { useState, useEffect, createRef } from 'react'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import GeneralDataPoint from '../../model/GeneralDataPoint'
import { useMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
// import './pointsSelectPopup.css'

type PopupProps = {
    sectionId: number
    defaultSelectedPointIds: number[]
    close: () => void
}

const PointsSelectPopup = ({ sectionId, defaultSelectedPointIds, close }: PopupProps) => {
    const mark = useMark()
    const user = useUser()

	const [points, setPoints] = useState<GeneralDataPoint[]>([])
    const [selectedPoints, setSelectedPoints] = useState<GeneralDataPoint[]>([])
    
    const refs = useState([] as React.MutableRefObject<undefined>[])[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
            if (
				refs.length > 0 &&
				points.length > 0
			) {
                console.log(defaultSelectedPointIds)
				for (const [i, s] of points.entries()) {
					if (defaultSelectedPointIds.includes(s.id)) {
						const inputElement = refs[i].current as any
						if (inputElement) {
							inputElement.checked = true
						}
					}
				}
				return
			}
			const fetchData = async () => {
				try {
					const pointsResponse = await httpClient.get(
						`/users/${user.id}/general-data-sections/${sectionId}/general-data-points`
                    )
                    for (let _ of pointsResponse.data) {
                        refs.push(createRef())
                    }
					setPoints(pointsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
            fetchData()
		}
    }, [mark, points])
    
    const onPointClick = (row: number, id: number) => {
        const inputElement = refs[row].current as any
        if (inputElement) {
            inputElement.checked = !(inputElement.checked)
        }
    }

	return (
		<div className="div-container component-cnt-div white-bg selection-popup shadow p-3 mb-5 rounded">
			<div className="full-width">
				<label className="bold no-bot-mrg">Разделы</label>
				<div className="flex-v general-data-selection mrg-top">
					{points.map((p, index) => {
						return (
                            <div
                                className="pointer selection-text flex"
                                key={p.id}
                                onClick={() => onPointClick(index, p.id)}
                            >
                                <p className="no-bot-mrg" style={{flex: 1}}>
                                    {p.text}
                                </p>
                                <div
                                    className="check-area"
                                >
                                    <Form.Check
                                        ref={refs[index]}
                                        type="checkbox"
                                        className="checkmark"
                                        style={{pointerEvents: 'none'}}
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
	)
}

export default PointsSelectPopup
