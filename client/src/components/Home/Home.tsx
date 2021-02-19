// Global
import React from 'react'
import { Link } from 'react-router-dom'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
// Style
import './Home.css'

const Home = () => {
	const mark = useMark()

	const onConstructionDocumentDownloadButtonClick = async () => {
		try {
			const response = await httpClient.get(
				`/marks/${mark.id}/construction-document`,
				{
					responseType: 'blob',
				}
			)
			const url = window.URL.createObjectURL(new Blob([response.data]))
			const link = document.createElement('a')
			link.href = url
			link.setAttribute('download', `${mark.code}_ВМП.docx`)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
	}

	const onBoltDocumentDownloadButtonClick = async () => {
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
			link.setAttribute('download', `${mark.code}_ВБ.docx`)
			document.body.appendChild(link)
			link.click()
			link.remove()
		} catch (e) {
			console.log('Failed to download the file')
		}
	}

	return (
		<div>
			<h2 className="home-cnt-header text-centered">Данные</h2>
			<div className="home-cnt">
				<Link to="/marks">
					<Button variant="outline-secondary">
						Выбор / создание марки
					</Button>
				</Link>
				<Link to={mark != null ? `/marks/${mark.id}` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Данные марки
					</Button>
				</Link>
				<Link to="/approvals">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Согласования
					</Button>
				</Link>
				<Link to="/specifications">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Выпуски спецификаций
					</Button>
				</Link>
				<Link to="/sheets">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Листы основного комплекта
					</Button>
				</Link>
				<Link to="/developing-attached-docs">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Разрабатываемые прилагаемые документы
					</Button>
				</Link>
				<Link to="/other-attached-docs">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Прочие прилагаемые документы
					</Button>
				</Link>
				<Link to="/linked-docs">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Ссылочные документы
					</Button>
				</Link>
				<Link to="/operating-conditions">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Технические условия
					</Button>
				</Link>
				<Link to="/additional-work">
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Учет дополнительных работ
					</Button>
				</Link>
			</div>

			<h2 className="home-cnt-header text-centered">Документы</h2>
			<div className="home-cnt">
				<Link to={mark != null ? `/general-data` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Общие данные
					</Button>
				</Link>
				<Button
					variant="outline-secondary"
					disabled={mark == null ? true : false}
					onClick={onConstructionDocumentDownloadButtonClick}
				>
					Ведомость металлоконструкций
				</Button>
				<Button
					variant="outline-secondary"
					disabled={mark == null ? true : false}
					onClick={onBoltDocumentDownloadButtonClick}
				>
					Ведомость болтов
				</Button>
				<Link to={mark != null ? `/estimate-task` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Задание на смету
					</Button>
				</Link>
				<Link to={mark != null ? `/set-doc` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Комплект для расчета
					</Button>
				</Link>
				<Link to={mark != null ? `/registration-doc` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Лист регистрации проекта
					</Button>
				</Link>
				<Link to={mark != null ? `/metal-spec-doc` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Спецификация металла
					</Button>
				</Link>
			</div>
		</div>
	)
}

export default Home
