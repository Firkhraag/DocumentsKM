// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import EstimateTask from '../../model/EstimateTask'
import Department from '../../model/Department'
import Employee from '../../model/Employee'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import getFromOptions from '../../util/get-from-options'
import getNullableFieldValue from '../../util/get-field-value'
import { reactSelectStyle } from '../../util/react-select-style'
import { makeMarkName } from '../../util/make-name'

const ProjectRegistration = () => {
	const history = useHistory()
	const mark = useMark()

    const [defaultDate, setDefaultData] = useState<string>('')
	// const [date, setData] = useState<string>(new Date(Date.now()).toLocaleString())
	const [date, setData] = useState<string>('')
    const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
                const fullDateString = new Date(Date.now()).toLocaleString()
                const d = fullDateString.split(',')[0]
                setData(d.replace(/\//g, "."))
				// try {
				// 	const departmentsResponse = await httpClient.get(
				// 		'/departments'
				// 	)
				// 	setOptionsObject({
				// 		departments: departmentsResponse.data,
				// 		employees: optionsObject.employees,
				// 	})
				// } catch (e) {
				// 	console.log('Failed to fetch the data')
				// }

				// try {
				// 	const estimateTaskResponse = await httpClient.get(
				// 		`/marks/${mark.id}/estimate-task`
				// 	)
				// 	setSelectedObject(estimateTaskResponse.data)
                //     setSelectedDepartment(estimateTaskResponse.data.approvalEmployee?.department)
				// 	setDefaultSelectedObject(estimateTaskResponse.data)
				// } catch (e) {
				// 	console.log('Failed to fetch the data')
				// }
			}
			fetchData()
		}
	}, [mark])

	const onDateChange = (event: React.FormEvent<HTMLInputElement>) => {
		setData(event.currentTarget.value)
	}

	const checkIfValid = () => {
		// if (selectedObject.taskText == null) {
		// 	setErrMsg('Пожалуйста, введите текст задания')
		// 	return false
		// }
		return true
	}

	const onChangeButtonClick = async () => {
		// if (checkIfValid()) {
		// 	try {
		// 		const object = {
		// 			taskText:
		// 				selectedObject.taskText ===
		// 				defaultSelectedObject.taskText
		// 					? undefined
		// 					: selectedObject.taskText,
		// 			additionalText:
		// 				selectedObject.additionalText ===
		// 				defaultSelectedObject.additionalText
		// 					? undefined
		// 					: selectedObject.additionalText,
		// 			approvalEmployeeId: getNullableFieldValue(
		// 				selectedObject.approvalEmployee,
		// 				defaultSelectedObject.approvalEmployee
		// 			),
		// 		}
		// 		if (!Object.values(object).some((x) => x !== undefined)) {
		// 			setErrMsg('Изменения осутствуют')
		// 			return
		// 		}
		// 		await httpClient.patch(
		// 			`/marks/${mark.id}/estimate-task`,
		// 			object
		// 		)
        //         history.push('/')
		// 	} catch (e) {
		// 		setErrMsg('Произошла ошибка')
		// 	}
		// }
	}

	const onDownloadButtonClick = async () => {
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/project-reg`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute('download', `${makeMarkName(
                mark.subnode.node.project.baseSeries,
                mark.subnode.node.code,
                mark.subnode.code,
                mark.code
          )}_ЛР.docx`)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
            // if (e.response != null && e.response.status === 404) {
            //     setErrMsg('Пожалуйста, заполните условия эскплуатации у марки')
            //     return
            // }
			setErrMsg('Произошла ошибка')
		}
	}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Лист регистрации проекта</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
                {/* <Form.Group className="mrg-top-2 flex-cent-v"> */}
                <Form.Group className="no-bot-mrg">
					<Form.Label
						// className="no-bot-mrg"
						htmlFor="date"
						// style={{ marginRight: '1em' }}
					>
						Дата выдачи проекта
					</Form.Label>
					<Form.Control
						id="date"
						type="text"
						placeholder="Введите дату выдачи проекта"
						autoComplete="off"
						// className="auto-width flex-grow"
						defaultValue={date}
						onBlur={onDateChange}
					/>
				</Form.Group>
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
                {/* <div className="flex">
                <Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
					onClick={onChangeButtonClick}
				>
					Сохранить изменения
				</Button>
				<Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
                    style={{ marginLeft: 10 }}
					onClick={onDownloadButtonClick}
				>
					Скачать документ
				</Button>
                </div> */}
				{/* <Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
					onClick={onChangeButtonClick}
				>
					Сохранить изменения
				</Button>
				<Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
					onClick={onDownloadButtonClick}
				>
					Скачать документ
				</Button> */}
                <Button
					variant="secondary"
					className="full-width btn-mrg-top-2"
					onClick={onDownloadButtonClick}
				>
					Скачать документ
				</Button>
			</div>
		</div>
	)
}

export default ProjectRegistration
