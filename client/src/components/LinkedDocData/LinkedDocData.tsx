// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import LinkedDoc from '../../model/LinkedDoc'
import SheetName from '../../model/SheetName'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

type LinkedDocDataProps = {
	linkedDoc: LinkedDoc
	isCreateMode: boolean
}

const LinkedDocData = ({ linkedDoc, isCreateMode }: LinkedDocDataProps) => {
	const defaultOptionsObject = {
		sheetNames: [] as SheetName[],
		employees: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()

	// const [selectedObject, setSelectedObject] = useState<Sheet>(sheet)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (!isCreateMode && linkedDoc.id === -1) {
				history.push('/linked-docs')
				return
			}
			const fetchData = async () => {
				try {
					const sheetNamesFetchedResponse = await httpClient.get(
						`/sheet-names`
					)
					const sheetNamesFetched = sheetNamesFetchedResponse.data
					const employeesFetchedResponse = await httpClient.get(
						`/departments/${mark.department.id}/employees`
					)
					const employeesFetched = employeesFetchedResponse.data
					setOptionsObject({
						sheetNames: sheetNamesFetched,
						employees: employeesFetched,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			// fetchData()
		}
	}, [mark])

	return (mark == null && !isCreateMode) ? null : (
		<div className="component-cnt">
			<h1 className="text-centered">
				{isCreateMode ? 'Добавление ссылочного документа' : 'Данные ссылочного документа'}
			</h1>
			<div className="flex">
				<div className="info-area shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label>Обозначение</Form.Label>
						<Form.Control
							type="text"
							value={'1'}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Наименование</Form.Label>
						<Form.Control
							as="textarea"
                            rows={4}
                            style={{resize: 'none'}}
							value={'2'}
							readOnly={true}
						/>
					</Form.Group>
				</div>

				<div className="shadow p-3 mb-5 bg-white rounded mrg-left component-width component-cnt-div">
                    <div className="bold">Вид</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите вид ссылочного документа"
						noOptionsMessage={() => 'Виды не найдены'}
						className="mrg-top"
						onChange={null}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>

					<div className="bold mrg-top-2">Шифр</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите шифр ссылочного документа"
						noOptionsMessage={() => 'Ссылочные документы не найдены'}
						className="mrg-top"
						onChange={null}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							isCreateMode
								? null
								: null
						}
					>
						{isCreateMode ? 'Добавить ссылочный документ' : 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default LinkedDocData
