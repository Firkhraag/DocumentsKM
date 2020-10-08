import React from 'react'
import { useSpring, animated } from 'react-spring'
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
			className="side-drawer white-bg absolute border-radius"
			style={propsSpringOpen}
			id="user-drawer"
		>
			<div className="text-centered bold">{user}</div>
			<button
				onClick={authMethods.logout}
				className="logout-btn input-border-radius pointer"
			>
				Выйти
			</button>
		</animated.div>
	)
}

export default Drawer
