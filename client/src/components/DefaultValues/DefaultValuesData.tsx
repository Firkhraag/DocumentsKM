// // Global
// import React, { useState, useEffect } from 'react'
// import { useHistory } from 'react-router-dom'
// import Select from 'react-select'
// // Bootstrap
// import Form from 'react-bootstrap/Form'
// import Button from 'react-bootstrap/Button'
// // Util
// import httpClient from '../../axios'
// import Employee from '../../model/Employee'
// import Department from '../../model/Department'
// import DefaultValues from '../../model/DefaultValues'
// import GasGroup from '../../model/GasGroup'
// import ConstructionMaterial from '../../model/ConstructionMaterial'
// import PaintworkType from '../../model/PaintworkType'
// import HighTensileBoltsType from '../../model/HighTensileBoltsType'
// import ErrorMsg from '../ErrorMsg/ErrorMsg'
// import { useUser } from '../../store/UserStore'
// import getFromOptions from '../../util/get-from-options'
// import { reactSelectStyle } from '../../util/react-select-style'

// const DefaultValuesData = () => {
// 	const history = useHistory()
//     const user = useUser()

//     const [selectedObject, setSelectedObject] = useState<DefaultValues>({
//         id: -1,
//         department: user.employee.department,
// 		creator: user.employee,
//         inspector: null,
//         normContr: null,
// 	})
//     const [optionsObject, setOptionsObject] = useState({
// 		departments: [] as Department[],
// 		employees: [] as Employee[],
// 	})

// 	const [processIsRunning, setProcessIsRunning] = useState(false)
// 	const [errMsg, setErrMsg] = useState('')

// 	useEffect(() => {
// 		const fetchData = async () => {
//             try {
//                 const employeesResponse = await httpClient.get(
//                     `/departments/${user.employee.department.id}/employees`
//                 )
//                 const departmentsResponse = await httpClient.get(
//                     `/departments`
//                 )

//                 setOptionsObject({
//                     departments: departmentsResponse.data,
//                     employees: employeesResponse.data,
//                 })
//             } catch (e) {
//                 console.log('Failed to fetch the data')
//             }
//         }
//         fetchData()
// 	})

// 	const onDepartmentSelect = async (number: number) => {
// 		if (number == null) {
// 			setOptionsObject({
// 				...defaultOptionsObject,
// 				departments: optionsObject.departments,
// 			})
// 			setSelectedObject({
// 				...selectedObject,
// 				department: null,
// 				chiefSpecialist: null,
// 				groupLeader: null,
// 				mainBuilder: null,
// 			})
// 		}
// 		const v = getFromOptions(
// 			number,
// 			optionsObject.departments,
// 			selectedObject.department
// 		)
// 		if (v != null) {
// 			if (cachedMainEmployees.has(v.id)) {
// 				setOptionsObject({
// 					...defaultOptionsObject,
// 					departments: optionsObject.departments,
// 					chiefSpecialists: cachedMainEmployees.get(v.id)
// 						.chiefSpecialists,
// 					groupLeaders: cachedMainEmployees.get(v.id).groupLeaders,
// 					mainBuilders: cachedMainEmployees.get(v.id).mainBuilders,
// 				})
// 				setSelectedObject({
// 					...selectedObject,
// 					department: v,
// 					chiefSpecialist: null,
// 					groupLeader: null,
// 					mainBuilder: null,
// 				})
// 				setDepartmentHead(cachedMainEmployees.get(v.id).departmentHead)
// 			} else {
// 				try {
// 					const fetchedMainEmployeesResponse = await httpClient.get(
// 						`departments/${number}/mark-main-employees`
// 					)
// 					const fetchedMainEmployees =
// 						fetchedMainEmployeesResponse.data
// 					cachedMainEmployees.set(v.id, fetchedMainEmployees)
// 					setOptionsObject({
// 						...defaultOptionsObject,
// 						departments: optionsObject.departments,
// 						chiefSpecialists: fetchedMainEmployees.chiefSpecialists,
// 						groupLeaders: fetchedMainEmployees.groupLeaders,
// 						mainBuilders: fetchedMainEmployees.mainBuilders,
// 					})
// 					setSelectedObject({
// 						...selectedObject,
// 						department: v,
// 						chiefSpecialist: null,
// 						groupLeader: null,
// 						mainBuilder: null,
// 					})
// 					setDepartmentHead(fetchedMainEmployees.departmentHead)
// 				} catch (e) {
// 					setErrMsg('Произошла ошибка')
// 				}
// 			}
// 		}
// 	}

// 	const onTempChange = (event: React.FormEvent<HTMLInputElement>) => {
// 		setSelectedObject({
// 			...selectedObject,
// 			temperature: parseInt(event.currentTarget.value),
// 		})
// 	}

// 	const onOperatingAreaSelect = (id: number) => {
// 		if (id == null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				operatingArea: null,
// 			})
// 		}
// 		const v = getFromOptions(
// 			id,
// 			optionsObject.operatingAreas,
// 			selectedObject
// 		)
// 		if (v != null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				operatingArea: v,
// 			})
// 		}
// 	}

// 	const onGasGroupSelect = (id: number) => {
// 		if (id == null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				gasGroup: null,
// 			})
// 		}
// 		const v = getFromOptions(id, optionsObject.gasGroups, selectedObject)
// 		if (v != null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				gasGroup: v,
// 			})
// 		}
// 	}

// 	const onConstructionMaterialSelect = (id: number) => {
// 		if (id == null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				constructionMaterial: null,
// 			})
// 		}
// 		const v = getFromOptions(
// 			id,
// 			optionsObject.constructionMaterials,
// 			selectedObject
// 		)
// 		if (v != null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				constructionMaterial: v,
// 			})
// 		}
// 	}

// 	const onPaintworkTypeSelect = (id: number) => {
// 		if (id == null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				paintworkType: null,
// 			})
// 		}
// 		const v = getFromOptions(
// 			id,
// 			optionsObject.paintworkTypes,
// 			selectedObject
// 		)
// 		if (v != null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				paintworkType: v,
// 			})
// 		}
// 	}

// 	const onHighTensileBoltsTypeSelect = (id: number) => {
// 		if (id == null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				highTensileBoltsType: null,
// 			})
// 		}
// 		const v = getFromOptions(
// 			id,
// 			optionsObject.highTensileBoltsTypes,
// 			selectedObject
// 		)
// 		if (v != null) {
// 			setSelectedObject({
// 				...selectedObject,
// 				highTensileBoltsType: v,
// 			})
// 		}
// 	}

// 	const checkIfValid = () => {
// 		if (isNaN(selectedObject.safetyCoeff)) {
// 			setErrMsg('Пожалуйста, введите коэффициент надежности')
// 			return false
// 		}
// 		if (selectedObject.envAggressiveness == null) {
// 			setErrMsg('Пожалуйста, введите агрессивность среды')
// 			return false
// 		}
// 		if (isNaN(selectedObject.temperature)) {
// 			setErrMsg('Пожалуйста, введите температуру эксплуатации')
// 			return false
// 		}
// 		if (selectedObject.operatingArea == null) {
// 			setErrMsg('Пожалуйста, выберите зону эксплуатации')
// 			return false
// 		}
// 		if (selectedObject.gasGroup == null) {
// 			setErrMsg('Пожалуйста, выберите группу газов')
// 			return false
// 		}
// 		if (selectedObject.constructionMaterial == null) {
// 			setErrMsg('Пожалуйста, выберите материал конструкций')
// 			return false
// 		}
// 		if (selectedObject.paintworkType == null) {
// 			setErrMsg('Пожалуйста, выберите тип лакокрасочного материала')
// 			return false
// 		}
// 		if (selectedObject.highTensileBoltsType == null) {
// 			setErrMsg('Пожалуйста, выберите высокопрочные болты')
// 			return false
// 		}
// 		return true
// 	}

// 	const onCreateButtonClick = async () => {
// 		setProcessIsRunning(true)
// 		if (checkIfValid()) {
// 			try {
// 				await httpClient.post(
// 					`/marks/${mark.id}/mark-operating-conditions`,
// 					{
// 						safetyCoeff: selectedObject.safetyCoeff,
// 						temperature: selectedObject.temperature,
// 						envAggressivenessId:
// 							selectedObject.envAggressiveness.id,
// 						operatingAreaId: selectedObject.operatingArea.id,
// 						gasGroupId: selectedObject.gasGroup.id,
// 						constructionMaterialId:
// 							selectedObject.constructionMaterial.id,
// 						paintworkTypeId: selectedObject.paintworkType.id,
// 						highTensileBoltsTypeId:
// 							selectedObject.highTensileBoltsType.id,
// 					}
// 				)
// 				history.push('/')
// 			} catch (e) {
// 				setErrMsg('Произошла ошибка')
// 				setProcessIsRunning(false)
// 			}
// 		} else {
// 			setProcessIsRunning(false)
// 		}
// 	}

// 	const onChangeButtonClick = async () => {
// 		setProcessIsRunning(true)
// 		if (checkIfValid()) {
// 			try {
// 				const object = {
// 					safetyCoeff:
// 						selectedObject.safetyCoeff ===
// 						defaultSelectedObject.safetyCoeff
// 							? undefined
// 							: selectedObject.safetyCoeff,
// 					temperature:
// 						selectedObject.temperature ===
// 						defaultSelectedObject.temperature
// 							? undefined
// 							: selectedObject.temperature,
// 					envAggressivenessId:
// 						selectedObject.envAggressiveness.id ===
// 						defaultSelectedObject.envAggressiveness.id
// 							? undefined
// 							: selectedObject.envAggressiveness.id,
// 					operatingAreaId:
// 						selectedObject.operatingArea.id ===
// 						defaultSelectedObject.operatingArea.id
// 							? undefined
// 							: selectedObject.operatingArea.id,
// 					gasGroupId:
// 						selectedObject.gasGroup.id ===
// 						defaultSelectedObject.gasGroup.id
// 							? undefined
// 							: selectedObject.gasGroup.id,
// 					constructionMaterialId:
// 						selectedObject.constructionMaterial.id ===
// 						defaultSelectedObject.constructionMaterial.id
// 							? undefined
// 							: selectedObject.constructionMaterial.id,
// 					paintworkTypeId:
// 						selectedObject.paintworkType.id ===
// 						defaultSelectedObject.paintworkType.id
// 							? undefined
// 							: selectedObject.paintworkType.id,
// 					highTensileBoltsTypeId:
// 						selectedObject.highTensileBoltsType.id ===
// 						defaultSelectedObject.highTensileBoltsType.id
// 							? undefined
// 							: selectedObject.highTensileBoltsType.id,
// 				}
// 				if (!Object.values(object).some((x) => x !== undefined)) {
// 					setErrMsg('Изменения осутствуют')
// 					setProcessIsRunning(false)
// 					return
// 				}
// 				await httpClient.patch(
// 					`/marks/${mark.id}/mark-operating-conditions`,
// 					object
// 				)
// 				history.push('/')
// 			} catch (e) {
// 				setErrMsg('Произошла ошибка')
// 				setProcessIsRunning(false)
// 			}
// 		} else {
// 			setProcessIsRunning(false)
// 		}
// 	}

// 	return selectedObject == null || mark == null ? null : (
// 		<div className="component-cnt flex-v-cent-h">
// 			<h1 className="text-centered">Общие условия эксплуатации</h1>
// 			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
// 				<Form.Group className="flex-cent-v">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="coeff"
// 						style={{ marginRight: '2.5em' }}
// 					>
// 						Коэф. надежности по ответственности
// 					</Form.Label>
// 					<Form.Control
// 						id="coeff"
// 						type="text"
// 						placeholder="Введите коэффициент надежности"
// 						autoComplete="off"
// 						defaultValue={
// 							isNaN(selectedObject.safetyCoeff)
// 								? ''
// 								: selectedObject.safetyCoeff
// 						}
// 						className="auto-width flex-grow"
// 						onBlur={onCoeffChange}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="mrg-top-2 flex-cent-v">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="aggressiveness"
// 						style={{ marginRight: '10.5em' }}
// 					>
// 						Агрессивность среды
// 					</Form.Label>
// 					<Select
// 						inputId="aggressiveness"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите агрессивность среды"
// 						noOptionsMessage={() =>
// 							'Агрессивность среды не найдена'
// 						}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onEnvAggresivnessSelect(
// 								(selectedOption as any)?.value
// 							)
// 						}
// 						value={
// 							selectedObject.envAggressiveness == null
// 								? null
// 								: {
// 										value:
// 											selectedObject.envAggressiveness.id,
// 										label:
// 											selectedObject.envAggressiveness
// 												.name,
// 								  }
// 						}
// 						options={optionsObject.envAggressiveness.map((a) => {
// 							return {
// 								value: a.id,
// 								label: a.name,
// 							}
// 						})}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="mrg-top-2 flex-cent-v">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="temp"
// 						style={{ marginRight: '1em' }}
// 					>
// 						Расчетная температура эксплуатации (°C)
// 					</Form.Label>
// 					<Form.Control
// 						id="temp"
// 						type="text"
// 						placeholder="Введите наименование"
// 						autoComplete="off"
// 						className="auto-width flex-grow"
// 						defaultValue={
// 							isNaN(selectedObject.temperature)
// 								? ''
// 								: selectedObject.temperature
// 						}
// 						onBlur={onTempChange}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="flex-cent-v mrg-top-2">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="operatingArea"
// 						style={{ marginRight: '11.65em' }}
// 					>
// 						Зона эксплуатации
// 					</Form.Label>
// 					<Select
// 						inputId="operatingArea"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите зону эксплуатации"
// 						noOptionsMessage={() => 'Зоны эксплуатации не найдены'}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onOperatingAreaSelect(
// 								(selectedOption as any)?.value
// 							)
// 						}
// 						value={
// 							selectedObject.operatingArea == null
// 								? null
// 								: {
// 										value: selectedObject.operatingArea.id,
// 										label:
// 											selectedObject.operatingArea.name,
// 								  }
// 						}
// 						options={optionsObject.operatingAreas.map((a) => {
// 							return {
// 								value: a.id,
// 								label: a.name,
// 							}
// 						})}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="flex-cent-v mrg-top-2">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="gasGroup"
// 						style={{ marginRight: '14.65em' }}
// 					>
// 						Группа газов
// 					</Form.Label>
// 					<Select
// 						inputId="gasGroup"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите группу газов"
// 						noOptionsMessage={() => 'Группы газов не найдены'}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onGasGroupSelect((selectedOption as any)?.value)
// 						}
// 						value={
// 							selectedObject.gasGroup == null
// 								? null
// 								: {
// 										value: selectedObject.gasGroup.id,
// 										label: selectedObject.gasGroup.name,
// 								  }
// 						}
// 						options={optionsObject.gasGroups.map((g) => {
// 							return {
// 								value: g.id,
// 								label: g.name,
// 							}
// 						})}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="flex-cent-v mrg-top-2">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="constructionMaterial"
// 						style={{ marginRight: '9.65em' }}
// 					>
// 						Материал конструкций
// 					</Form.Label>
// 					<Select
// 						inputId="constructionMaterial"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите материал конструкций"
// 						noOptionsMessage={() =>
// 							'Материалы конструкций не найдены'
// 						}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onConstructionMaterialSelect(
// 								(selectedOption as any)?.value
// 							)
// 						}
// 						value={
// 							selectedObject.constructionMaterial == null
// 								? null
// 								: {
// 										value:
// 											selectedObject.constructionMaterial
// 												.id,
// 										label:
// 											selectedObject.constructionMaterial
// 												.name,
// 								  }
// 						}
// 						options={optionsObject.constructionMaterials.map(
// 							(m) => {
// 								return {
// 									value: m.id,
// 									label: m.name,
// 								}
// 							}
// 						)}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="flex-cent-v mrg-top-2">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="paintworkType"
// 						style={{ marginRight: '5.75em' }}
// 					>
// 						Тип лакокрасочного материала
// 					</Form.Label>
// 					<Select
// 						inputId="paintworkType"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите тип"
// 						noOptionsMessage={() => 'Типы не найдены'}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onPaintworkTypeSelect(
// 								(selectedOption as any)?.value
// 							)
// 						}
// 						value={
// 							selectedObject.paintworkType == null
// 								? null
// 								: {
// 										value: selectedObject.paintworkType.id,
// 										label:
// 											selectedObject.paintworkType.name,
// 								  }
// 						}
// 						options={optionsObject.paintworkTypes.map((t) => {
// 							return {
// 								value: t.id,
// 								label: t.name,
// 							}
// 						})}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<Form.Group className="flex-cent-v mrg-top-2 no-bot-mrg">
// 					<Form.Label
// 						className="no-bot-mrg"
// 						htmlFor="highTensileBoltsType"
// 						style={{ marginRight: '9.65em' }}
// 					>
// 						Высокопрочные болты
// 					</Form.Label>
// 					<Select
// 						inputId="highTensileBoltsType"
// 						maxMenuHeight={250}
// 						isClearable={true}
// 						isSearchable={true}
// 						placeholder="Выберите высокопрочные болты"
// 						noOptionsMessage={() =>
// 							'Высокопрочные болты не найдены'
// 						}
// 						className="auto-width flex-grow"
// 						onChange={(selectedOption) =>
// 							onHighTensileBoltsTypeSelect(
// 								(selectedOption as any)?.value
// 							)
// 						}
// 						value={
// 							selectedObject.highTensileBoltsType == null
// 								? null
// 								: {
// 										value:
// 											selectedObject.highTensileBoltsType
// 												.id,
// 										label:
// 											selectedObject.highTensileBoltsType
// 												.name,
// 								  }
// 						}
// 						options={optionsObject.highTensileBoltsTypes.map(
// 							(t) => {
// 								return {
// 									value: t.id,
// 									label: t.name,
// 								}
// 							}
// 						)}
// 						styles={reactSelectStyle}
// 					/>
// 				</Form.Group>

// 				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

// 				<Button
// 					variant="secondary"
// 					className="btn-mrg-top-2 full-width"
// 					onClick={
// 						isCreateMode ? onCreateButtonClick : onChangeButtonClick
// 					}
// 					disabled={processIsRunning}
// 				>
// 					{'Сохранить изменения'}
// 				</Button>
// 			</div>
// 		</div>
// 	)
// }

// export default DefaultValuesData
