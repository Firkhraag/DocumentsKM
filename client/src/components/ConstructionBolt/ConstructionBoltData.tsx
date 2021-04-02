// Global
import React, { useState, useEffect } from 'react'
import { useHistory, Link } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import ConstructionBolt from '../../model/ConstructionBolt'
import BoltDiameter from '../../model/BoltDiameter'
import { useMark } from '../../store/MarkStore'
import { useSetScroll } from '../../store/ScrollStore'
import getFromOptions from '../../util/get-from-options'
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
	const setScroll = useSetScroll()

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

	const [processIsRunning, setProcessIsRunning] = useState(false)
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
		setProcessIsRunning(true)
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
				setScroll(4)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Болт уже существует')
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
						selectedObject.washerNum === constructionBolt.washerNum
							? undefined
							: selectedObject.washerNum,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/construction-bolts/${selectedObject.id}`,
					object
				)
				setScroll(4)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Болт уже существует')
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
				<Link to="/specifications">Выпуски спецификаций</Link>
				<Link onClick={() => setScroll(1)} to={`/specifications/${specificationId}`}>Виды конструкций</Link>
				<Link onClick={() => setScroll(4)} to={`/specifications/${specificationId}/constructions/${constructionId}`}>Высокопрочные болты</Link>
			</div>
			<h1 className="text-centered">
				{isCreateMode
					? 'Создание высокопрочного болта'
					: 'Данные высокопрочного болта'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group className="space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="diameter"
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
						className="bolt-input-width"
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

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="packet"
					>
						Толщина пакета, мм
					</Form.Label>
					<Form.Control
						id="packet"
						type="text"
						placeholder="Введите толщину пакета"
						className="bolt-input-width"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.packet)
								? ''
								: selectedObject.packet
						}
						onBlur={onPacketChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="num"
					>
						Число болтов, штук
					</Form.Label>
					<Form.Control
						id="num"
						type="text"
						placeholder="Введите число болтов"
						className="bolt-input-width"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onBlur={onNumChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="nutNum"
					>
						Число гаек на болт, штук
					</Form.Label>
					<Form.Control
						id="nutNum"
						type="text"
						placeholder="Введите число гаек на болт"
						className="bolt-input-width"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.nutNum)
								? ''
								: selectedObject.nutNum
						}
						onBlur={onNutNumChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="washerNum"
					>
						Число шайб на болт, штук
					</Form.Label>
					<Form.Control
						id="washerNum"
						type="text"
						placeholder="Введите число шайб на болт"
						className="bolt-input-width"
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
					disabled={processIsRunning}
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
