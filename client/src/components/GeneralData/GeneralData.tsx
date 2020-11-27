// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import GeneralDataSection from '../../model/GeneralDataSection'
import EnvAggressiveness from '../../model/EnvAggressiveness'
import OperatingArea from '../../model/OperatingArea'
import GasGroup from '../../model/GasGroup'
import ConstructionMaterial from '../../model/ConstructionMaterial'
import PaintworkType from '../../model/PaintworkType'
import HighTensileBoltsType from '../../model/HighTensileBoltsType'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectstyle } from '../../util/react-select-style'
import './GeneralData.css'

const GeneralData = () => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<string>('')
	const [optionsObject, setOptionsObject] = useState({
		sections: [] as GeneralDataSection[],
		points: [] as any[],
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sectionsResponse = await httpClient.get(
						`/general-data-sections`
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
		try {
			const pointsResponse = await httpClient.get(
				`/general-data-sections/${id}/general-data-points`
			)
			setOptionsObject({
				...optionsObject,
				points: pointsResponse.data,
			})
			setSelectedObject(
				pointsResponse.data.length === 1
					? pointsResponse.data[0].text == null
						? ''
						: pointsResponse.data[0].text
					: ''
			)
		} catch (e) {
			console.log('Failed to fetch the data')
		}
	}

	const onPointSelect = async (text: string) => {
		setSelectedObject(text == null ? '' : text)
	}

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
            const response = await httpClient.get(`/marks/${mark.id}/general-data`)
            const blob = new Blob([response.data], { type: 'application/x-tex' })
            const link = document.createElement('a')
            link.href = window.URL.createObjectURL(blob)
            link.download = 'Report.tex'
            link.click()
            link.remove()
        } catch (e) {
            console.log('Failed to download the file')
        }
	}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Состав общих указаний</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-3 component-cnt-div">
				<div className="flex">
					<div className="full-width">
						<label className="bold no-bot-mrg">Разделы</label>
						<div className="flex-v general-data-selection mrg-top">
							{optionsObject.sections.map((s) => {
								return (
									<div
										className="pointer"
										onClick={() => onSectionSelect(s.id)}
										key={s.id}
									>
										{s.name}
									</div>
								)
							})}
						</div>
					</div>
					<div className="full-width mrg-left-2">
						<label className="bold no-bot-mrg">Пункты</label>
						<div className="flex-v general-data-selection mrg-top">
							{optionsObject.points.map((p) => {
								return (
									<div
										className="pointer"
										onClick={() => onPointSelect(p.text)}
										key={p.id}
									>
										{p.title}
									</div>
								)
							})}
						</div>
					</div>
				</div>
				<Form.Group className="mrg-top-2">
					<Form.Control
						id="name"
						as="textarea"
						rows={8}
						style={{ resize: 'none' }}
						value={selectedObject}
						onChange={null}
					/>
				</Form.Group>
                <Button
                    variant="secondary"
                    className="btn-mrg-top-2 full-width"
                    onClick={onDownloadButtonClick}
                >
                    Скачать
                </Button>
			</div>
		</div>
	)
}

export default GeneralData
