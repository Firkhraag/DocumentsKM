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
import ConstructionElement from '../../model/ConstructionElement'
import ProfileClass from '../../model/ProfileClass'
import ProfileType from '../../model/ProfileType'
import Steel from '../../model/Steel'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

type ConstructionElementDataProps = {
	constructionElement: ConstructionElement
	isCreateMode: boolean
	specificationId: number
	constructionId: number
}

const ConstructionElementData = ({
	constructionElement,
	isCreateMode,
	specificationId,
	constructionId,
}: ConstructionElementDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<ConstructionElement>(
		isCreateMode
			? {
					id: -1,
					profileClass: null,
					profileName: '',
					symbol: '',
					weight: NaN,
					surfaceArea: NaN,
					profileType: null,
					steel: null,
					length: NaN,
					status: NaN,
			  }
			: constructionElement
	)
	const [optionsObject, setOptionsObject] = useState({
		profileClasses: [] as ProfileClass[],
		profileTypes: [] as ProfileType[],
		steel: [] as Steel[],
	})
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null || specificationId == -1) {
				history.push('/specifications')
				return
			}
			const fetchData = async () => {
				try {
					const profileClassResponse = await httpClient.get(
						`/profile-classes`
					)
					const profileTypeResponse = await httpClient.get(
						`/profile-types`
					)
                    const steelResponse = await httpClient.get(`/steel`)
                    console.log(profileClassResponse.data)
					setOptionsObject({
						profileClasses: profileClassResponse.data,
						profileTypes: profileTypeResponse.data,
						steel: steelResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onProfileClassSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				profileClass: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.profileClasses,
			selectedObject.profileClass
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				profileClass: v,
			})
		}
	}

	const onProfileNameChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			profileName: event.currentTarget.value,
		})
	}

	const onSymbolChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			symbol: event.currentTarget.value,
		})
	}

	const onWeightChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			weight: parseFloat(event.currentTarget.value),
		})
	}

	const onSurfaceAreaChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			surfaceArea: parseFloat(event.currentTarget.value),
		})
	}

	const onLengthChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			length: parseFloat(event.currentTarget.value),
		})
	}

	const onStatusChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			status: parseInt(event.currentTarget.value),
		})
	}

	const onProfileTypeSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				profileType: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.profileTypes,
			selectedObject.profileType
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				profileType: v,
			})
		}
	}

	const onSteelSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				steel: null,
			})
		}
		const v = getFromOptions(id, optionsObject.steel, selectedObject.steel)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				steel: v,
			})
		}
	}

	const checkIfValid = () => {
		if (selectedObject.profileClass == null) {
			setErrMsg('Пожалуйста, выберите вид профиля')
			return false
		}
		if (selectedObject.profileName == null) {
			setErrMsg('Пожалуйста, введите имя профиля')
			return false
		}
		if (selectedObject.symbol == null) {
			setErrMsg('Пожалуйста, введите символ профиля')
			return false
		}
		if (isNaN(selectedObject.weight)) {
			setErrMsg('Пожалуйста, введите вес профиля')
			return false
		}
		if (isNaN(selectedObject.surfaceArea)) {
			setErrMsg('Пожалуйста, введите площадь поверхности')
			return false
		}
		if (selectedObject.profileType == null) {
			setErrMsg('Пожалуйста, выберите тип профиля')
			return false
		}
		if (selectedObject.steel == null) {
			setErrMsg('Пожалуйста, выберите марку стали')
			return false
		}
		if (isNaN(selectedObject.length)) {
			setErrMsg('Пожалуйста, введите длину поверхности')
			return false
		}
		if (isNaN(selectedObject.status)) {
			setErrMsg('Пожалуйста, введите статус')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(
					`/constructions/${constructionId}/elements`,
					{
						profileClass: selectedObject.profileClass,
						profileName: selectedObject.profileName,
						symbol: selectedObject.symbol,
						weight: selectedObject.weight,
						surfaceArea: selectedObject.surfaceArea,
						profileType: selectedObject.profileType,
						steel: selectedObject.steel,
						length: selectedObject.length,
						status: selectedObject.status,
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
					`/construction-elements/${selectedObject.id}`,
					{
						profileClassId:
							selectedObject.profileClass.id ===
							constructionElement.profileClass.id
								? undefined
								: selectedObject.profileClass.id,
						profileName:
							selectedObject.profileName ===
							constructionElement.profileName
								? undefined
								: selectedObject.profileName,
						symbol:
							selectedObject.symbol === constructionElement.symbol
								? undefined
								: selectedObject.symbol,
						weight:
							selectedObject.weight === constructionElement.weight
								? undefined
								: selectedObject.weight,
						surfaceArea:
							selectedObject.surfaceArea ===
							constructionElement.surfaceArea
								? undefined
								: selectedObject.surfaceArea,
						profileTypeId:
							selectedObject.profileType.id ===
							constructionElement.profileType.id
								? undefined
								: selectedObject.profileType.id,
						steelId:
							selectedObject.steel.id ===
							constructionElement.steel.id
								? undefined
								: selectedObject.steel.id,
						length:
							selectedObject.length === constructionElement.length
								? undefined
								: selectedObject.length,
						status:
							selectedObject.status === constructionElement.status
								? undefined
								: selectedObject.status,
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
					? 'Создание элемента конструкции'
					: 'Данные элемента конструкцииа'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="profileClass"
						style={{ marginRight: '4.3em' }}
					>
						Вид профиля
					</Form.Label>
					<Select
						inputId="profileClass"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите вид профиля"
						noOptionsMessage={() => 'Вид профиля не найден'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onProfileClassSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.profileClass == null
								? null
								: {
										value: selectedObject.profileClass.id,
										label: selectedObject.profileClass.name,
								  }
						}
						options={optionsObject.profileClasses.map((pc) => {
							return {
								value: pc.id,
								label: pc.name,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="profileName"
						style={{ marginRight: '3.85em' }}
					>
						Имя профиля
					</Form.Label>
					<Form.Control
						id="profileName"
						type="text"
						placeholder="Введите имя профиля"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={selectedObject.profileName}
						onBlur={onProfileNameChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="symbol"
						style={{ marginRight: '4.05em' }}
					>
						Символ профиля
					</Form.Label>
					<Form.Control
						id="symbol"
						type="text"
						placeholder="Введите символ профиля"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={selectedObject.symbol}
						onBlur={onSymbolChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="weight"
						style={{ marginRight: '1.6em' }}
					>
						Вес 1 м профиля, кг
					</Form.Label>
					<Form.Control
						id="weight"
						type="text"
						placeholder="Введите вес профиля"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.weight)
								? ''
								: selectedObject.weight
						}
						onBlur={onWeightChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="surfaceArea"
						style={{ marginRight: '1em' }}
					>
						Площадь поверхности
					</Form.Label>
					<Form.Control
						id="surfaceArea"
						type="text"
						placeholder="Введите площадь поверхности"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.surfaceArea)
								? ''
								: selectedObject.surfaceArea
						}
						onBlur={onSurfaceAreaChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="profileType"
						style={{ marginRight: '4.3em' }}
					>
						Тип профиля
					</Form.Label>
					<Select
						inputId="profileType"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите тип профиля"
						noOptionsMessage={() => 'Тип профиля не найден'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onProfileTypeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.profileType == null
								? null
								: {
										value: selectedObject.profileType.id,
										label: selectedObject.profileType.name,
								  }
						}
						options={optionsObject.profileTypes.map((pt) => {
							return {
								value: pt.id,
								label: pt.name,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="steel"
						style={{ marginRight: '4.3em' }}
					>
						Марка стали
					</Form.Label>
					<Select
						inputId="steel"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите марку стали"
						noOptionsMessage={() => 'Марки стали не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onSteelSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.steel == null
								? null
								: {
										value: selectedObject.steel.id,
										label: selectedObject.steel.name,
								  }
						}
						options={optionsObject.steel.map((s) => {
							return {
								value: s.id,
								label: s.name,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="length"
						style={{ marginRight: '1.6em' }}
					>
						Длина поверхности
					</Form.Label>
					<Form.Control
						id="length"
						type="text"
						placeholder="Введите длину поверхности"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.length)
								? ''
								: selectedObject.length
						}
						onBlur={onLengthChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="status"
						style={{ marginRight: '1em' }}
					>
						Статус
					</Form.Label>
					<Form.Control
						id="status"
						type="text"
						placeholder="Введите статус"
						className="auto-width flex-grow"
						autoComplete="off"
						defaultValue={
							isNaN(selectedObject.status)
								? ''
								: selectedObject.status
						}
						onBlur={onStatusChange}
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
						? 'Добавить элемент конструкции'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default ConstructionElementData
