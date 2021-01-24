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
import ConstructionBolt from '../../model/ConstructionBolt'
import BoltDiameter from '../../model/BoltDiameter'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

type ConstructionBoltDataProps = {
	constructionBolt: ConstructionBolt
	isCreateMode: boolean
	specificationId: number
	constructionId: number
}

const ConstructionBoltData = ({
	constructionBolt,
	isCreateMode,
	specificationId,
	constructionId,
}: ConstructionBoltDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<ConstructionBolt>(
		isCreateMode
			? {
					id: -1,
					diameter: null,
					packet: NaN,
					num: NaN,
					nutNum: NaN,
					washerNum: NaN,
			  }
			: constructionBolt
	)
	const [optionsObject, setOptionsObject] = useState([] as BoltDiameter[])
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null || specificationId == -1) {
				history.push('/specifications')
				return
			}
			const fetchData = async () => {
				try {
					const boltDiametersResponse = await httpClient.get(
						`/bolt-diameters`
					)
					setOptionsObject(boltDiametersResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onDiameterSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				diameter: null,
			})
		}
		const v = getFromOptions(id, optionsObject, selectedObject.diameter)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				diameter: v,
			})
		}
	}

	const onPacketChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			packet: parseInt(event.currentTarget.value),
		})
	}

	const onNumChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			num: parseInt(event.currentTarget.value),
		})
	}

	const onNutNumChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			nutNum: parseInt(event.currentTarget.value),
		})
	}

	const onWasherNumChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			washerNum: parseInt(event.currentTarget.value),
		})
	}

	const checkIfValid = () => {
		if (isNaN(selectedObject.packet)) {
			setErrMsg('Пожалуйста, введите толщину пакета')
			return false
		}
		if (isNaN(selectedObject.num)) {
			setErrMsg('Пожалуйста, введите число болтов')
			return false
		}
		if (isNaN(selectedObject.nutNum)) {
			setErrMsg('Пожалуйста, введите число гаек на болт')
			return false
		}
		if (isNaN(selectedObject.washerNum)) {
			setErrMsg('Пожалуйста, введите число шайб на болт')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(
					`/constructions/${constructionId}/bolts`,
					{
						diameterId: selectedObject.diameter.id,
						packet: selectedObject.packet,
						num: selectedObject.num,
						nutNum: selectedObject.nutNum,
						washerNum: selectedObject.washerNum,
					}
				)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(
					`/construction-bolts/${selectedObject.id}`,
					{
						diameterId:
							selectedObject.diameter.id ===
							constructionBolt.diameter.id
								? undefined
								: selectedObject.diameter.id,
						packet:
							selectedObject.packet === constructionBolt.packet
								? undefined
								: selectedObject.packet,
						num:
							selectedObject.num === constructionBolt.num
								? undefined
								: selectedObject.num,
						nutNum:
							selectedObject.nutNum === constructionBolt.nutNum
								? undefined
								: selectedObject.nutNum,
						washerNum:
							selectedObject.washerNum ===
							constructionBolt.washerNum
								? undefined
								: selectedObject.washerNum,
					}
				)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
				{isCreateMode
					? 'Добавление высокопрочного болта'
					: 'Данные высокопрочного болта'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="diameter"
						style={{ marginRight: '4.3em' }}
					>
						Диаметр болта, мм
					</Form.Label>
					<Select
						inputId="diameter"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите диаметр болта"
						noOptionsMessage={() => 'Диаметры болтов не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onDiameterSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.diameter == null
								? null
								: {
										value: selectedObject.diameter.id,
										label: selectedObject.diameter.diameter,
								  }
						}
						options={optionsObject.map((d) => {
							return {
								value: d.id,
								label: d.diameter,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="packet"
						style={{ marginRight: '3.85em' }}
					>
						Толщина пакета, мм
					</Form.Label>
					<Form.Control
						id="packet"
						type="text"
						placeholder="Введите толщину пакета"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.packet)
								? ''
								: selectedObject.packet
						}
						onBlur={onPacketChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="num"
						style={{ marginRight: '4.05em' }}
					>
						Число болтов, штук
					</Form.Label>
					<Form.Control
						id="num"
						type="text"
						placeholder="Введите число болтов"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onBlur={onNumChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="nutNum"
						style={{ marginRight: '1.6em' }}
					>
						Число гаек на болт, штук
					</Form.Label>
					<Form.Control
						id="nutNum"
						type="text"
						placeholder="Введите число гаек на болт"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.nutNum)
								? ''
								: selectedObject.nutNum
						}
						onBlur={onNutNumChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="washerNum"
						style={{ marginRight: '1em' }}
					>
						Число шайб на болт, штук
					</Form.Label>
					<Form.Control
						id="washerNum"
						type="text"
						placeholder="Введите число шайб на болт"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.washerNum)
								? ''
								: selectedObject.washerNum
						}
						onBlur={onWasherNumChange}
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
						? 'Добавить высокопрочный болт'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default ConstructionBoltData
