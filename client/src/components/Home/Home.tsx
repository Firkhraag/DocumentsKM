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
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>Согласования</Button>
			</Link>
			<Link to="/specifications">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Выпуски спецификаций
				</Button>
			</Link>
			<Link to="/sheets">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>Листы основного комплекта</Button>
			</Link>
			<Link to="/attached-docs">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Прилагаемые документы
				</Button>
			</Link>
            <Link to="/linked-docs">
				<Button variant="outline-secondary" disabled={mark == null ? true : false}>
					Ссылочные документы
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
