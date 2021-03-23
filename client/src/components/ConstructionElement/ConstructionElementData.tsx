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
import Profile from '../../model/Profile'
import { useMark } from '../../store/MarkStore'
import { useSetScroll } from '../../store/ScrollStore'
import getFromOptions from '../../util/get-from-options'
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
	const setScroll = useSetScroll()

	const profileType8Name = 'тонколистовая сталь'
	const profileType6Name = 'толстолистовая сталь'

	const [selectedObject, setSelectedObject] = useState<ConstructionElement>(
		isCreateMode
			? {
					id: -1,
					profileClass: null,
					profile: null,
					steel: null,
					length: NaN,
			  }
			: {
					...constructionElement,
					profileClass: constructionElement.profile.class,
			  }
	)
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
		if (mark != null && mark.id != null) {
			if (selectedObject == null || specificationId == -1) {
				history.push('/specifications')
				return
			}
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

					if (!isCreateMode) {
						const profilesResponse = await httpClient.get(
							`/profile-classes/${selectedObject.profileClass.id}/profiles`
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
		}
	}, [mark])

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

	const onLengthChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			length: parseFloat(event.currentTarget.value),
		})
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
				await httpClient.post(
					`/constructions/${constructionId}/elements`,
					{
						profileId: selectedObject.profile.id,
						steelId: selectedObject.steel.id,
						length: selectedObject.length,
					}
				)
				setScroll(3)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
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
						constructionElement.profile.id
							? undefined
							: selectedObject.profile.id,
					steelId:
						selectedObject.steel.id === constructionElement.steel.id
							? undefined
							: selectedObject.steel.id,
					length:
						selectedObject.length === constructionElement.length
							? undefined
							: selectedObject.length,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(
					`/construction-elements/${selectedObject.id}`,
					object
				)
				setScroll(3)
				history.push(
					`/specifications/${specificationId}/constructions/${constructionId}`
				)
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
			<h1 className="text-centered">
				{isCreateMode
					? 'Создание элемента конструкции'
					: 'Данные элемента конструкции'}
			</h1>

			<div className="flex">
				<div className="info-area shadow p-3 bg-white rounded component-width component-cnt-div">
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

				<div className="shadow p-3 bg-white rounded mrg-left component-width component-cnt-div">
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
						<Form.Label htmlFor="length">
							Длина (м) или площадь (м<sup>2</sup>)
						</Form.Label>
						<Form.Control
							id="length"
							type="text"
							placeholder="Введите длину или площадь элемента"
							autoComplete="off"
							defaultValue={
								isNaN(selectedObject.length)
									? ''
									: selectedObject.length
							}
							onBlur={onLengthChange}
						/>
					</Form.Group>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

					<Button
						variant="secondary"
						className="btn-mrg-top-2 full-width"
						onClick={
							isCreateMode
								? onCreateButtonClick
								: onChangeButtonClick
						}
						disabled={processIsRunning}
					>
						{isCreateMode
							? 'Добавить элемент конструкции'
							: 'Сохранить изменения'}
					</Button>
				</div>
			</div>
		</div>
	)
}

export default ConstructionElementData
