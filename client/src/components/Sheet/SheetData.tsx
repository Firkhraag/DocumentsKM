// Global
import React, { useState, useEffect } from 'react'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { X } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import Doc from '../../model/Doc'
import SheetName from '../../model/SheetName'
import { useMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

type ISheetDataProps = {
	sheet: Doc
	isCreateMode: boolean
	index: number
}

type SheetDataProps = {
	sheetData: ISheetDataProps
	setSheetData: (d: ISheetDataProps) => void
	sheets: Doc[]
	setSheets: (a: Doc[]) => void
}

const SheetData = ({ sheetData, setSheetData, sheets, setSheets }: SheetDataProps) => {
	// Id листа основного комплекта из справочника
	const basicSheetDocTypeId = 1

	const formats = [0.25, 0.5, 0.75, 1, 1.25]

	const defaultOptionsObject = {
		sheetNames: [] as SheetName[],
		employees: [] as Employee[],
	}

	const mark = useMark()
	const user = useUser()

	const defaultSelectedObject = {
		id: -1,
		num: 1,
		form: 1.0,
		name: '',
		type: null,
		creator: null,
		inspector: null,
		normContr: null,
		releaseNum: 0,
		numOfPages: 0,
		note: '',
    } as Doc

	const [selectedObject, setSelectedObject] = useState<Doc>(null)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	const [fetched, setFetched] = useState(false)

	useEffect(() => {
		if (!fetched || sheetData.sheet == null) {
			const fetchData = async () => {
				try {
					const sheetNamesResponse = await httpClient.get(
						`/sheet-names`
					)
					const employeesResponse = await httpClient.get(
						`/departments/${mark.department.id}/employees`
					)
					setOptionsObject({
						sheetNames: sheetNamesResponse.data,
						employees: employeesResponse.data,
					})
					if (sheetData.isCreateMode && sheetData.sheet == null) {
						const valuesResponse = await httpClient.get(
							`/users/${user.id}/default-values`
						)
						setSelectedObject({
							...defaultSelectedObject,
							creator: valuesResponse.data.creator,
							inspector: valuesResponse.data.inspector,
							normContr: valuesResponse.data.normContr,
						})
					}
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
			setFetched(true)
		}
		if (sheetData.sheet != null) {
			setSelectedObject({
				...defaultSelectedObject,
				...sheetData.sheet
			})
		}
	}, [sheetData])

	const onNameSelect = (id: number) => {
		let v = null
		for (let el of optionsObject.sheetNames) {
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

	const onFormatSelect = (value: number) => {
		setSelectedObject({
			...selectedObject,
			form: value,
		})
	}

	const onNameChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onFormatChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			form: parseFloat(event.currentTarget.value),
		})
	}

	const onNoteChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	const onCreatorSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				creator: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.creator
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				creator: v,
			})
		}
	}

	const onInspectorSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				inspector: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.inspector
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				inspector: v,
			})
		}
	}

	const onNormControllerSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				normContr: null,
			})
		}
		const v = getFromOptions(
			id,
			optionsObject.employees,
			selectedObject.normContr
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				normContr: v,
			})
		}
	}

	const checkIfValid = () => {
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование листа')
			return false
		}
		if (isNaN(selectedObject.form)) {
			setErrMsg('Пожалуйста, введите формат листа')
			return false
		}
		if (selectedObject.form < 0 || selectedObject.form > 1000000) {
			setErrMsg('Пожалуйста, введите правильный формат')
			return false
		}
		if (selectedObject.creator == null) {
			setErrMsg('Пожалуйста, выберите разработчика')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		setProcessIsRunning(true)
		if (checkIfValid()) {
			try {
				const idResponse = await httpClient.post(`/marks/${mark.id}/docs`, {
					name: selectedObject.name,
					form: selectedObject.form,
					typeId: basicSheetDocTypeId,
					creatorId: selectedObject.creator.id,
					inspectorId: selectedObject.inspector?.id,
					normContrId: selectedObject.normContr?.id,
					note: selectedObject.note,
				})
				const arr = [...sheets]
				arr.push({
					...selectedObject,
					id: idResponse.data.id,
				})
				setSheets(arr)
				setSheetData({
					sheet: null,
					isCreateMode: false,
					index: -1,
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
					name:
						selectedObject.name === sheetData.sheet.name
							? undefined
							: selectedObject.name,
					form:
						selectedObject.form === sheetData.sheet.form
							? undefined
							: selectedObject.form,
					creatorId:
						selectedObject.creator.id === sheetData.sheet.creator.id
							? undefined
							: selectedObject.creator.id,
					inspectorId: getNullableFieldValue(
						selectedObject.inspector,
						sheetData.sheet.inspector
					),
					normContrId: getNullableFieldValue(
						selectedObject.normContr,
						sheetData.sheet.normContr
					),
					note:
						(selectedObject.note === sheetData.sheet.note) || (selectedObject.note === '' && sheetData.sheet.note == null)
							? undefined
							: selectedObject.note,
				}
				await httpClient.patch(`/docs/${selectedObject.id}`, object)

				const arr = []
				for (const v of sheets) {
					if (v.id == selectedObject.id) {
						arr.push(selectedObject)
						continue
					}
					arr.push(v)
				}
				setSheets(arr)
				setSheetData({
					sheet: null,
					isCreateMode: false,
					index: -1,
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
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div relative">
				<div className="pointer absolute"
					style={{top: 5, right: 8}}
					onClick={() => setSheetData({
						sheet: null,
						isCreateMode: false,
						index: -1,
					})}
				>
					<X color="#666" size={33} />
				</div>
				{sheetData.isCreateMode ? null :
					<div className="absolute bold" style={{top: -25, left: 0, color: "#666"}}>
						{sheetData.index}
					</div>
				}
				<Form.Group>
					<Form.Label htmlFor="name">Наименование</Form.Label>
					<Form.Control
						id="name"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Введите наименование"
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
					options={optionsObject.sheetNames.map((s) => {
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
						htmlFor="format"
					>
						Формат
					</Form.Label>
					<div className="flex doc-input-width">
						<Form.Control
							id="format"
							type="text"
							placeholder="Введите формат"
							// className="doc-input-width"
							style={{marginRight: 10, width: 100}}
							autoComplete="off"
							value={
								isNaN(selectedObject.form)
									? ''
									: selectedObject.form
							}
							onChange={onFormatChange}
						/>
						<Select
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Типовые форматы"
							noOptionsMessage={() => 'Формат не найден'}
							onChange={(selectedOption) =>
								onFormatSelect((selectedOption as any)?.value)
							}
							className="flex-grow"
							value={null}
							options={formats.map((f) => {
								return {
									value: f,
									label: f,
								}
							})}
							styles={reactSelectStyle}
						/>
					</div>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="creator"
					>
						Разработал
					</Form.Label>
					<Select
						inputId="creator"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор разработчика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="doc-input-width"
						onChange={(selectedOption) =>
							onCreatorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.creator == null
								? null
								: {
										value: selectedObject.creator.id,
										label: selectedObject.creator.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="inspector"
					>
						Проверил
					</Form.Label>
					<Select
						inputId="inspector"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор проверщика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="doc-input-width"
						onChange={(selectedOption) =>
							onInspectorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.inspector == null
								? null
								: {
										value: selectedObject.inspector.id,
										label: selectedObject.inspector.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="normContr"
					>
						Нормоконтроль
					</Form.Label>
					<Select
						inputId="normContr"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор нормоконтролера"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="doc-input-width"
						onChange={(selectedOption) =>
							onNormControllerSelect(
								(selectedOption as any)?.value
							)
						}
						value={
							selectedObject.normContr == null
								? null
								: {
										value: selectedObject.normContr.id,
										label: selectedObject.normContr.fullname,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.fullname,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2" style={{ marginBottom: 0 }}>
					<Form.Label htmlFor="note">Примечание</Form.Label>
					<Form.Control
						id="note"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Не введено"
						value={selectedObject.note}
						onChange={onNoteChange}
					/>
				</Form.Group>

				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={
						sheetData.isCreateMode ? onCreateButtonClick : onChangeButtonClick
					}
					disabled={processIsRunning || (!sheetData.isCreateMode && !Object.values({
						name:
							selectedObject.name === sheetData.sheet.name
								? undefined
								: selectedObject.name,
						form:
							selectedObject.form === sheetData.sheet.form
								? undefined
								: selectedObject.form,
						creatorId:
							selectedObject.creator == null || selectedObject.creator.id === sheetData.sheet.creator.id
								? undefined
								: selectedObject.creator.id,
						inspectorId: getNullableFieldValue(
							selectedObject.inspector,
							sheetData.sheet.inspector
						),
						normContrId: getNullableFieldValue(
							selectedObject.normContr,
							sheetData.sheet.normContr
						),
						note:
							(selectedObject.note === sheetData.sheet.note) || (selectedObject.note === '' && sheetData.sheet.note == null)
								? undefined
								: selectedObject.note,
					}).some((x) => x !== undefined))}
				>
					{sheetData.isCreateMode
						? 'Создать лист основного комплекта'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default SheetData
