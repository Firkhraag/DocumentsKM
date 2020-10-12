import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import Profile from '../Svg/Profile'
import { useMark } from '../../store/MarkStore'
import Drawer from './Drawer/Drawer'
import './Header.css'

const Header = () => {
	const mark = useMark()

	const [isDrawerShown, setDrawerShown] = useState(false)

	useEffect(() => {
		const clickHandler = (e: MouseEvent) => {
			if ((e.target as HTMLElement).id != 'user-drawer') {
				setDrawerShown(false)
			}
		}
		document.body.addEventListener('click', clickHandler)
		return () => document.body.removeEventListener('click', clickHandler)
	}, [])

	return (
		<div className="space-between-cent-v header white-bg">
			<Link to="/" className="pointer bold">
				Главная
			</Link>
			<Link to="/mark-select" className="pointer bold">
				{mark == null
					? '-'
					: `${mark.subnode.node.project.baseSeries}.${mark.subnode.node.code}.${mark.subnode.code}-${mark.code}`}
			</Link>
			<div
				className="profile-icon-cnt relative"
				onClick={() => setDrawerShown(true)}
			>
				<Drawer isShown={isDrawerShown} />
				<div className="pointer">
					<Profile />
				</div>
			</div>
		</div>
	)
}

export default Header
