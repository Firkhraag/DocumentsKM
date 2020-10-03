import React from 'react'
import { Link } from 'react-router-dom'
import Profile from '../Svg/Profile'
import { useMark } from '../../store/MarkStore'
import './Header.css'

const Header = () => {
	const mark = useMark()

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
			<div className="pointer profile-icon-cnt">
				<Profile />
			</div>
		</div>
	)
}

export default Header
