// Global
import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { useHistory } from 'react-router-dom'
// Bootstrap
import { PersonFill } from 'react-bootstrap-icons'
// Util
import { useMark } from '../../store/MarkStore'
import Drawer from './Drawer/Drawer'
import { makeMarkName } from '../../util/make-name'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'
import './Header.css'

const Header = () => {
	const mark = useMark()
	const history = useHistory()
	const setPopup = useSetPopup()

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

	const onMainClick = () => {
		setPopup(defaultPopup)
		history.push('/')
	}

	return (
		<div className="space-between-cent-v header bg-white">
			<div onClick={onMainClick} className="pointer bold header-link-pad">
				Главная
			</div>
			<div onClick={onMainClick} className="pointer bold header-link-pad">
				{mark == null
					? '-'
					: makeMarkName(
							mark.subnode.node.project.baseSeries,
							mark.subnode.node.code,
							mark.subnode.code,
							mark.code
					  )}
			</div>
			<div id="user-cnt" className="profile-icon-cnt relative">
				<Drawer
					isShown={isDrawerShown}
					hide={() => setDrawerShown(false)}
				/>
				<PersonFill
					onClick={() => setDrawerShown(true)}
					size={36}
					className="pointer"
				/>
			</div>
		</div>
	)
}

export default Header
