// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { X } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import AttachedDoc from '../../model/AttachedDoc'
import { useMark } from '../../store/MarkStore'

type IOtherAttachedDocDataProps = {
	otherAttachedDoc: AttachedDoc
	isCreateMode: boolean
}

type OtherAttachedDocDataProps = {
	otherAttachedDocData: IOtherAttachedDocDataProps
	setOtherAttachedDocData: (d: IOtherAttachedDocDataProps) => void
	otherAttachedDocs: AttachedDoc[]
	setOtherAttachedDocs: (a: AttachedDoc[]) => void
}

const OtherAttachedDocData = ({
	otherAttachedDocData,
	setOtherAttachedDocData,
	otherAttachedDocs,
	setOtherAttachedDocs,
}: OtherAttachedDocDataProps) => {
	const mark = useMark()

	const defaultSelectedObject = {
		id: -1,
		designation: '',
		name: 'Локальная смета',
		note: '',
	} as AttachedDoc

	const [selectedObject, setSelectedObject] = useState<AttachedDoc>(null)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (otherAttachedDocData.otherAttachedDoc == null) {
			const fetchData = async () => {
				try {
					const newDesignationResponse = await httpClient.get(
						`/marks/${mark.id}/attached-docs/new-designation`
					)
					setSelectedObject({
						...defaultSelectedObject,
						designation: mark.designation + '.' + newDesignationResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch designation')
				}
			}
			fetchData()
		} else {
			setSelectedObject({
				...defaultSelectedObject,
				...otherAttachedDocData.otherAttachedDoc,
			})
		}
	}, [otherAttachedDocData])

	const onDesignationChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			designation: event.currentTarget.value,
		})
	}

	const onNameChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onNoteChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	const checkIfValid = () => {
		if (selectedObject.designation === '') {
			setErrMsg('Пожалуйста, введите обозначение прилагаемого документа')
			return false
		}
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование прилагаемого документа')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(`/marks/${mark.id}/attached-docs`, {
					designation: selectedObject.designation,
					name: selectedObject.name,
					note: selectedObject.note,
				})
				const arr = [...otherAttachedDocs]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setOtherAttachedDocs(arr)
				setOtherAttachedDocData({
					otherAttachedDoc: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg(
						'Прилагаемый документ с таким обозначением уже существует'
					)
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
					designation:
						selectedObject.designation ===
						otherAttachedDocData.otherAttachedDoc.designation
							? undefined
							: selectedObject.designation,
					name:
						selectedObject.name === otherAttachedDocData.otherAttachedDoc.name
							? undefined
							: selectedObject.name,
					note:
						(selectedObject.note === otherAttachedDocData.otherAttachedDoc.note) || (selectedObject.note === '' && otherAttachedDocData.otherAttachedDoc.note == null)
							? undefined
							: selectedObject.note,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/attached-docs/${selectedObject.id}`,
					object
				)
				
				const arr = []
				for (const v of otherAttachedDocs) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setOtherAttachedDocs(arr)
				setOtherAttachedDocData({
					otherAttachedDoc: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg(
						'Прилагаемый документ с таким обозначением уже существует'
					)
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
		<div className="component-cnt flex-v-cent-h">
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div relative">
				<div className="pointer absolute"
					style={{top: 5, right: 8}}
					onClick={() => setOtherAttachedDocData({
						otherAttachedDoc: null,
						isCreateMode: false,
					})}
				>
					<X color="#666" size={33} />
				</div>
				<Form.Group>
					<Form.Label htmlFor="designation">Обозначение</Form.Label>
					<Form.Control
						id="designation"
						type="text"
						placeholder="Введите обозначение"
						autoComplete="off"
						value={selectedObject.designation}
						onChange={onDesignationChange}
					/>
				</Form.Group>

				<Form.Group>
					<Form.Label htmlFor="name">Наименование</Form.Label>
					<Form.Control
						id="name"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите наименование"
						value={selectedObject.name}
						onChange={onNameChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2" style={{ marginBottom: 0 }}>
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
						otherAttachedDocData.isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
					disabled={processIsRunning || (!otherAttachedDocData.isCreateMode && !Object.values({
						designation:
							selectedObject.designation ===
							otherAttachedDocData.otherAttachedDoc.designation
								? undefined
								: selectedObject.designation,
						name:
							selectedObject.name === otherAttachedDocData.otherAttachedDoc.name
								? undefined
								: selectedObject.name,
						note:
							(selectedObject.note === otherAttachedDocData.otherAttachedDoc.note) || (selectedObject.note === '' && otherAttachedDocData.otherAttachedDoc.note == null)
								? undefined
								: selectedObject.note,
					}).some((x) => x !== undefined))}
				>
					{otherAttachedDocData.isCreateMode
						? 'Добавить прилагаемый документ'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default OtherAttachedDocData
