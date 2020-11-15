// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import Doc from '../../model/Doc'
import SheetName from '../../model/SheetName'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

type SheetDataProps = {
	sheet: Doc
	isCreateMode: boolean
}

const SheetData = ({ sheet, isCreateMode }: SheetDataProps) => {
	// Id листа основного комплекта из справочника
	const basicSheetDocTypeId = 1

	const defaultOptionsObject = {
		sheetNames: [] as SheetName[],
		employees: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<Doc>(
		isCreateMode
			? {
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
			  }
			: sheet
	)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (!isCreateMode && selectedObject == null) {
				history.push('/sheets')
				return
			}
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
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onNameSelect = async (id: number) => {
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

	const onNameChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onFormatChange = (event: React.FormEvent<HTMLInputElement>) => {
		try {
			const v = parseFloat(event.currentTarget.value)
			setSelectedObject({
				...selectedObject,
				form: v,
			})
		} catch (e) {
			setSelectedObject({
				...selectedObject,
				form: null,
			})
		}
	}

	const onNoteChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
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
		if (selectedObject.form == null) {
			setErrMsg('Пожалуйста, введите формат листа')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(`/marks/${mark.id}/docs`, {
					name: selectedObject.name,
					form: selectedObject.form,
					docTypeId: basicSheetDocTypeId,
					creatorId: selectedObject.creator?.id,
					inspectorId: selectedObject.inspector?.id,
					normContrId: selectedObject.normContr?.id,
					note: selectedObject.note,
				})
				history.push('/sheets')
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(`/docs/${selectedObject.id}`, {
					name:
						selectedObject.name === sheet.name
							? undefined
							: selectedObject.name,
					form:
						selectedObject.form === sheet.form
							? undefined
							: selectedObject.form,
					creatorId: getNullableFieldValue(
						selectedObject.creator,
						sheet.creator
					),
					inspectorId: getNullableFieldValue(
						selectedObject.inspector,
						sheet.inspector
					),
					normContrId: getNullableFieldValue(
						selectedObject.normContr,
						sheet.normContr
					),
					note:
						selectedObject.note === sheet.note
							? undefined
							: selectedObject.note,
				})
				history.push('/sheets')
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
					? 'Создание листа основного комплекта'
					: 'Данные листа основного комплекта'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
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
					styles={reactSelectstyle}
				/>

				<Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="format"
						style={{ marginRight: '5.6em' }}
					>
						Формат
					</Form.Label>
					<Form.Control
						id="format"
						type="text"
						placeholder="Введите формат"
						defaultValue={selectedObject.form}
						onBlur={onFormatChange}
					/>
				</Form.Group>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
                        style={{ marginRight: '3.9em' }}
                        htmlFor="creator"
					>
						Разработал
					</label>
					<Select
                        inputId="creator"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор разработчика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onCreatorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.creator == null
								? null
								: {
										value: selectedObject.creator.id,
										label: selectedObject.creator.name,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
                        style={{ marginRight: '4.5em' }}
                        htmlFor="inspector"
					>
						Проверил
					</label>
					<Select
                        inputId="inspector"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор проверщика"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onInspectorSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.inspector == null
								? null
								: {
										value: selectedObject.inspector.id,
										label: selectedObject.inspector.name,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<div className="flex-cent-v mrg-top-2">
					<label
						className="bold no-bot-mrg"
                        style={{ marginRight: '1em' }}
                        htmlFor="normContr"
					>
						Нормоконтролер
					</label>
					<Select
                        inputId="normContr"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор нормоконтролера"
						noOptionsMessage={() => 'Сотрудники не найдены'}
						// className="mrg-top"
						className="auto-width flex-grow"
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
										label: selectedObject.normContr.name,
								  }
						}
						options={optionsObject.employees.map((e) => {
							return {
								value: e.id,
								label: e.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</div>

				<Form.Group className="mrg-top-2" style={{ marginBottom: 0 }}>
					<Form.Label htmlFor="note">Примечание</Form.Label>
					<Form.Control
                        id="note"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Не введено"
						defaultValue={selectedObject.note}
						onBlur={onNoteChange}
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
						? 'Создать лист основного комплекта'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default SheetData
