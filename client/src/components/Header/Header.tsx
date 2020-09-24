import React from 'react'
import { Link } from 'react-router-dom'
import './Header.css'

const Header = () => {
	return (
		<div className="header sticky white-bg space-between">
			<ul className="flex-cent-v semibold">
				<Link to="/">
					<li className="pointer border-radius">Главная</li>
				</Link>
				<Link to="/">
					<li className="pointer border-radius">Выйти</li>
				</Link>
			</ul>

			<ul className="flex-cent-v semibold">
				<Link to="/login">
					<li className="pointer border-radius">Выбранная марка</li>
				</Link>
			</ul>
		</div>
	)
}

export default Header
