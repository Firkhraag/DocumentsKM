// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { X } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import MarkLinkedDoc from '../../model/MarkLinkedDoc'
import LinkedDoc from '../../model/LinkedDoc'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'
import LinkedDocType from '../../model/LinkedDocType'

type ILinkedDocDataProps = {
	markLinkedDoc: MarkLinkedDoc
	isCreateMode: boolean
	index: number
}

type LinkedDocDataProps = {
	linkedDocData: ILinkedDocDataProps
	setLinkedDocData: (d: ILinkedDocDataProps) => void
	linkedDocs: MarkLinkedDoc[]
	setLinkedDocs: (a: MarkLinkedDoc[]) => void
}

const LinkedDocData = ({
	linkedDocData,
	setLinkedDocData,
	linkedDocs,
	setLinkedDocs,
}: LinkedDocDataProps) => {

	const defaultOptionsObject = {
		types: [] as LinkedDocType[],
		docs: [] as LinkedDoc[],
	}
	const defaultLinkedDoc = {
		id: -1,
		code: '',
		name: '',
		designation: '',
		type: null,
	} as LinkedDoc

	const defaultSelectedObject = {
		id: -1,
		linkedDoc: defaultLinkedDoc,
		note: '',
	} as MarkLinkedDoc

	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<MarkLinkedDoc>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)
	const cachedDocs = useState(new Map<number, LinkedDoc[]>())[0]

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		const fetchData = async () => {
			try {
				const linkedDocTypesResponse = await httpClient.get(
					`/linked-doc-types`
				)
				if (!linkedDocData.isCreateMode) {
					const linkedDocsResponse = await httpClient.get(
						`/linked-docs-types/${linkedDocData.markLinkedDoc.linkedDoc.type.id}/docs`
					)
					cachedDocs.set(
						linkedDocData.markLinkedDoc.linkedDoc.id,
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
				console.log('Failed to fetch the data', e)
			}
		}
		fetchData()
		if (linkedDocData.markLinkedDoc != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...linkedDocData.markLinkedDoc,
			})
		} else {
			setSelectedObject({
				...defaultSelectedObject,
			})
		}
	}, [linkedDocData])

	const onTypeSelect = async (id: number) => {
		if (id == null) {
			selectedObject.linkedDoc = defaultLinkedDoc
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
				setSelectedObject({
					...selectedObject,
					linkedDoc: {
						...defaultLinkedDoc,
						type: v,
					},
				})
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
					setSelectedObject({
						...selectedObject,
						linkedDoc: {
							...defaultLinkedDoc,
							type: v,
						},
					})
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
					...defaultLinkedDoc,
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

	const onNoteChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
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
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(`/marks/${mark.id}/mark-linked-docs`, {
					linkedDocId: selectedObject.linkedDoc.id,
					note: selectedObject.note,
				})
				const arr = [...linkedDocs]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setLinkedDocs(arr)
				setLinkedDocData({
					markLinkedDoc: null,
					isCreateMode: false,
					index: -1,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Данный ссылочный документ уже добавлен к марке')
				} else {
					setErrMsg('Произошла ошибка')
				}
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	const onChangeButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const object = {
					linkedDocId:
						selectedObject.linkedDoc.id ===
						linkedDocData.markLinkedDoc.linkedDoc.id
							? undefined
							: selectedObject.linkedDoc.id,
					note:
						(selectedObject.note === linkedDocData.markLinkedDoc.note) || (selectedObject.note === '' && linkedDocData.markLinkedDoc.note == null)
							? undefined
							: selectedObject.note,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/mark-linked-docs/${selectedObject.id}`,
					object
				)

				const arr = []
				for (const v of linkedDocs) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setLinkedDocs(arr)
				setLinkedDocData({
					markLinkedDoc: null,
					isCreateMode: false,
					index: -1,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Данный ссылочный документ уже добавлен к марке')
				} else {
					setErrMsg('Произошла ошибка')
				}
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt">
			<div className="flex">
				<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div relative">
					<div className="pointer absolute"
						style={{top: 5, right: 8}}
						onClick={() => setLinkedDocData({
							markLinkedDoc: null,
							isCreateMode: false,
							index: -1,
						})}
					>
						<X color="#666" size={33} />
					</div>
					{linkedDocData.isCreateMode ? null :
						<div className="absolute bold" style={{top: -25, left: 0, color: "#666"}}>
							{linkedDocData.index}
						</div>
					}
					<Form.Group>
						<Form.Label className="no-bot-mrg" htmlFor="type">
							Вид
						</Form.Label>
						<Select
							inputId="type"
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
											value:
												selectedObject.linkedDoc.type
													.id,
											label:
												selectedObject.linkedDoc.type
													.name,
									  }
							}
							options={optionsObject.types.map((t) => {
								return {
									value: t.id,
									label: t.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="mrg-top-2 no-bot-mrg">
						<Form.Label className="no-bot-mrg" htmlFor="code">
							Шифр
						</Form.Label>
						<Select
							inputId="code"
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
								selectedObject.linkedDoc.id === -1
									? null
									: {
											value: selectedObject.id,
											label:
												selectedObject.linkedDoc.code,
									  }
							}
							options={optionsObject.docs.map((d) => {
								return {
									value: d.id,
									label: d.code + " " + d.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group
						className="mrg-top-2"
						style={{ marginBottom: 0 }}
					>
						<Form.Label htmlFor="note">Примечание</Form.Label>
						<Form.Control
							id="note"
							as="textarea"
							rows={4}
							style={{ resize: 'none' }}
							placeholder="Не введено"
							value={selectedObject.note}
							onChange={onNoteChange}
						/>
					</Form.Group>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							linkedDocData.isCreateMode
								? onCreateButtonClick
								: onChangeButtonClick
						}
						disabled={processIsRunning || (!linkedDocData.isCreateMode && !Object.values({
							linkedDocId:
								selectedObject.linkedDoc.code == '' || selectedObject.linkedDoc.id ===
								linkedDocData.markLinkedDoc.linkedDoc.id
									? undefined
									: selectedObject.linkedDoc.id,
							note:
								(selectedObject.note === linkedDocData.markLinkedDoc.note) || (selectedObject.note === '' && linkedDocData.markLinkedDoc.note == null)
									? undefined
									: selectedObject.note,
						}).some((x) => x !== undefined))}
					>
						{linkedDocData.isCreateMode
							? 'Добавить ссылочный документ'
							: 'Сохранить изменения'}
					</Button>
				</div>

				<div className="info-area shadow p-3 mb-5 bg-white rounded component-width component-cnt-div mrg-left">
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
			</div>
		</div>
	)
}

export default LinkedDocData
