// Global
import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { useHistory } from 'react-router-dom'
// Bootstrap
import { PersonFill } from 'react-bootstrap-icons'
import { Link45deg } from 'react-bootstrap-icons'
// Util
import { useMark } from '../../store/MarkStore'
import Drawer from './Drawer/Drawer'
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
			<div className="flex">
				<div className="bold header-link-pad">
					{mark == null
						? '-'
						: mark.designation}
				</div>
				{mark == null ? null : <div className="header-link-pad pointer" onClick={
					() => {
						navigator.clipboard.writeText(`${window.location.protocol}//${window.location.host}/marks/${mark.id}/set-current`)
						alert("Ссылка скопирована")
				}}>
					<Link45deg color="#333" size={18} style={{marginLeft: 5}} />
				</div>}
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
