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
		<div className="home-cnt">
			<Link to="/mark-select">
				<Button variant="outline-secondary">
					Выбор / создание марки
				</Button>
			</Link>
			<Link to="/mark-data">
				<Button
					variant="outline-secondary"
					disabled={mark == null ? true : false}
				>
					Данные марки
				</Button>
			</Link>
			<Link to="/mark-approval">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>Согласования</Button>
			</Link>
			<Link to="/specifications">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Выпуски спецификаций
				</Button>
			</Link>
			<Link to="/sheets">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>Листы</Button>
			</Link>
			<Link to="/documents">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Прилагаемые документы
				</Button>
			</Link>
			<Link to="/exploitation">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Условия эксплуатации
				</Button>
			</Link>
		</div>
	)
}

export default Home
