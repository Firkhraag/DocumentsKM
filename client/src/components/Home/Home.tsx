// Global
import React, { useState } from 'react'
import { useHistory, Link } from 'react-router-dom'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import Specification from '../../model/Specification'
import { useMark } from '../../store/MarkStore'
import { useUser } from '../../store/UserStore'
// Style
import './Home.css'

type HomeProps = {
	setSpecification: (s: Specification) => void
}

const Home = ({ setSpecification }: HomeProps) => {
	const mark = useMark()
	const user = useUser()
	const history = useHistory()

	const [processIsRunning, setProcessIsRunning] = useState(false)

	const onConstructionDocumentDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/users/${user.id}/marks/${mark.id}/construction-document`,
				{
					responseType: 'blob',
				}
			)
			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_ВМП.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
		setProcessIsRunning(false)
	}

	const onBoltDocumentDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/bolt-document`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_ВБ.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
		setProcessIsRunning(false)
	}

	const onSpecDocumentDownloadButtonClick = async () => {
		setProcessIsRunning(true)
		try {
			const response = await httpClient.get(
				`/users/${user.id}/marks/${mark.id}/spec-document`,
				{
					responseType: 'blob',
				}
			)

			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute(
				'download',
				`${mark.designation}_СМ.docx`
			)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
		setProcessIsRunning(false)
	}

	const goToCurrentSpecification = async () => {
		const specResponse = await httpClient.get(
			`/marks/${mark.id}/specifications/current`
		)
		setSpecification(specResponse.data)
		history.push(`/specifications/${specResponse.data.id}`)
	}

	return (
		<div>
			<h2 className="home-cnt-header text-centered">Данные</h2>
			<div className="home-cnt">
				<Link to="/marks">
					<Button
						variant="outline-secondary"
						disabled={processIsRunning}
					>
						Выбор / создание марки
					</Button>
				</Link>
				<Link to={mark != null ? `/marks/${mark.id}` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Данные марки
					</Button>
				</Link>
				<Link to="/approvals">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Согласования
					</Button>
				</Link>
				<Link to="/sheets">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Листы основного комплекта
					</Button>
				</Link>
				<Link to="/developing-attached-docs">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Разрабатываемые прилагаемые документы
					</Button>
				</Link>
				<Link to="/other-attached-docs">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Прочие прилагаемые документы
					</Button>
				</Link>
				<Link to="/linked-docs">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Ссылочные документы
					</Button>
				</Link>
				<Link to="/operating-conditions">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Технические условия
					</Button>
				</Link>
				<Link to="/additional-work">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Учет дополнительных работ
					</Button>
				</Link>
				<Button
					variant="outline-secondary"
					disabled={
						mark == null || processIsRunning ? true : false
					}
					onClick={goToCurrentSpecification}
				>
					Текущий выпуск спецификации
				</Button>
				<Link to="/specifications">
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Выпуски спецификаций
					</Button>
				</Link>
			</div>

			<h2 className="home-cnt-header text-centered">Документы</h2>
			<div className="home-cnt">
				<Link to={mark != null ? `/general-data` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Общие данные
					</Button>
				</Link>
				<Button
					variant="outline-secondary"
					disabled={mark == null || processIsRunning ? true : false}
					onClick={onSpecDocumentDownloadButtonClick}
				>
					Спецификация металла
				</Button>
				<Button
					variant="outline-secondary"
					disabled={mark == null || processIsRunning ? true : false}
					onClick={onConstructionDocumentDownloadButtonClick}
				>
					Ведомость металлоконструкций
				</Button>
				<Button
					variant="outline-secondary"
					disabled={mark == null || processIsRunning ? true : false}
					onClick={onBoltDocumentDownloadButtonClick}
				>
					Ведомость болтов
				</Button>
				<Link to={mark != null ? `/estimate-task` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Задание на смету
					</Button>
				</Link>
				<Link to={mark != null ? `/project-registration` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Лист регистрации проекта
					</Button>
				</Link>

				<Link to={mark != null ? `/estimation` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={
							mark == null || processIsRunning ? true : false
						}
					>
						Комплект для расчета
					</Button>
				</Link>
			</div>
		</div>
	)
}

export default Home
