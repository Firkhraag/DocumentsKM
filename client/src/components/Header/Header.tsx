import React from 'react'
import { Link } from 'react-router-dom'
import Profile from '../Svg/Profile'
import './Header.css'

const Header = () => {

    const getSelectedMark = () => {
        const markStr = localStorage.getItem('selectedMark')
        if (!markStr) {
            return '-'
        }
        const mark = JSON.parse(markStr)
        const now = new Date()
        if (now.getTime() > mark.expiry) {
            localStorage.removeItem('selectedMark')
            return '-'
        }
        return mark.name
    }

	return (
        <div>
            Выбранная марка: {getSelectedMark()}
            <Profile />
        </div>
		// <div className="header sticky white-bg space-between">
		// 	<ul className="flex-cent-v semibold">
		// 		<Link to="/">
		// 			<li className="pointer border-radius">Главная</li>
		// 		</Link>
		// 		<Link to="/">
		// 			<li className="pointer border-radius">Выйти</li>
		// 		</Link>
		// 	</ul>

		// 	<ul className="flex-cent-v semibold">
		// 		<Link to="/login">
		// 			<li className="pointer border-radius">Выбранная марка</li>
		// 		</Link>
		// 	</ul>
		// </div>
	)
}

export default Header
