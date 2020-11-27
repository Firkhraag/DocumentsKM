// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import GeneralDataSection from '../../model/GeneralDataSection'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import getFromOptions from '../../util/get-from-options'
import { reactSelectstyle } from '../../util/react-select-style'

const UserGeneralData = () => {
	const [selectedObject, setSelectedObject] = useState<GeneralDataSection>(null)
	const [optionsObject, setOptionsObject] = useState({
		sections: [] as GeneralDataSection[],
		// points: [] as GeneralDataPoint[],
	})

	useEffect(() => {
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
	}, [])

	const onSectionSelect = async (id: number) => {
        if (id == null) {
			setSelectedObject(null)
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.sections,
			selectedObject
		)
		if (v != null) {
			setSelectedObject(v)
		}
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
	}

	return (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Состав общих указаний</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-3 component-cnt-div">
            <label className="bold no-bot-mrg" htmlFor="sections">Разделы</label>
                <Select
                    inputId="sections"
                    maxMenuHeight={250}
                    isClearable={true}
                    isSearchable={true}
                    placeholder="Выберите раздел общих указаний"
                    noOptionsMessage={() => 'Разделы не найдены'}
                    className="mrg-top"
                    onChange={(selectedOption) =>
                        onSectionSelect((selectedOption as any)?.value)
                    }
                    value={
                        selectedObject == null
                        ? null
                        : {
                                value: selectedObject.id,
                                label: selectedObject.name,
                          }}
                    options={optionsObject.sections.map((s) => {
                        return {
                            value: s.id,
                            label: s.name,
                        }
                    })}
                    styles={reactSelectstyle}
                />
				<Form.Group className="mrg-top-2">
					<Form.Control
						id="name"
						as="textarea"
						rows={8}
						style={{ resize: 'none' }}
						value={''}
						onChange={null}
					/>
				</Form.Group>
                <Button
                    variant="secondary"
                    className="btn-mrg-top-2 full-width"
                    onClick={onDownloadButtonClick}
                >
                    Сохранить изменения
                </Button>
			</div>
		</div>
	)
}

export default UserGeneralData
