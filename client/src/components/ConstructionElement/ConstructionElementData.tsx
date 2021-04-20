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
import ConstructionElement from '../../model/ConstructionElement'
import ProfileClass from '../../model/ProfileClass'
import ProfileType from '../../model/ProfileType'
import Steel from '../../model/Steel'
import Profile from '../../model/Profile'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectStyle } from '../../util/react-select-style'

type IConstructionElementDataProps = {
	constructionElement: ConstructionElement
	isCreateMode: boolean
}

type ConstructionElementDataProps = {
	constructionElementData: IConstructionElementDataProps
	setConstructionElementData: (d: IConstructionElementDataProps) => void
	constructionElements: ConstructionElement[]
	setConstructionElements: (a: ConstructionElement[]) => void
	constructionId: number
}

const ConstructionElementData = ({
	constructionElementData,
	setConstructionElementData,
	constructionElements,
	setConstructionElements,
	constructionId
}: ConstructionElementDataProps) => {
	const mark = useMark()

	const profileType8Name = 'тонколистовая сталь'
	const profileType6Name = 'толстолистовая сталь'

	const defaultSelectedObject = {
		id: -1,
		profileClass: null,
		profile: null,
		steel: null,
		length: NaN,
		arithmeticExpression: '',
  } as ConstructionElement

  const [selectedObject, setSelectedObject] = useState<ConstructionElement>(null)

	// const [selectedObject, setSelectedObject] = useState<ConstructionElement>(
	// 	isCreateMode
	// 		? {
	// 				id: -1,
	// 				profileClass: null,
	// 				profile: null,
	// 				steel: null,
	// 				length: NaN,
	// 		  }
	// 		: {
	// 				...constructionElement,
	// 				profileClass: constructionElement.profile.class,
	// 		  }
	// )
	const [optionsObject, setOptionsObject] = useState({
		profileClasses: [] as ProfileClass[],
		profileTypes: [] as ProfileType[],
		steel: [] as Steel[],
		profiles: [] as Profile[],
	})

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const [width, setWidth] = useState<number>(null)
	const [thickness, setThickness] = useState<number>(null)

	useEffect(() => {
		const fetchData = async () => {
			try {
				const profileClassesResponse = await httpClient.get(
					`/profile-classes`
				)
				const steelResponse = await httpClient.get(`/steel`)

				setOptionsObject({
					...optionsObject,
					profileClasses: profileClassesResponse.data,
					steel: steelResponse.data,
				})

				if (!constructionElementData.isCreateMode) {
					const profilesResponse = await httpClient.get(
						`/profile-classes/${constructionElementData.constructionElement.profile.class.id}/profiles`
					)
					setOptionsObject({
						...optionsObject,
						profileClasses: profileClassesResponse.data,
						steel: steelResponse.data,
						profiles: profilesResponse.data,
					})
					return
				}
				setOptionsObject({
					...optionsObject,
					profileClasses: profileClassesResponse.data,
					steel: steelResponse.data,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
		if (constructionElementData.constructionElement != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...constructionElementData.constructionElement,
				profileClass: constructionElementData.constructionElement.profile.class,
			})
		} else {
			setSelectedObject({
				...defaultSelectedObject,
			})
		}
	}, [constructionElementData])

	const onProfileClassSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				profileClass: null,
				profile: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.profileClasses,
			selectedObject.profileClass
		)
		if (v != null) {
			const profilesResponse = await httpClient.get(
				`/profile-classes/${v.id}/profiles`
			)
			setOptionsObject({
				...optionsObject,
				profiles: profilesResponse.data,
			})

			setSelectedObject({
				...selectedObject,
				profileClass: v,
				profile: null,
			})
		}
	}

	const onProfileSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				profile: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.profiles,
			selectedObject.profile
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				profile: v,
			})
			if (
				selectedObject.profileClass.id == 16 ||
				selectedObject.profileClass.id == 17
			) {
				var splitted = v.name.split('*', 2)
				if (splitted.length > 1) {
					setWidth(splitted[0])
					setThickness(splitted[1])
				}
			}
		}
	}

	const onLengthChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			length: parseFloat(event.currentTarget.value),
		})
	}

	const onArithmeticExpressionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		try {
			length = eval(event.currentTarget.value)
			setSelectedObject({
				...selectedObject,
				length: length,
				arithmeticExpression: event.currentTarget.value,
			})
		} catch (e) {
			setSelectedObject({
				...selectedObject,
				arithmeticExpression: event.currentTarget.value,
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
		if (selectedObject.profile == null) {
			setErrMsg('Пожалуйста, выберите имя профиля')
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
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(
					`/constructions/${constructionId}/elements`,
					{
						profileId: selectedObject.profile.id,
						steelId: selectedObject.steel.id,
						length: selectedObject.length,
					}
				)
				const arr = [...constructionElements]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setConstructionElements(arr)
				setConstructionElementData({
					constructionElement: null,
					isCreateMode: false,
				})
			} catch (e) {
				setErrMsg('Произошла ошибка')
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
					profileId:
						selectedObject.profile.id ===
						constructionElementData.constructionElement.profile.id
							? undefined
							: selectedObject.profile.id,
					steelId:
						selectedObject.steel.id === constructionElementData.constructionElement.steel.id
							? undefined
							: selectedObject.steel.id,
					length:
						selectedObject.length === constructionElementData.constructionElement.length
							? undefined
							: selectedObject.length,
				}
				await httpClient.patch(
					`/construction-elements/${selectedObject.id}`,
					object
				)
				const arr = []
				for (const v of constructionElements) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setConstructionElements(arr)
				setConstructionElementData({
					constructionElement: null,
					isCreateMode: false,
				})
			} catch (e) {
				setErrMsg('Произошла ошибка')
				setProcessIsRunning(false)
			}
		} else {
			setProcessIsRunning(false)
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<div className="flex">
			<div className="shadow p-3 bg-white rounded component-width component-cnt-div relative">
					<div className="pointer absolute"
						style={{top: 5, right: 8}}
						onClick={() => setConstructionElementData({
							constructionElement: null,
							isCreateMode: false,
						})}
					>
						<X color="#666" size={33} />
					</div>
					<Form.Group>
						<Form.Label htmlFor="profileClass">
							Вид профиля
						</Form.Label>
						<Select
							inputId="profileClass"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите вид профиля"
							noOptionsMessage={() => 'Вид профиля не найден'}
							onChange={(selectedOption) =>
								onProfileClassSelect(
									(selectedOption as any)?.value
								)
							}
							value={
								selectedObject.profileClass == null
									? null
									: {
											value:
												selectedObject.profileClass.id,
											label:
												selectedObject.profileClass
													.name,
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

					<Form.Group>
						<Form.Label htmlFor="name">Имя профиля</Form.Label>
						<Select
							inputId="name"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите имя профиля"
							noOptionsMessage={() => 'Имя профиля не найдено'}
							onChange={(selectedOption) =>
								onProfileSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.profile == null
									? null
									: {
											value: selectedObject.profile.id,
											label: selectedObject.profile.name,
									  }
							}
							options={optionsObject.profiles.map((p) => {
								return {
									value: p.id,
									label: p.name,
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label htmlFor="steel">Марка стали</Form.Label>
						<Select
							inputId="steel"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите марку стали"
							noOptionsMessage={() => 'Марки стали не найдены'}
							onChange={(selectedOption) =>
								onSteelSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.steel == null
									? null
									: {
											value: selectedObject.steel.id,
											label:
												selectedObject.steel.name +
												' (' +
												selectedObject.steel.standard +
												')',
									  }
							}
							options={optionsObject.steel.map((s) => {
								return {
									value: s.id,
									label: s.name + ' (' + s.standard + ')',
								}
							})}
							styles={reactSelectStyle}
						/>
					</Form.Group>

					<Form.Group className="no-bot-mrg">
						<div className="space-between">
							<Form.Label htmlFor="length">
								Длина (м) или площадь (м<sup>2</sup>)
							</Form.Label>
							<Form.Label htmlFor="arithmeticExpression">
								Арифметическое выражение
							</Form.Label>
						</div>
						<div className="space-between">
							<Form.Control
								id="length"
								type="text"
								placeholder="Введите длину или площадь элемента"
								style={{width: 350}}
								autoComplete="off"
								value={
									isNaN(selectedObject.length)
										? ''
										: selectedObject.length
								}
								onChange={onLengthChange}
							/>

							<Form.Control
								id="arithmeticExpression"
								type="text"
								placeholder="Введите выражение"
								autoComplete="off"
								style={{marginLeft: 10}}
								value={selectedObject.arithmeticExpression}
								onChange={onArithmeticExpressionChange}
							/>
						</div>
					</Form.Group>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							constructionElementData.isCreateMode
								? onCreateButtonClick
								: onChangeButtonClick
						}
						disabled={processIsRunning || (!constructionElementData.isCreateMode && !Object.values({
							profileId:
								selectedObject.profile.id ===
								constructionElementData.constructionElement.profile.id
									? undefined
									: selectedObject.profile.id,
							steelId:
								selectedObject.steel.id === constructionElementData.constructionElement.steel.id
									? undefined
									: selectedObject.steel.id,
							length:
								selectedObject.length === constructionElementData.constructionElement.length
									? undefined
									: selectedObject.length,
						}).some((x) => x !== undefined))}
					>
						{constructionElementData.isCreateMode
							? 'Добавить элемент конструкции'
							: 'Сохранить изменения'}
					</Button>
				</div>

				<div className="info-area shadow p-3 mrg-left bg-white rounded component-width component-cnt-div">
					<Form.Group>
						<Form.Label>Символ профиля</Form.Label>
						<Form.Control
							type="text"
							value={
								selectedObject.profile == null
									? ''
									: selectedObject.profile.symbol
							}
							readOnly={true}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label>Вес 1 м профиля, кг</Form.Label>
						<Form.Control
							type="text"
							value={
								selectedObject.profile == null
									? ''
									: selectedObject.profileClass.id == 16 ||
									  selectedObject.profileClass.id == 17
									? Math.round(
											0.00785 *
												width *
												thickness *
												1000000000
									  ) / 1000000000
									: selectedObject.profile.weight
							}
							readOnly={true}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label>
							Площадь развернутой поверхности 1 м профиля, 100 м
							<sup>2</sup>
						</Form.Label>
						<Form.Control
							type="text"
							autoComplete="off"
							value={
								selectedObject.profile == null
									? ''
									: selectedObject.profileClass.id == 16 ||
									  selectedObject.profileClass.id == 17
									? Math.round(
											0.00002 *
												(width + thickness) *
												1000000000
									  ) / 1000000000
									: Math.round(
											selectedObject.profile.area *
												0.01 *
												1000000000
									  ) / 1000000000
							}
							readOnly={true}
						/>
					</Form.Group>

					<Form.Group>
						<Form.Label>Тип профиля</Form.Label>
						<Form.Control
							type="text"
							value={
								selectedObject.profile == null
									? ''
									: selectedObject.profileClass.id == 16 ||
									  selectedObject.profileClass.id == 17
									? width < 4
										? profileType8Name
										: profileType6Name
									: selectedObject.profile.type.name
							}
							readOnly={true}
						/>
					</Form.Group>
				</div>
			</div>
		</div>
	)
}

export default ConstructionElementData
