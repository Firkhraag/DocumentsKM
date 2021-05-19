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
import ConstructionBolt from '../../model/ConstructionBolt'
import BoltDiameter from '../../model/BoltDiameter'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'

type IConstructionBoltDataProps = {
	constructionBolt: ConstructionBolt
	isCreateMode: boolean
	index: number
}

type ConstructionBoltDataProps = {
	constructionBoltData: IConstructionBoltDataProps
	setConstructionBoltData: (d: IConstructionBoltDataProps) => void
	constructionBolts: ConstructionBolt[]
	setConstructionBolts: (a: ConstructionBolt[]) => void
	constructionId: number
}

const ConstructionBoltData = ({
	constructionBoltData,
	setConstructionBoltData,
	constructionBolts,
	setConstructionBolts,
	constructionId
}: ConstructionBoltDataProps) => {
	const mark = useMark()

	const defaultSelectedObject = {
		id: -1,
		diameter: null,
		packet: NaN,
		num: NaN,
		nutNum: 1,
		washerNum: 2,
  	} as ConstructionBolt

	const [selectedObject, setSelectedObject] = useState<ConstructionBolt>(null)
	const [optionsObject, setOptionsObject] = useState([] as BoltDiameter[])

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const [fetched, setFetched] = useState(false)

	useEffect(() => {
		if (!fetched) {
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
			setFetched(true)
		}
		if (constructionBoltData.constructionBolt != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...constructionBoltData.constructionBolt,
			})
		} else {
			setSelectedObject({
				...defaultSelectedObject,
			})
		}
	}, [constructionBoltData])

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

	const onPacketChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			packet: parseInt(event.currentTarget.value),
		})
	}

	const onNumChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			num: parseInt(event.currentTarget.value),
		})
	}

	const onNutNumChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			nutNum: parseInt(event.currentTarget.value),
		})
	}

	const onWasherNumChange = (event: React.ChangeEvent<HTMLInputElement>) => {
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
				const idResponse = await httpClient.post(
					`/constructions/${constructionId}/bolts`,
					{
						diameterId: selectedObject.diameter.id,
						packet: selectedObject.packet,
						num: selectedObject.num,
						nutNum: selectedObject.nutNum,
						washerNum: selectedObject.washerNum,
					}
				)
				const arr = [...constructionBolts]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setConstructionBolts(arr)
				setConstructionBoltData({
					constructionBolt: null,
					isCreateMode: false,
					index: -1,
				})
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
						constructionBoltData.constructionBolt.diameter.id
							? undefined
							: selectedObject.diameter.id,
					packet:
						selectedObject.packet === constructionBoltData.constructionBolt.packet
							? undefined
							: selectedObject.packet,
					num:
						selectedObject.num === constructionBoltData.constructionBolt.num
							? undefined
							: selectedObject.num,
					nutNum:
						selectedObject.nutNum === constructionBoltData.constructionBolt.nutNum
							? undefined
							: selectedObject.nutNum,
					washerNum:
						selectedObject.washerNum === constructionBoltData.constructionBolt.washerNum
							? undefined
							: selectedObject.washerNum,
				}
				await httpClient.patch(
					`/construction-bolts/${selectedObject.id}`,
					object
				)
				const arr = []
				for (const v of constructionBolts) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setConstructionBolts(arr)
				setConstructionBoltData({
					constructionBolt: null,
					isCreateMode: false,
					index: -1,
				})
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
			<div className="shadow custom-p-3 mb-5 bg-white rounded component-width component-cnt-div relative">
				<div className="pointer absolute"
					style={{top: 5, right: 8}}
					onClick={() => setConstructionBoltData({
						constructionBolt: null,
						isCreateMode: false,
						index: -1,
					})}
				>
					<X color="#666" size={33} />
				</div>
				{constructionBoltData.isCreateMode ? null :
					<div className="absolute bold" style={{top: -25, left: 0, color: "#666"}}>
						{constructionBoltData.index}
					</div>
				}
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
						value={
							isNaN(selectedObject.packet)
								? ''
								: selectedObject.packet
						}
						onChange={onPacketChange}
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
						value={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onChange={onNumChange}
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
						value={
							isNaN(selectedObject.nutNum)
								? ''
								: selectedObject.nutNum
						}
						onChange={onNutNumChange}
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
						value={
							isNaN(selectedObject.washerNum)
								? ''
								: selectedObject.washerNum
						}
						onChange={onWasherNumChange}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						constructionBoltData.isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
					disabled={processIsRunning || (!constructionBoltData.isCreateMode && !Object.values({
						diameterId:
							selectedObject.diameter == null ||selectedObject.diameter.id ===
							constructionBoltData.constructionBolt.diameter.id
								? undefined
								: selectedObject.diameter.id,
						packet:
							selectedObject.packet === constructionBoltData.constructionBolt.packet
								? undefined
								: selectedObject.packet,
						num:
							selectedObject.num === constructionBoltData.constructionBolt.num
								? undefined
								: selectedObject.num,
						nutNum:
							selectedObject.nutNum === constructionBoltData.constructionBolt.nutNum
								? undefined
								: selectedObject.nutNum,
						washerNum:
							selectedObject.washerNum === constructionBoltData.constructionBolt.washerNum
								? undefined
								: selectedObject.washerNum,
					}).some((x) => x !== undefined))}
				>
					{constructionBoltData.isCreateMode
						? 'Добавить высокопрочный болт'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default ConstructionBoltData
