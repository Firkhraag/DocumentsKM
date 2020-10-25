// Global
import React from 'react'
// Bootstrap
import Button from 'react-bootstrap/Button'
// Util
import { useUser, useAuthMethods } from '../../../store/UserStore'
import './Drawer.css'

type DrawerProps = {
	isShown: boolean
}

const Drawer = ({ isShown }: DrawerProps) => {
	const user = useUser()
	const authMethods = useAuthMethods()

	return !isShown ? null : (
		<div className="side-drawer white-bg absolute border-radius shadow p-3 mb-5 bg-white rounded">
			<div className="text-centered bold">{user}</div>
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
