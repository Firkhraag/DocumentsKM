// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import MarkOperatingConditions from '../../model/MarkOperatingConditions'
import EnvAggressiveness from '../../model/EnvAggressiveness'
import OperatingArea from '../../model/OperatingArea'
import GasGroup from '../../model/GasGroup'
import ConstructionMaterial from '../../model/ConstructionMaterial'
import PaintworkType from '../../model/PaintworkType'
import HighTensileBoltsType from '../../model/HighTensileBoltsType'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import Mark from '../../model/Mark'
import { useMark, useSetMark } from '../../store/MarkStore'
import { makeMarkName, makeComplexAndObjectName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

const OperatingConditions = () => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<
		MarkOperatingConditions
	>({
		id: -1,
		safetyCoeff: 1.0,
		envAggressiveness: null,
		temperature: -34,
		operatingArea: null,
		gasGroup: null,
		constructionMaterial: null,
		paintworkType: null,
		highTensileBoltsType: null,
	})
	const [optionsObject, setOptionsObject] = useState({
		envAggressiveness: [] as EnvAggressiveness[],
		operatingAreas: [] as OperatingArea[],
		gasGroups: [] as GasGroup[],
		constructionMaterials: [] as ConstructionMaterial[],
		paintworkTypes: [] as PaintworkType[],
		highTensileBoltsTypes: [] as HighTensileBoltsType[],
	})

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const envAggressivenessResponse = await httpClient.get(
						`/env-aggressiveness`
					)
					const operatingAreasResponse = await httpClient.get(
						`/operating-areas`
					)
					const gasGroupsResponse = await httpClient.get(
						`/gas-groups`
					)
					const constructionMaterialsResponse = await httpClient.get(
						`/construction-materials`
					)
					const paintworkTypesResponse = await httpClient.get(
						`/paintwork-types`
					)
					const highTensileBoltsTypesResponse = await httpClient.get(
						`/high-tensile-bolts-types`
					)
					setOptionsObject({
						envAggressiveness: envAggressivenessResponse.data,
						operatingAreas: operatingAreasResponse.data,
						gasGroups: gasGroupsResponse.data,
						constructionMaterials:
							constructionMaterialsResponse.data,
						paintworkTypes: paintworkTypesResponse.data,
						highTensileBoltsTypes:
							highTensileBoltsTypesResponse.data,
					})
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

    const onCoeffChange = (event: React.FormEvent<HTMLInputElement>) => {
		try {
			const v = parseFloat(event.currentTarget.value)
			setSelectedObject({
				...selectedObject,
				safetyCoeff: v,
			})
		} catch (e) {
			setSelectedObject({
				...selectedObject,
				safetyCoeff: null,
			})
		}
	}

	const onEnvAggresivnessSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				envAggressiveness: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.envAggressiveness,
			selectedObject
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				envAggressiveness: v,
			})
		}
	}

    const onTempChange = (event: React.FormEvent<HTMLInputElement>) => {
		try {
			const v = parseInt(event.currentTarget.value)
			setSelectedObject({
				...selectedObject,
				temperature: v,
			})
		} catch (e) {
			setSelectedObject({
				...selectedObject,
				temperature: null,
			})
		}
	}

	const onOperatingAreaSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				operatingArea: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.operatingAreas,
			selectedObject
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				operatingArea: v,
			})
		}
	}

	const onGasGroupSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				gasGroup: null,
			})
		}
		const v = getFromOptions(id, optionsObject.gasGroups, selectedObject)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				gasGroup: v,
			})
		}
	}

	const onConstructionMaterialSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				constructionMaterial: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.constructionMaterials,
			selectedObject
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				constructionMaterial: v,
			})
		}
	}

	const onPaintworkTypeSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				paintworkType: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.paintworkTypes,
			selectedObject
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				paintworkType: v,
			})
		}
	}

	const onHighTensileBoltsTypeSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				highTensileBoltsType: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.highTensileBoltsTypes,
			selectedObject
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				highTensileBoltsType: v,
			})
		}
	}

	const onChangeButtonClick = () => {}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Общие условия эксплуатации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<Form.Group className="flex-cent-v">
					<Form.Label htmlFor="coeff" style={{ marginRight: '2.5em' }}>
						Коэф. надежности по ответственности
					</Form.Label>
					<Form.Control
						id="coeff"
						type="text"
						placeholder="Введите коэффициент надежности"
						value={selectedObject.safetyCoeff.toFixed(1)}
						className="auto-width flex-grow"
						onBlur={onCoeffChange}
					/>
				</Form.Group>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '10.5em' }}
					>
						Агрессивность среды
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите агрессивность среды"
						noOptionsMessage={() =>
							'Агрессивность среды не найдена'
						}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.envAggressiveness == null
								? null
								: {
										value:
											selectedObject.envAggressiveness.id,
										label:
											selectedObject.envAggressiveness
												.name,
								  }
						}
						options={optionsObject.envAggressiveness.map((a) => {
							return {
								value: a.id,
								label: a.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label htmlFor="temp" style={{ marginRight: '1em' }}>
						Расчетная температура эксплуатации (°C)
					</Form.Label>
					<Form.Control
						id="temp"
						type="text"
						placeholder="Введите наименование"
						className="auto-width flex-grow"
						value={selectedObject.temperature}
						onBlur={onTempChange}
					/>
				</Form.Group>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '11.65em' }}
					>
						Зона эксплуатации
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите зону эксплуатации"
						noOptionsMessage={() => 'Зоны эксплуатации не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onOperatingAreaSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.operatingArea == null
								? null
								: {
										value: selectedObject.operatingArea.id,
										label:
											selectedObject.operatingArea.name,
								  }
						}
						options={optionsObject.operatingAreas.map((a) => {
							return {
								value: a.id,
								label: a.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '14.65em' }}
					>
						Группа газов
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите группу газов"
						noOptionsMessage={() => 'Группы газов не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onGasGroupSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.gasGroup == null
								? null
								: {
										value: selectedObject.gasGroup.id,
										label: selectedObject.gasGroup.name,
								  }
						}
						options={optionsObject.gasGroups.map((g) => {
							return {
								value: g.id,
								label: g.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '9.65em' }}
					>
						Материал конструкций
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите материал конструкций"
						noOptionsMessage={() =>
							'Материалы конструкций не найдены'
						}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onConstructionMaterialSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.constructionMaterial == null
								? null
								: {
										value:
											selectedObject.constructionMaterial
												.id,
										label:
											selectedObject.constructionMaterial
												.name,
								  }
						}
						options={optionsObject.constructionMaterials.map(
							(m) => {
								return {
									value: m.id,
									label: m.name,
								}
							}
						)}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '5.75em' }}
					>
						Тип лакокрасочного материала
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите тип"
						noOptionsMessage={() => 'Типы не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onPaintworkTypeSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.paintworkType == null
								? null
								: {
										value: selectedObject.paintworkType.id,
										label:
											selectedObject.paintworkType.name,
								  }
						}
						options={optionsObject.paintworkTypes.map((t) => {
							return {
								value: t.id,
								label: t.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '9.65em' }}
					>
						Высокопрочные болты
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите высокопрочные болты"
						noOptionsMessage={() =>
							'Высокопрочные болты не найдены'
						}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onHighTensileBoltsTypeSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.highTensileBoltsType == null
								? null
								: {
										value:
											selectedObject.highTensileBoltsType
												.id,
										label:
											selectedObject.highTensileBoltsType
												.name,
								  }
						}
						options={optionsObject.highTensileBoltsTypes.map(
							(t) => {
								return {
									value: t.id,
									label: t.name,
								}
							}
						)}
						styles={reactSelectstyle}
					/>
				</div>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={onChangeButtonClick}
				>
					{'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default OperatingConditions
