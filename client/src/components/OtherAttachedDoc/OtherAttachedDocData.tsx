// Global
import React, { useState, useEffect } from 'react'
import { useHistory, Link } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import AttachedDoc from '../../model/AttachedDoc'
import { useMark } from '../../store/MarkStore'
import { makeMarkName } from '../../util/make-name'

type OtherAttachedDocDataProps = {
	otherAttachedDoc: AttachedDoc
	isCreateMode: boolean
}

const OtherAttachedDocData = ({
	otherAttachedDoc,
	isCreateMode,
}: OtherAttachedDocDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<AttachedDoc>(
		isCreateMode
			? {
					id: -1,
					designation:
						mark.designation + '.ЛС',
					name: 'Локальная смета',
					note: '',
			  }
			: otherAttachedDoc
	)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null) {
				history.push('/other-attached-docs')
				return
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
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				await httpClient.post(`/marks/${mark.id}/attached-docs`, {
					designation: selectedObject.designation,
					name: selectedObject.name,
					note: selectedObject.note,
				})
				history.push('/other-attached-docs')
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
				history.push('/other-attached-docs')
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
			<div className="hanging-routes">
				<Link to="/other-attached-docs">
					Прилагаемые документы
				</Link>
			</div>
			<h1 className="text-centered">
				{isCreateMode
					? 'Создание прочего прилагаемого документа'
					: 'Данные прочего прилагаемого документа'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group>
					<Form.Label htmlFor="designation">Обозначение</Form.Label>
					<Form.Control
						id="designation"
						type="text"
						placeholder="Введите обозначение"
						autoComplete="off"
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
					disabled={processIsRunning}
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
