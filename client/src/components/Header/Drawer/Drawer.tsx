// Global
import React from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import { useUser, useAuthMethods } from '../../../store/UserStore'
import './Drawer.css'

type DrawerProps = {
	isShown: boolean
	hide: () => void
}

const Drawer = ({ isShown, hide }: DrawerProps) => {
	const history = useHistory()
	const user = useUser()
	const authMethods = useAuthMethods()

	return !isShown ? null : (
		<div className="side-drawer white-bg absolute border-radius shadow p-3 mb-5 bg-white rounded">
			<Button
				variant="secondary"
				onClick={() => {
					hide()
					history.push('/user/default-values')
				}}
				className="full-width btn-mrg-top"
			>
				По умолчанию
			</Button>
			<Button
				variant="secondary"
				onClick={authMethods.logout}
				className="full-width btn-mrg-top"
			>
				Выйти
			</Button>
		</div>
	)
}

export default Drawer
