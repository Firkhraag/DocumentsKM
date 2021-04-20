// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import ErrorMsg from '../ErrorMsg/ErrorMsg'
import { useMark } from '../../store/MarkStore'

const EstimationDocument = () => {
	const history = useHistory()
	const mark = useMark()

	const [numOfPages, setNumOfPages] = useState<number>(2)

	const [processIsRunning, setProcessIsRunning] = useState(false)
	const [errMsg, setErrMsg] = useState('')

	// useEffect(() => {
	// 	if (mark != null && mark.id != null) {
	// 		const fetchData = async () => {
	// 			try {
	// 				const issueDateResponse = await httpClient.get(
	// 					`/marks/${mark.id}/issue-date`
	// 				)
	// 				var issueDate = issueDateResponse.data.issueDate
	// 				if (issueDate == null) {
	// 					const fullDateString = new Date(
	// 						Date.now()
	// 					).toLocaleString()
	// 					const d = fullDateString
	// 						.split(',')[0]
	// 						.replace(/\//g, '.')
	// 					setDate(d)
	// 					setDefaultDate(d)
	// 				} else {
	// 					const d = new Date(issueDate)
	// 						.toLocaleDateString()
	// 						.replace(/\//g, '.')
	// 					setDate(d)
	// 					setDefaultDate(d)
	// 				}
	// 			} catch (e) {
	// 				console.log('Failed to fetch the data')
	// 			}
	// 		}
	// 		fetchData()
	// 	}
	// }, [mark])

	const onNumOfPagesChange = async (event: React.FormEvent<HTMLInputElement>) => {
		setNumOfPages(parseInt(event.currentTarget.value))
	}

	const onTitleDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/estimation-document-title`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_РР-обл_тит.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
            history.push('/')
		} catch (e) {
			console.log('Failed to download the file')
		    setProcessIsRunning(false)
		}
	}

    const onPagesDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/estimation-document-pages/${numOfPages}`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_РР-листы.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
            history.push('/')
		} catch (e) {
			console.log('Failed to download the file')
		    setProcessIsRunning(false)
		}
	}

	return mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Комплект для расчета</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
				<Form.Group className="no-bot-mrg">
					<Form.Label htmlFor="date">Число страниц</Form.Label>
					<Form.Control
						id="date"
						type="text"
						placeholder="Введите число страниц"
						autoComplete="off"
						defaultValue={numOfPages}
						onBlur={onNumOfPagesChange}
					/>
				</Form.Group>
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

                <div className="flex btn-mrg-top-2">
					<Button
						variant="secondary"
						className="full-width"
						onClick={onTitleDownloadButtonClick}
						disabled={processIsRunning}
					>
						Скачать титульный
					</Button>
					<Button
						variant="secondary"
						className="full-width mrg-left"
						onClick={onPagesDownloadButtonClick}
						disabled={processIsRunning}
					>
						Скачать листы
					</Button>
				</div>
			</div>
		</div>
	)
}

export default EstimationDocument
