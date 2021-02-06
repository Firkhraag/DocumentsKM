// Global
import React from 'react'
import { Link } from 'react-router-dom'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import { useMark } from '../../store/MarkStore'
// Style
import './Home.css'

const Home = () => {
	const mark = useMark()

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
                <Link to={mark != null ? `/bolts-doc` : '/'}>
					<Button
						variant="outline-secondary"
						disabled={mark == null ? true : false}
					>
						Ведомость болтов
					</Button>
				</Link>
			</div>
		</div>
	)
}

export default Home
