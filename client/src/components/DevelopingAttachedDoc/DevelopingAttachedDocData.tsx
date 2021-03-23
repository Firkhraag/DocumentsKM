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
import DocType from '../../model/DocType'
import { useMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'

type DevelopingAttachedDocDataProps = {
	developingAttachedDoc: Doc
	isCreateMode: boolean
}

const DevelopingAttachedDocData = ({
	developingAttachedDoc,
	isCreateMode,
}: DevelopingAttachedDocDataProps) => {
	const defaultOptionsObject = {
		types: [] as DocType[],
		employees: [] as Employee[],
	}

	const history = useHistory()
	const mark = useMark()
	const user = useUser()

	const [selectedObject, setSelectedObject] = useState<Doc>(
		isCreateMode
			? {
					id: -1,
					num: 1,
					numOfPages: 1,
					form: 0.125,
					name: '',
					type: null,
					creator: null,
					inspector: null,
					normContr: null,
					releaseNum: 0,
					note: '',
			  }
			: developingAttachedDoc
	)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null) {
				history.push('/developing-attached-docs')
				return
			}
			const fetchData = async () => {
				try {
					const docTypesResponse = await httpClient.get(
						`/doc-types/attached`
					)
					const employeesResponse = await httpClient.get(
						`/departments/${mark.department.id}/employees`
					)
					setOptionsObject({
						employees: employeesResponse.data,
						types: docTypesResponse.data,
					})
                    if (isCreateMode) {
                        const defaultValuesResponse = await httpClient.get(
                            `/users/${user.id}/default-values`
                        )
                        setSelectedObject({
                            ...selectedObject,
                            creator: defaultValuesResponse.data.creator,
                            inspector: defaultValuesResponse.data.inspector,
                            normContr: defaultValuesResponse.data.normContr,
                        })
                    }
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onCodeSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				type: null,
			})
		}
		const v = getFromOptions(id, optionsObject.types, selectedObject.type)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				type: v,
			})
		}
	}

	const onNameChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			name: event.currentTarget.value,
		})
	}

	const onNumOfPagesChange = (event: React.FormEvent<HTMLInputElement>) => {
		const v = parseInt(event.currentTarget.value)
		setSelectedObject({
			...selectedObject,
			numOfPages: v,
			form: Math.round(v * 0.125 * 1000) / 1000,
		})
	}

	const onFormatChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSelectedObject({
			...selectedObject,
			form: parseFloat(event.currentTarget.value),
		})
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
		if (selectedObject.type == null) {
			setErrMsg('Пожалуйста, выберите шифр прилагаемого документа')
			return false
		}
		if (selectedObject.name === '') {
			setErrMsg('Пожалуйста, введите наименование прилагаемого документа')
			return false
		}
		if (isNaN(selectedObject.form)) {
			setErrMsg('Пожалуйста, введите формат прилагаемого документа')
			return false
		}
		if (selectedObject.form < 0 || selectedObject.form > 1000000) {
			setErrMsg('Пожалуйста, введите правильный формат')
			return false
		}
		if (
			!isNaN(selectedObject.numOfPages) &&
			(selectedObject.numOfPages < 0 ||
				selectedObject.numOfPages > 1000000)
		) {
			setErrMsg('Пожалуйста, введите правильное число листов')
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
				await httpClient.post(`/marks/${mark.id}/docs`, {
					typeId: selectedObject.type.id,
					name: selectedObject.name,
					numOfPages: selectedObject.numOfPages,
					form: selectedObject.form,
					creatorId: selectedObject.creator.id,
					inspectorId: selectedObject.inspector?.id,
					normContrId: selectedObject.normContr?.id,
					note: selectedObject.note,
				})
				history.push('/developing-attached-docs')
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
					typeId:
						selectedObject.type.id === developingAttachedDoc.type.id
							? undefined
							: selectedObject.type.id,
					name:
						selectedObject.name === developingAttachedDoc.name
							? undefined
							: selectedObject.name,
					numOfPages:
						selectedObject.numOfPages ===
						developingAttachedDoc.numOfPages
							? undefined
							: selectedObject.numOfPages,
					form:
						selectedObject.form === developingAttachedDoc.form
							? undefined
							: selectedObject.form,
					creatorId:
						selectedObject.creator.id ===
						developingAttachedDoc.creator.id
							? undefined
							: selectedObject.creator.id,
					inspectorId: getNullableFieldValue(
						selectedObject.inspector,
						developingAttachedDoc.inspector
					),
					normContrId: getNullableFieldValue(
						selectedObject.normContr,
						developingAttachedDoc.normContr
					),
					note:
						selectedObject.note === developingAttachedDoc.note
							? undefined
							: selectedObject.note,
				}
				if (!Object.values(object).some((x) => x !== undefined)) {
					setErrMsg('Изменения осутствуют')
					setProcessIsRunning(false)
					return
				}
				await httpClient.patch(`/docs/${selectedObject.id}`, object)
				history.push('/developing-attached-docs')
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
					? 'Создание прилагаемого документа'
					: 'Данные прилагаемого документа'}
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group className="space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="code"
						style={{ marginRight: '1em' }}
					>
						Шифр документа
					</Form.Label>
					<Select
						inputId="code"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор шифр документа"
						noOptionsMessage={() => 'Шифры не найдены'}
						className="doc-input-width"
						onChange={(selectedOption) =>
							onCodeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.type == null
								? null
								: {
										value: selectedObject.type.id,
										label: selectedObject.type.code,
								  }
						}
						options={optionsObject.types.map((t) => {
							return {
								value: t.id,
								label: t.code,
							}
						})}
						styles={reactSelectStyle}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2">
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

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="numOfPages"
					>
						Число листов
					</Form.Label>
					<Form.Control
						id="numOfPages"
						type="text"
						placeholder="Введите число листов"
						autoComplete="off"
						className="doc-input-width"
						defaultValue={
							isNaN(selectedObject.numOfPages)
								? ''
								: selectedObject.numOfPages
						}
						onBlur={onNumOfPagesChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 space-between-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="format"
					>
						Формат
					</Form.Label>
					<Form.Control
						id="format"
						type="text"
						placeholder="Введите формат"
						autoComplete="off"
						className="doc-input-width"
						value={
							isNaN(selectedObject.form)
								? ''
								: selectedObject.form
						}
						onChange={onFormatChange}
					/>
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
					disabled={processIsRunning}
				>
					{isCreateMode
						? 'Создать разрабатываемый прилагаемый документ'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default DevelopingAttachedDocData
