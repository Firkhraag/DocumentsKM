// Global
import React, { useState, useEffect } from 'react'
import { useHistory, Link } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Specification from '../../model/Specification'
import Construction from '../../model/Construction'
import StandardConstruction from '../../model/StandardConstruction'
import { useMark } from '../../store/MarkStore'
import ConstructionTable from '../Construction/ConstructionTable'
import StandardConstructionTable from '../StandardConstruction/StandardConstructionTable'
import ErrorMsg from '../ErrorMsg/ErrorMsg'

type SpecificationDataProps = {
	specification: Specification
	setConstruction: (c: Construction) => void
	copiedConstruction: Construction
	setCopiedConstruction: (c: Construction) => void
	setStandardConstruction: (sc: StandardConstruction) => void
}

const SpecificationData = ({
	specification,
	setConstruction,
	copiedConstruction,
	setCopiedConstruction,
	setStandardConstruction,
}: SpecificationDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState(specification)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null && selectedObject == null) {
			history.push('/specifications')
		}
	}, [mark])

	const onNoteChange = async (
		event: React.FormEvent<HTMLTextAreaElement>
	) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	const onChangeButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const object = {
				note:
					selectedObject.note === specification.note
						? undefined
						: selectedObject.note,
			}
			if (!Object.values(object).some((x) => x !== undefined)) {
				setErrMsg('Изменения осутствуют')
				setProcessIsRunning(false)
				return
			}
			await httpClient.patch(
				`/specifications/${selectedObject.id}`,
				object
			)
			history.push('/specifications')
		} catch (e) {
			setErrMsg('Произошла ошибка')
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<div className="hanging-routes">
				<Link to="/specifications">Выпуски спецификаций</Link>
			</div>
			<h1 className="text-centered">Данные выпуска спецификации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<div className="space-between">
					<Form.Group className="flex-cent-v">
						<Form.Label
							className="no-bot-mrg"
							style={{ marginRight: '.75em' }}
						>
							Номер
						</Form.Label>
						<Form.Control
							type="text"
							value={selectedObject.num}
							readOnly={true}
							className="text-centered"
							style={{ width: '50px' }}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Check
							name="currentRelease"
							type="radio"
							style={{ pointerEvents: 'none' }}
							checked={selectedObject.isCurrent}
							readOnly={true}
						/>
					</Form.Group>
				</div>
				<Form.Group className="no-bot-mrg">
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
					onClick={onChangeButtonClick}
					disabled={processIsRunning}
				>
					Сохранить изменения
				</Button>
			</div>

			<ConstructionTable
				setConstruction={setConstruction}
				copiedConstruction={copiedConstruction}
				setCopiedConstruction={setCopiedConstruction}
				specificationId={selectedObject.id}
			/>
			<StandardConstructionTable
				setStandardConstruction={setStandardConstruction}
				specificationId={selectedObject.id}
			/>
		</div>
	)
}

export default SpecificationData
