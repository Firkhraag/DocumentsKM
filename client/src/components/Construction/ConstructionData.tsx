// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Construction from '../../model/Construction'
import ConstructionType from '../../model/ConstructionType'
import ConstructionSubtype from '../../model/ConstructionSubtype'
import WeldingControl from '../../model/WeldingControl'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import { reactSelectstyle } from '../../util/react-select-style'

type ConstructionDataProps = {
	construction: Construction
	isCreateMode: boolean
}

const ConstructionData = ({
	construction,
	isCreateMode,
}: ConstructionDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<Construction>({
		id: -1,
		name: '',
		type: null,
		subtype: null,
		valuation: '',
		standardAlbumCode: '',
		numOfStandardConstructions: 0,
		paintworkCoeff: 0,
        weldingControl: null,
        hasEdgeBlunting: false,
        hasDynamicLoad: false,
        hasFlangedConnections: false,
	})
	const [
		defaultSelectedObject,
		setDefaultSelectedObject,
	] = useState<Construction>(construction)
	const [optionsObject, setOptionsObject] = useState({
		types: [] as ConstructionType[],
		subtypes: [] as ConstructionSubtype[],
		weldingControl: [] as WeldingControl[],
	})

	const [errMsg, setErrMsg] = useState('')

	const cachedSubtypes = useState(new Map<number, ConstructionSubtype[]>())[0]

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const constructionTypeResponse = await httpClient.get(
						`/construction-types`
					)
					const weldingControlResponse = await httpClient.get(
						`/welding-control`
					)
					setOptionsObject({
						...optionsObject,
						types: constructionTypeResponse.data,
						weldingControl: weldingControlResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onConstructionTypeSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...optionsObject,
				subtypes: [],
			})
			setSelectedObject({
				...selectedObject,
				type: null,
				subtype: null,
				name: '',
			})
			return
		}
		const v = getFromOptions(id, optionsObject.types, selectedObject.type)
		if (v != null) {
			if (cachedSubtypes.has(v.id)) {
				setOptionsObject({
					...optionsObject,
					subtypes: cachedSubtypes.get(v.id),
				})
				setSelectedObject({
					...selectedObject,
					type: v,
					subtype: null,
					name: v.name,
				})
			} else {
				try {
					const constructionSubtypeResponse = await httpClient.get(
						`/construction-types/${id}/construction-subtypes`
					)
					cachedSubtypes.set(v.id, constructionSubtypeResponse.data)
					setOptionsObject({
						...optionsObject,
						subtypes: constructionSubtypeResponse.data,
					})
					setSelectedObject({
						...selectedObject,
						type: v,
						subtype: null,
						name: v.name,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
		}
	}

	const onConstructionSubtypeSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				subtype: null,
				name: selectedObject.type.name,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.subtypes,
			selectedObject.subtype
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				subtype: v,
				name: selectedObject.name + ' ' + v.name,
				valuation: v.valuation,
			})
		}
    }
    
    const onValuationChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			valuation: event.currentTarget.value,
		})
    }
    
    const onStandardAlbumCodeChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			standardAlbumCode: event.currentTarget.value,
		})
    }
    
    const onNumOfStandardConstructionsChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			numOfStandardConstructions: parseInt(event.currentTarget.value),
		})
    }
    
    const onPaintworkCoeffChange = (event: React.FormEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			paintworkCoeff: parseFloat(event.currentTarget.value),
		})
    }

	const onWeldingControlSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				weldingControl: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.weldingControl,
			selectedObject.weldingControl
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				weldingControl: v,
			})
		}
    }
    
    const onEdgeBluntingCheck = () => {
        setSelectedObject({
			...selectedObject,
			hasEdgeBlunting: !selectedObject.hasEdgeBlunting,
		})
    }

    const onDynamicLoadCheck = () => {
        setSelectedObject({
			...selectedObject,
			hasDynamicLoad: !selectedObject.hasDynamicLoad,
		})
    }

    const onFlangedConnectionsCheck = () => {
        setSelectedObject({
			...selectedObject,
			hasFlangedConnections: !selectedObject.hasFlangedConnections,
		})
    }

	const checkIfValid = () => {
		if (selectedObject.type == null) {
			setErrMsg('Пожалуйста, выберите тип конструкции')
			return false
		}
		if (selectedObject.weldingControl == null) {
			setErrMsg('Пожалуйста, выберите контроль плотности сварных швов')
			return false
		}
		return true
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
				{isCreateMode
					? 'Добавление вида конструкции'
					: 'Данные по виду конструкций'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<label className="bold no-bot-mrg">Шифр вида конструкции</label>
				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Выберите вид конструкции"
					noOptionsMessage={() => 'Вид конструкции не найден'}
					className="mrg-top"
					onChange={(selectedOption) =>
						onConstructionTypeSelect((selectedOption as any)?.value)
					}
					value={
						selectedObject.type == null
							? null
							: {
									value: selectedObject.type.id,
									label: selectedObject.type.name,
							  }
					}
					options={optionsObject.types.map((t) => {
						return {
							value: t.id,
							label: t.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<label className="bold no-bot-mrg mrg-top-2">
					Шифр подвида конструкции
				</label>
				<Select
					maxMenuHeight={250}
					isClearable={true}
					isSearchable={true}
					placeholder="Выберите подвид конструкции"
					noOptionsMessage={() => 'Подвид конструкции не найден'}
					className="mrg-top"
					onChange={(selectedOption) =>
						onConstructionSubtypeSelect(
							(selectedOption as any)?.value
						)
					}
					value={
						selectedObject.subtype == null
							? null
							: {
									value: selectedObject.subtype.id,
									label: selectedObject.subtype.name,
							  }
					}
					options={optionsObject.subtypes.map((s) => {
						return {
							value: s.id,
							label: s.name,
						}
					})}
					styles={reactSelectstyle}
				/>

				<Form.Group className="mrg-top-2">
					<Form.Label htmlFor="name">Название</Form.Label>
					<Form.Control
						id="name"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						value={selectedObject.name}
						readOnly={true}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
                        className="no-bot-mrg"
						htmlFor="valuation"
						style={{ marginRight: '13.35em' }}
					>
						Расценка
					</Form.Label>
					<Form.Control
						id="valuation"
						type="text"
						placeholder="Введите расценку"
						className="auto-width flex-grow"
						defaultValue={selectedObject.valuation}
						onBlur={onValuationChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
                        className="no-bot-mrg"
						htmlFor="standardAlbumCode"
						style={{ marginRight: '5.8em' }}
					>
						Шифр типового альбома
					</Form.Label>
					<Form.Control
						id="standardAlbumCode"
						type="text"
						placeholder="Не введено"
						className="auto-width flex-grow"
						defaultValue={''}
						onBlur={onStandardAlbumCodeChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
                        className="no-bot-mrg"
						htmlFor="numOfStandardConstructions"
						style={{ marginRight: '4.15em' }}
					>
						Число типовых конструкций
					</Form.Label>
					<Form.Control
						id="numOfStandardConstructions"
						type="text"
						placeholder="Не введено"
						className="auto-width flex-grow"
						defaultValue={''}
						onBlur={onNumOfStandardConstructionsChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
                        className="no-bot-mrg"
						htmlFor="paintworkCoeff"
						style={{ marginRight: '4.5em' }}
					>
						Коэффициент окрашивания
					</Form.Label>
					<Form.Control
						id="paintworkCoeff"
						type="text"
						placeholder="Не введено"
						defaultValue={''}
						className="auto-width flex-grow"
						onBlur={onPaintworkCoeffChange}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
                    <Form.Label
                        className="no-bot-mrg"
						htmlFor="weldingControl"
						style={{ marginRight: '1em' }}
					>
						Контроль плотности сварных швов
					</Form.Label>
					<Select
                        inputId="weldingControl"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите контроль плотности"
						noOptionsMessage={() => 'Контроль плотности не найден'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onWeldingControlSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.weldingControl == null
								? null
								: {
										value: selectedObject.weldingControl.id,
										label:
											selectedObject.weldingControl.name,
								  }
						}
						options={optionsObject.weldingControl.map((c) => {
							return {
								value: c.id,
								label: c.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
						htmlFor="edgeBlunting"
						style={{ marginRight: '7.6em' }}
					>
						Притупление кромок
					</Form.Label>
					<Form.Check
						id="edgeBlunting"
						type="checkbox"
                        className="checkmark"
                        onChange={onEdgeBluntingCheck}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2">
					<Form.Label
						htmlFor="dynamicLoad"
						style={{ marginRight: '6.4em' }}
					>
						Динамическая нагрузка
					</Form.Label>
					<Form.Check
						id="dynamicLoad"
						type="checkbox"
                        className="checkmark"
                        onChange={onDynamicLoadCheck}
					/>
				</Form.Group>

				<Form.Group className="flex-cent-v mrg-top-2 no-bot-mrg">
					<Form.Label
						htmlFor="flangedConnections"
						style={{ marginRight: '6.4em' }}
					>
						Фланцевые соединения
					</Form.Label>
					<Form.Check
						id="flangedConnections"
						type="checkbox"
                        className="checkmark"
                        onChange={onFlangedConnectionsCheck}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={null}
				>
					{'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default ConstructionData
