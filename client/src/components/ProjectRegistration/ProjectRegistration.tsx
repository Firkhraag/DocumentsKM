// Global
import React, { useState, useEffect } from 'react'
import moment from 'moment'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'
import { makeMarkName } from '../../util/make-name'

const ProjectRegistration = () => {
	const history = useHistory()
	const mark = useMark()

	const [defaultDate, setDefaultDate] = useState<string>('')
	const [date, setDate] = useState<string>('')
	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const issueDateResponse = await httpClient.get(
						`/marks/${mark.id}/issue-date`
					)
					var issueDate = issueDateResponse.data.issueDate
					if (issueDate == null) {
						const fullDateString = new Date(
							Date.now()
						).toLocaleString()
						const d = fullDateString
							.split(',')[0]
							.replace(/\//g, '.')
						setDate(d)
						setDefaultDate(d)
					} else {
                        const d = new Date(issueDate)
                            .toLocaleDateString()
                            .replace(/\//g, '.')
						setDate(d)
                        setDefaultDate(d)
					}
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onDateChange = async (event: React.FormEvent<HTMLInputElement>) => {
		setDate(event.currentTarget.value)
	}

	const checkIfValid = () => {
		return moment(date, 'DD.MM.YYYY', true).isValid()
	}

	const onDownloadButtonClick = async () => {
		if (checkIfValid()) {
			try {
                // console.log({
                //     issueDate: date === defaultDate ? undefined : moment(date, 'DD.MM.YYYY').toDate(),
                // })
				const response = await httpClient.post(
					`/marks/${mark.id}/project-reg`,
					{
						issueDate: date === defaultDate ? undefined : moment(date, 'DD.MM.YYYY').toDate(),
					},
					{
						responseType: 'blob',
					}
				)

				const url = window.URL.createObjectURL(
					new Blob([response.data])
				)
				const link = document.createElement('a')
				link.href = url
				link.setAttribute(
					'download',
					`${makeMarkName(
						mark.subnode.node.project.baseSeries,
						mark.subnode.node.code,
						mark.subnode.code,
						mark.code
					)}_ЛР.docx`
				)
				document.body.appendChild(link)
				link.click()
				link.remove()
				history.push('/')
			} catch (e) {
				setErrMsg('Произошла ошибка')
                console.log(e)
			}
		} else {
			setErrMsg('Пожалуйста, введите дату в формате ДД.ММ.ГГГГ')
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
