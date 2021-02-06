// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import StandardConstruction from '../../model/StandardConstruction'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'

type StandardConstructionDataProps = {
	standardConstruction: StandardConstruction
	isCreateMode: boolean
	specificationId: number
}

const standardConstructionData = ({
	standardConstruction,
	isCreateMode,
	specificationId,
}: StandardConstructionDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<StandardConstruction>(
		isCreateMode
			? {
					id: -1,
					name: '',
					num: NaN,
					sheet: '',
					weight: NaN,
			  }
			: standardConstruction
	)

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null || specificationId == -1) {
				history.push('/specifications')
				return
			}
		}
	}, [mark])

	const onNameChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onNumChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			num: parseInt(event.currentTarget.value),
		})
	}

	const onSheetChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			sheet: event.currentTarget.value,
		})
	}

	const onWeightChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			weight: parseFloat(event.currentTarget.value),
		})
	}

	const checkIfValid = () => {
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование типовой конструкции')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(
					`/specifications/${specificationId}/standard-constructions`,
					{
						name: selectedObject.name,
						num: selectedObject.num,
						sheet: selectedObject.sheet,
						weight: selectedObject.weight,
					}
				)
				history.push(`/specifications/${specificationId}`)
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error', e)
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(
					`/standard-constructions/${selectedObject.id}`,
					{
						name:
							selectedObject.name === standardConstruction.name
								? undefined
								: selectedObject.name,
						num:
							selectedObject.num === standardConstruction.num
								? undefined
								: selectedObject.num,
						sheet:
							selectedObject.sheet === standardConstruction.sheet
								? undefined
								: selectedObject.sheet,
						weight:
							selectedObject.weight ===
							standardConstruction.weight
								? undefined
								: selectedObject.weight,
					}
				)
				history.push(`/specifications/${specificationId}`)
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error', e)
			}
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
				{isCreateMode
					? 'Создание типовой конструкции'
					: 'Данные по типовой конструкции'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<Form.Group>
					<Form.Label htmlFor="name">Название</Form.Label>
					<Form.Control
						id="name"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите наименование"
						autoComplete="off"
						defaultValue={selectedObject.name}
						onBlur={onNameChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="num"
						style={{ marginRight: '9.55em' }}
					>
						Количество элементов, шт.
					</Form.Label>
					<Form.Control
						id="num"
						type="text"
						placeholder="Не введено"
						autoComplete="off"
						className="auto-width flex-grow"
						defaultValue={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onBlur={onNumChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="sheet"
						style={{ marginRight: '1em' }}
					>
						№ чертежа проекта с типовой конструкцией
					</Form.Label>
					<Form.Control
						id="sheet"
						type="text"
						placeholder="Не введено"
						autoComplete="off"
						className="auto-width flex-grow"
						defaultValue={selectedObject.sheet}
						onBlur={onSheetChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="weight"
						style={{ marginRight: '5.55em' }}
					>
						Общий вес типовой конструкции, т
					</Form.Label>
					<Form.Control
						id="weight"
						type="text"
						placeholder="Не введено"
						autoComplete="off"
						className="auto-width flex-grow"
						defaultValue={
							isNaN(selectedObject.weight)
								? ''
								: selectedObject.weight
						}
						onBlur={onWeightChange}
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
						? 'Добавить типовую конструкцию'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default standardConstructionData
