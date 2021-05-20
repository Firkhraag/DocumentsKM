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
	sectionId: number
	sectionName: string
	defaultSelectedPointTexts: string[]
	close: () => void
	optionsObject: IOptionsObject
	setOptionsObject: (optionsObject: IOptionsObject) => void
	selectedObject: ISelectionObject
	setSelectedObject: (selectedObject: ISelectionObject) => void
    cachedPoints: Map<number, GeneralDataPoint[]>
}

const PointsSelectPopup = ({
	sectionId,
	sectionName,
	defaultSelectedPointTexts,
	close,
	optionsObject,
    setOptionsObject,
	selectedObject,
	setSelectedObject,
	cachedPoints,
}: PopupProps) => {
	const mark = useMark()

	const [points, setPoints] = useState<GeneralDataPoint[]>([])
	const [selectedPoints, setSelectedPoints] = useState<GeneralDataPoint[]>([])

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const refs = useState([] as React.MutableRefObject<undefined>[])[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
            if (points.length > 0) {
				for (const [i, s] of points.entries()) {
					if (defaultSelectedPointTexts.includes(s.text)) {
						const inputElement = refs[i].current as any
						if (inputElement) {
							inputElement.checked = true
							selectedPoints.push(points[i])
						}
					}
				}
				return
			}
			const fetchData = async () => {
				try {
					const pointsResponse = await httpClient.get(
						`/general-data-section-names/${sectionName}/general-data-points`
					)
					for (let _ of pointsResponse.data) {
						refs.push(createRef())
					}
                    if (pointsResponse.data.length > 0) {
                        setPoints(pointsResponse.data)
                    }
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
			inputElement.checked = !inputElement.checked
			if (inputElement.checked) {
				selectedPoints.push(points[row])
			} else {
				const index = selectedPoints.map((v) => v.id).indexOf(id)
				selectedPoints.splice(index, 1)
			}
		}
	}

	const onSaveButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const pointsResponse = await httpClient.patch(
				`/mark-general-data-sections/${sectionId}/mark-general-data-points`,
				selectedPoints.map((v) => v.id)
			)
			setSelectedObject({
				...selectedObject,
				point: null,
			})
            cachedPoints.delete(sectionId)
            setOptionsObject({
                ...optionsObject,
                points:  pointsResponse.data,
            })
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
					{points.map((p, index) => {
						return (
							<div
								className="pointer selection-text flex"
								key={p.id}
								onClick={() => onPointClick(index, p.id)}
							>
								<p className="no-bot-mrg" style={{ flex: 1 }}>
									{p.text}
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

export default PointsSelectPopup
