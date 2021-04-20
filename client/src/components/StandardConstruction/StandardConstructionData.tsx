// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { X } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import StandardConstruction from '../../model/StandardConstruction'
import StandardConstructionName from '../../model/StandardConstructionName'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import { reactSelectStyle } from '../../util/react-select-style'

type IStandardConstructionDataProps = {
	standardConstruction: StandardConstruction
	isCreateMode: boolean
}

type StandardConstructionDataProps = {
	standardConstructionData: IStandardConstructionDataProps
	setStandardConstructionData: (d: IStandardConstructionDataProps) => void
	standardConstructions: StandardConstruction[]
	setStandardConstructions: (a: StandardConstruction[]) => void
	specificationId: number
}

const StandardConstructionData = ({
	standardConstructionData,
	setStandardConstructionData,
	standardConstructions,
	setStandardConstructions,
	specificationId
}: StandardConstructionDataProps) => {
	const mark = useMark()

	const defaultSelectedObject = {
		id: -1,
		name: '',
		num: NaN,
		sheet: '',
		weight: NaN,
  	} as StandardConstruction

	const [selectedObject, setSelectedObject] = useState<StandardConstruction>(null)
	const [nameOptions, setNameOptions] = useState(
		[] as StandardConstructionName[]
	)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const [fetched, setFetched] = useState(false)

	useEffect(() => {
		if (!fetched) {
			const fetchData = async () => {
				try {
					const namesResponse = await httpClient.get(
						`/standard-construction-names`
					)
					setNameOptions(namesResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
			setFetched(true)
		}
		if (standardConstructionData.standardConstruction != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...standardConstructionData.standardConstruction,
			})
		} else {
			setSelectedObject({
				...defaultSelectedObject,
			})
		}
	}, [standardConstructionData])

	const onNameChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onNameSelect = async (id: number) => {
		let v = null
		for (let el of nameOptions) {
			if (el.id === id) {
				v = el
				break
			}
		}
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				name: v.name,
			})
		}
	}

	const onNumChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			num: parseInt(event.currentTarget.value),
		})
	}

	const onSheetChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			sheet: event.currentTarget.value,
		})
	}

	const onWeightChange = (event: React.ChangeEvent<HTMLInputElement>) => {
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
		if (isNaN(selectedObject.num)) {
			setErrMsg('Пожалуйста, введите количество элементов')
			return false
		}
		if (isNaN(selectedObject.weight)) {
			setErrMsg('Пожалуйста, введите общий вес типовой конструкции')
			return false
		}
		if (selectedObject.num < 0 || selectedObject.num > 1000000) {
			setErrMsg('Пожалуйста, введите правильное количество элементов')
			return false
		}
		if (selectedObject.weight < 0 || selectedObject.weight > 1000000) {
			setErrMsg('Пожалуйста, введите правильный общий вес')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(
					`/specifications/${specificationId}/standard-constructions`,
					{
						name: selectedObject.name,
						num: selectedObject.num,
						sheet: selectedObject.sheet,
						weight: selectedObject.weight,
					}
				)
				const arr = [...standardConstructions]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setStandardConstructions(arr)
				setStandardConstructionData({
					standardConstruction: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Типовая конструкция уже существует')
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
					name:
						selectedObject.name === standardConstructionData.standardConstruction.name
							? undefined
							: selectedObject.name,
					num:
						selectedObject.num === standardConstructionData.standardConstruction.num
							? undefined
							: selectedObject.num,
					sheet:
						selectedObject.sheet === standardConstructionData.standardConstruction.sheet
							? undefined
							: selectedObject.sheet,
					weight:
						selectedObject.weight === standardConstructionData.standardConstruction.weight
							? undefined
							: selectedObject.weight,
				}
				await httpClient.patch(
					`/standard-constructions/${selectedObject.id}`,
					object
				)
				const arr = []
				for (const v of standardConstructions) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setStandardConstructions(arr)
				setStandardConstructionData({
					standardConstruction: null,
					isCreateMode: false,
				})
			} catch (e) {
				if (e.response != null && e.response.status === 409) {
					setErrMsg('Типовая конструкция уже существует')
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
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div relative">
				<div className="pointer absolute"
					style={{top: 5, right: 8}}
					onClick={() => setStandardConstructionData({
						standardConstruction: null,
						isCreateMode: false,
					})}
				>
					<X color="#666" size={33} />
				</div>

				<Form.Group>
					<Form.Label htmlFor="name">Название</Form.Label>
					<Form.Control
						id="name"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите наименование"
						autoComplete="off"
						value={selectedObject.name}
						onChange={onNameChange}
					/>
				</Form.Group>

				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Типовые наименования"
					noOptionsMessage={() => 'Наименования не найдены'}
					onChange={(selectedOption) =>
						onNameSelect((selectedOption as any)?.value)
					}
					value={null}
					options={nameOptions.map((s) => {
						return {
							value: s.id,
							label: s.name,
						}
					})}
					styles={reactSelectStyle}
				/>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="num"
					>
						Количество элементов, шт.
					</Form.Label>
					<Form.Control
						id="num"
						type="text"
						placeholder="Введите количество элементов"
						autoComplete="off"
						className="standard-construction-input-width"
						value={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onChange={onNumChange}
					/>
				</Form.Group>

				<Form.Group className="space-between-cent-v mrg-top-2">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="sheet"
					>
						№ чертежа проекта с типовой конструкцией
					</Form.Label>
					<Form.Control
						id="sheet"
						type="text"
						placeholder="Не введено"
						autoComplete="off"
						className="standard-construction-input-width"
						value={selectedObject.sheet}
						onChange={onSheetChange}
					/>
				</Form.Group>

				<Form.Group className="space-between-cent-v mrg-top-2">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="weight"
					>
						Общий вес типовой конструкции, т
					</Form.Label>
					<Form.Control
						id="weight"
						type="text"
						placeholder="Введите вес"
						autoComplete="off"
						className="standard-construction-input-width"
						value={
							isNaN(selectedObject.weight)
								? ''
								: selectedObject.weight
						}
						onChange={onWeightChange}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						standardConstructionData.isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
					disabled={processIsRunning || (!standardConstructionData.isCreateMode && !Object.values({
						name:
							selectedObject.name === standardConstructionData.standardConstruction.name
								? undefined
								: selectedObject.name,
						num:
							selectedObject.num === standardConstructionData.standardConstruction.num
								? undefined
								: selectedObject.num,
						sheet:
							selectedObject.sheet === standardConstructionData.standardConstruction.sheet
								? undefined
								: selectedObject.sheet,
						weight:
							selectedObject.weight === standardConstructionData.standardConstruction.weight
								? undefined
								: selectedObject.weight,
					}).some((x) => x !== undefined))}
				>
					{standardConstructionData.isCreateMode
						? 'Добавить типовую конструкцию'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default StandardConstructionData
