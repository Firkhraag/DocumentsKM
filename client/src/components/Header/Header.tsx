// Global
import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
// Bootstrap
import { PersonFill } from 'react-bootstrap-icons'
// Util
import { useMark } from '../../store/MarkStore'
import Drawer from './Drawer/Drawer'
import { makeMarkName } from '../../util/make-name'
import './Header.css'

const Header = () => {
	const mark = useMark()

	const [isDrawerShown, setDrawerShown] = useState(false)

	useEffect(() => {
		const clickHandler = (e: MouseEvent) => {
			if ((e.target as HTMLElement).closest('#user-cnt') == null) {
				setDrawerShown(false)
			}
		}
		document.body.addEventListener('click', clickHandler)
		return () => document.body.removeEventListener('click', clickHandler)
	}, [])

	return (
		<div className="space-between-cent-v header bg-white">
			<Link to="/" className="pointer bold header-link-pad">
				Главная
			</Link>
			<Link to="/mark-select" className="pointer bold header-link-pad">
				{mark == null
					? '-'
					: makeMarkName(
							mark.subnode.node.project.baseSeries,
							mark.subnode.node.code,
							mark.subnode.code,
							mark.code
					  )}
			</Link>
			<div
				id="user-cnt"
				className="profile-icon-cnt relative"
				onClick={() => setDrawerShown(true)}
			>
				<Drawer isShown={isDrawerShown} />
				<PersonFill size={36} className="pointer" />
			</div>
		</div>
	)
}

export default Header
