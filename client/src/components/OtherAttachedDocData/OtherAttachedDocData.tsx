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
import AttachedDoc from '../../model/AttachedDoc'
import { useMark } from '../../store/MarkStore'
import { makeMarkName } from '../../util/make-name'

type OtherAttachedDocProps = {
	otherAttachedDoc: AttachedDoc
	isCreateMode: boolean
}

const OtherAttachedDocData = ({
	otherAttachedDoc,
	isCreateMode,
}: OtherAttachedDocProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<AttachedDoc>(
		otherAttachedDoc
	)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (!isCreateMode && otherAttachedDoc.id === -1) {
				history.push('/other-attached-docs')
				return
			}
			if (isCreateMode) {
				setSelectedObject({
					id: -1,
					designation:
						makeMarkName(
							mark.subnode.node.project.baseSeries,
							mark.subnode.node.code,
							mark.subnode.code,
							mark.code
						) + '.ЛС',
					name: 'Локальная смета',
					note: '',
				})
			}
		}
	}, [mark])

	const onDesignationChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			designation: event.currentTarget.value,
		})
	}

	const onNameChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onNoteChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
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
		if (checkIfValid()) {
			try {
				await httpClient.post(`/marks/${mark.id}/attached-docs`, {
					designation: selectedObject.designation,
					name: selectedObject.name,
					note: selectedObject.note,
				})
				history.push('/other-attached-docs')
			} catch (e) {
				if (e.response.status === 409) {
					setErrMsg('Прилагаемый документ с таким обозначением уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(`/attached-docs/${selectedObject.id}`, {
					designation:
						selectedObject.designation ===
						otherAttachedDoc.designation
							? undefined
							: selectedObject.designation,
					name:
						selectedObject.name === otherAttachedDoc.name
							? undefined
							: selectedObject.name,
					note:
						selectedObject.note === otherAttachedDoc.note
							? undefined
							: selectedObject.note,
				})
				history.push('/other-attached-docs')
			} catch (e) {
                if (e.response.status === 409) {
					setErrMsg('Прилагаемый документ с таким обозначением уже существует')
					return
				}
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	return selectedObject == null || (mark == null && !isCreateMode) ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
				{isCreateMode
					? 'Добавление прилагаемого документа'
					: 'Данные прилагаемого документа'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group>
					<Form.Label htmlFor="designation">Обозначение</Form.Label>
					<Form.Control
						id="designation"
						type="text"
						placeholder="Введите обозначение"
						defaultValue={selectedObject.designation}
						onBlur={onDesignationChange}
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
						defaultValue={selectedObject.name}
						onBlur={onNameChange}
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
						defaultValue={selectedObject.note}
						onBlur={onNoteChange}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
				>
					{isCreateMode
						? 'Добавить прилагаемый документ'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default OtherAttachedDocData
