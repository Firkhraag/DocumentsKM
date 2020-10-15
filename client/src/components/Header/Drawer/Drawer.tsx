// Global
import React from 'react'
import { useSpring, animated } from 'react-spring'
// Bootstrap
import Button from 'react-bootstrap/Button'
import { useUser, useAuthMethods } from '../../../store/UserStore'
import './Drawer.css'

type DrawerProps = {
	isShown: boolean
}

const Drawer = ({ isShown }: DrawerProps) => {
	const user = useUser()
	const authMethods = useAuthMethods()

	const propsSpringOpen = useSpring({
		from: { opacity: 0 as any, transform: 'scale(0)' as any },
		to: {
			opacity: isShown ? 1 : (0 as any),
			transform: isShown ? 'scale(1)' : ('scale(0)' as any),
		},
		config: {
			duration: 200,
		},
	})

	return (
		<animated.div
			className="side-drawer white-bg absolute border-radius shadow p-3 mb-5 bg-white rounded"
			style={propsSpringOpen}
			id="user-drawer"
		>
			<div className="text-centered bold">{user}</div>

			<Button variant="secondary" onClick={authMethods.logout} className="full-width btn-mrg-top">
				Выйти
			</Button>
		</animated.div>
	)
}

export default Drawer
