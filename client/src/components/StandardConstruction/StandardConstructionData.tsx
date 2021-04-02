// Global
import React, { useState, useEffect } from 'react'
import { useHistory, Link } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import StandardConstruction from '../../model/StandardConstruction'
import StandardConstructionName from '../../model/StandardConstructionName'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import { useSetScroll } from '../../store/ScrollStore'
import { reactSelectStyle } from '../../util/react-select-style'

type StandardConstructionDataProps = {
	standardConstruction: StandardConstruction
	isCreateMode: boolean
	specificationId: number
}

const StandardConstructionData = ({
	standardConstruction,
	isCreateMode,
	specificationId,
}: StandardConstructionDataProps) => {
	const history = useHistory()
	const mark = useMark()
    const setScroll = useSetScroll()

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
	const [nameOptions, setNameOptions] = useState(
		[] as StandardConstructionName[]
	)

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
					const namesResponse = await httpClient.get(
						`/standard-construction-names`
					)
					setNameOptions(namesResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

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
				await httpClient.post(
					`/specifications/${specificationId}/standard-constructions`,
					{
						name: selectedObject.name,
						num: selectedObject.num,
						sheet: selectedObject.sheet,
						weight: selectedObject.weight,
					}
				)
                setScroll(2)
				history.push(`/specifications/${specificationId}`)
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
						selectedObject.weight === standardConstruction.weight
							? undefined
							: selectedObject.weight,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/standard-constructions/${selectedObject.id}`,
					object
				)
                setScroll(2)
				history.push(`/specifications/${specificationId}`)
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
			<div className="hanging-routes">
				<Link to="/specifications">Выпуски спецификаций</Link>
				<Link onClick={() => setScroll(2)} to={`/specifications/${specificationId}`}>Типовые конструкции</Link>
			</div>
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
						defaultValue={
							isNaN(selectedObject.num) ? '' : selectedObject.num
						}
						onBlur={onNumChange}
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
						defaultValue={selectedObject.sheet}
						onBlur={onSheetChange}
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
					disabled={processIsRunning}
				>
					{isCreateMode
						? 'Добавить типовую конструкцию'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default StandardConstructionData
