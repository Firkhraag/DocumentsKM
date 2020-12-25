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
import AdditionalWork from '../../model/AdditionalWork'
import DocType from '../../model/DocType'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectstyle } from '../../util/react-select-style'

type AdditionalWorkDataProps = {
	additionalWork: AdditionalWork
	isCreateMode: boolean
}

const AdditionalWorkData = ({
	additionalWork,
	isCreateMode,
}: AdditionalWorkDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState<AdditionalWork>(
		isCreateMode
			? {
                id: 0,
                employee: null,
                valuation: 0,
                metalOrder: 0,
			  }
			: additionalWork
	)
	const [employees, setEmployees] = useState([] as Employee[])

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null) {
				history.push('/additional-work')
				return
			}
			const fetchData = async () => {
				try {
                    const employeesResponse = await httpClient.get(
						`/departments/${mark.department.id}/employees`
                    )
                    setEmployees(employeesResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
    }, [mark])
    
    const onEmployeeSelect = async (id: number) => {
		if (id == null) {
			setSelectedObject({
				...selectedObject,
				employee: null,
			})
		}
		const v = getFromOptions(
			id,
			employees,
			selectedObject.employee
		)
		if (v != null) {
			setSelectedObject({
				...selectedObject,
				employee: v,
			})
		}
	}
    
    const onValuationChange = (event: React.FormEvent<HTMLInputElement>) => {
        setSelectedObject({
			...selectedObject,
			valuation: parseInt(event.currentTarget.value),
		})
	}

	const onOrderChange = (event: React.FormEvent<HTMLInputElement>) => {
        setSelectedObject({
			...selectedObject,
			metalOrder: parseInt(event.currentTarget.value),
		})
	}

	const checkIfValid = () => {
        if (selectedObject.employee == null) {
			setErrMsg('Пожалуйста, выберите исполнителя')
			return false
		}
		if (isNaN(selectedObject.valuation)) {
			setErrMsg('Пожалуйста, введите расчет')
			return false
        }
        if (isNaN(selectedObject.metalOrder)) {
			setErrMsg('Пожалуйста, введите заказ металла')
			return false
		}
		return true
	}

	const onCreateButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.post(`/marks/${mark.id}/additional-work`, {
                    employeeId: selectedObject.employee.id,
                    valuation: selectedObject.valuation,
                    order: selectedObject.metalOrder,
				})
				history.push('/additional-work')
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	const onChangeButtonClick = async () => {
		if (checkIfValid()) {
			try {
				await httpClient.patch(`/marks/${mark.id}/additional-work/${selectedObject.id}`, {
                    employeeId: selectedObject.employee.id === additionalWork.employee.id
                        ? undefined
                        : selectedObject.employee.id,
                    valuation: selectedObject.valuation === additionalWork.valuation
                        ? undefined
                        : selectedObject.valuation,
                    order: selectedObject.metalOrder === additionalWork.metalOrder
                        ? undefined
                        : selectedObject.metalOrder,
				})
				history.push('/additional-work')
			} catch (e) {
				setErrMsg('Произошла ошибка')
				console.log('Error')
			}
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">
                Учет дополнительных проектных работ
			</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
                <Form.Group className="flex-cent-v">
                    <Form.Label
						className="no-bot-mrg"
						htmlFor="code"
						style={{ marginRight: '4.7em' }}
					>
						Исполнитель
					</Form.Label>
                    <Select
                        inputId="code"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выбор исполнителя"
						noOptionsMessage={() => 'Исполнители не найдены'}
						className="auto-width flex-grow"
						onChange={(selectedOption) =>
							onEmployeeSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.employee == null
								? null
								: {
										value: selectedObject.employee.id,
										label: selectedObject.employee.name,
								  }
						}
						options={employees.map((v) => {
							return {
								value: v.id,
								label: v.name,
							}
						})}
						styles={reactSelectstyle}
					/>
				</Form.Group>

                <Form.Group className="mrg-top-2 flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="numOfPages"
						style={{ marginRight: '3.7em' }}
					>
						Расчет, лист А4
					</Form.Label>
					<Form.Control
						id="numOfPages"
						type="text"
                        placeholder="Введите число листов"
                        className="auto-width flex-grow"
                        defaultValue={isNaN(selectedObject.valuation) ? '' : selectedObject.valuation}
						onBlur={onValuationChange}
					/>
				</Form.Group>

				<Form.Group className="mrg-top-2 flex-cent-v no-bot-mrg">
					<Form.Label
						className="no-bot-mrg"
						htmlFor="format"
						style={{ marginRight: '1em' }}
					>
						Заказ металла, строк
					</Form.Label>
					<Form.Control
						id="format"
						type="text"
                        placeholder="Введите число строк"
                        className="auto-width flex-grow"
						defaultValue={isNaN(selectedObject.metalOrder) ? '' : selectedObject.metalOrder}
						onBlur={onOrderChange}
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
						? 'Добавить исполнителя'
						: 'Сохранить изменения'}
				</Button>
			</div>
		</div>
	)
}

export default AdditionalWorkData
