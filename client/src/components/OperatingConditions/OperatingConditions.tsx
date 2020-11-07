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
	>(null)
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

	const onCoeffChange = () => {}

	const onEnvAggresivnessSelect = async (id: number) => {}

	const onTempChange = () => {}

	const onChangeButtonClick = () => {}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Общие условия эксплуатации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group>
					<Form.Label htmlFor="coeff">
						Коэффициент надежности по ответственности
					</Form.Label>
					<Form.Control
						id="coeff"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите коэффициент надежности"
						value={selectedObject.safetyCoeff}
						onChange={onCoeffChange}
					/>
				</Form.Group>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '3.9em' }}
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
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>
				</div>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label htmlFor="temp">
						Расчетная температура эксплуатации (°C)
					</Form.Label>
					<Form.Control
						id="temp"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите наименование"
						value={selectedObject.temperature}
						onChange={onTempChange}
					/>
				</Form.Group>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '3.9em' }}
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
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '4.5em' }}
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
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '1em' }}
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
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '1em' }}
					>
						Тип лакокрасочного материала
					</label>
					<Select
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите тип лакокрасочного материала"
						noOptionsMessage={() => 'Типы не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={null}
						options={[]}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
						style={{ marginRight: '1em' }}
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
							onEnvAggresivnessSelect(
								(selectedOption as any)?.value
							)
						}
						value={null}
						options={[]}
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
