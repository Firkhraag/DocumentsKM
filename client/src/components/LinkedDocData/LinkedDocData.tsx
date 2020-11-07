// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import MarkLinkedDoc from '../../model/MarkLinkedDoc'
import LinkedDoc from '../../model/LinkedDoc'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectstyle } from '../../util/react-select-style'
import LinkedDocType from '../../model/LinkedDocType'

type LinkedDocDataProps = {
	markLinkedDoc: MarkLinkedDoc
	isCreateMode: boolean
}

const LinkedDocData = ({ markLinkedDoc, isCreateMode }: LinkedDocDataProps) => {
	const defaultOptionsObject = {
		types: [] as LinkedDocType[],
		docs: [] as LinkedDoc[],
	}
	const defaultSelectedDoc = {
		id: -1,
		code: '',
		name: '',
		designation: '',
		type: null,
	} as LinkedDoc

	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<MarkLinkedDoc>(
		isCreateMode
			? {
					id: -1,
					linkedDoc: {
						id: -1,
						code: '',
						name: '',
						designation: '',
						type: null,
					},
			  }
			: markLinkedDoc
	)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const cachedDocs = useState(new Map<number, LinkedDoc[]>())[0]

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (!isCreateMode && markLinkedDoc.id === -1) {
				history.push('/linked-docs')
				return
			}
			const fetchData = async () => {
				try {
					const linkedDocTypesResponse = await httpClient.get(
						`/linked-doc-types`
					)
					if (!isCreateMode) {
						const linkedDocsResponse = await httpClient.get(
							`/linked-docs-types/${markLinkedDoc.linkedDoc.type.id}/docs`
						)
						cachedDocs.set(
							markLinkedDoc.linkedDoc.id,
							linkedDocsResponse.data
						)
						setOptionsObject({
							types: linkedDocTypesResponse.data,
							docs: linkedDocsResponse.data,
						})
					} else {
						setOptionsObject({
							...defaultOptionsObject,
							types: linkedDocTypesResponse.data,
						})
					}
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onTypeSelect = async (id: number) => {
		if (id == null) {
			selectedObject.linkedDoc = defaultSelectedDoc
			setOptionsObject({
				...defaultOptionsObject,
				types: optionsObject.types,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.types,
			selectedObject.linkedDoc.type
		)
		if (v != null) {
			if (cachedDocs.has(v.id)) {
				selectedObject.linkedDoc.type = v
				setOptionsObject({
					...optionsObject,
					docs: cachedDocs.get(v.id),
				})
			} else {
				try {
					const linkedDocsResponse = await httpClient.get(
						`/linked-docs-types/${id}/docs`
					)
					cachedDocs.set(v.id, linkedDocsResponse.data)
					selectedObject.linkedDoc.type = v
					setOptionsObject({
						...optionsObject,
						docs: linkedDocsResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

	const onDocSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				linkedDoc: {
					...defaultSelectedDoc,
					type: selectedObject.linkedDoc.type,
				},
			})
		}
		const v = getFromOptions(id, optionsObject.docs, selectedObject)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				linkedDoc: v,
			})
		}
	}

	const checkIfValid = () => {
		if (selectedObject.linkedDoc.type == null) {
			setErrMsg('Пожалуйста, выберите тип ссылочного документа')
			return false
		}
		if (selectedObject.linkedDoc.id === -1) {
			setErrMsg('Пожалуйста, выберите ссылочный документ')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(`/marks/${mark.id}/mark-linked-docs`, {
					linkedDocId: selectedObject.linkedDoc.id,
				})
				history.push('/linked-docs')
			} catch (e) {
				if (e.response.status === 409) {
					setErrMsg('Данный ссылочный документ уже добавлен к марке')
					return
				}
				console.log('Fail')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(
					`/mark-linked-docs/${selectedObject.id}`,
					{
						linkedDocId: selectedObject.linkedDoc.id,
					}
				)
				history.push('/linked-docs')
			} catch (e) {
				if (e.response.status === 409) {
					setErrMsg('Данный ссылочный документ уже добавлен к марке')
					return
				}
				console.log('Fail')
			}
		}
	}

	return mark == null && !isCreateMode ? null : (
		<div className="component-cnt">
			<h1 className="text-centered">
				{isCreateMode
					? 'Добавление ссылочного документа'
					: 'Данные ссылочного документа'}
			</h1>
			<div className="flex">
				<div className="info-area shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label>Обозначение</Form.Label>
						<Form.Control
							type="text"
							value={selectedObject.linkedDoc.designation}
							readOnly={true}
						/>
					</Form.Group>
					<Form.Group className="no-bot-mrg">
						<Form.Label>Наименование</Form.Label>
						<Form.Control
							as="textarea"
							rows={4}
							style={{ resize: 'none' }}
							value={selectedObject.linkedDoc.name}
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
						onChange={(selectedOption) =>
							onTypeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.linkedDoc.type == null
								? null
								: {
										value: selectedObject.linkedDoc.type.id,
										label:
											selectedObject.linkedDoc.type.name,
								  }
						}
						options={optionsObject.types.map((t) => {
							return {
								value: t.id,
								label: t.name,
							}
						})}
						styles={reactSelectstyle}
					/>

					<div className="bold mrg-top-2">Шифр</div>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите шифр ссылочного документа"
						noOptionsMessage={() =>
							'Ссылочные документы не найдены'
						}
						className="mrg-top"
						onChange={(selectedOption) =>
							onDocSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.id === -1
								? null
								: {
										value: selectedObject.id,
										label: selectedObject.linkedDoc.code,
								  }
						}
						options={optionsObject.docs.map((d) => {
							return {
								value: d.id,
								label: d.code,
							}
						})}
						styles={reactSelectstyle}
					/>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							isCreateMode
								? onCreateButtonClick
								: onChangeButtonClick
						}
					>
						{isCreateMode
							? 'Добавить ссылочный документ'
							: 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default LinkedDocData
