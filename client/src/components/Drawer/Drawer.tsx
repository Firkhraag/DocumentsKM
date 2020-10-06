import React from 'react'
import { useSpring, animated } from 'react-spring'
import { useUser, useAuthMethods } from '../../store/UserStore'
import './Drawer.css'

type DrawerProps = {
	closeButtonClick: () => void
	isShown: boolean
}

const Drawer = ({ closeButtonClick, isShown }: DrawerProps) => {
	const user = useUser()
    const authMethods = useAuthMethods()

	let drawerClasses = 'side-drawer white-bg absolute border-radius'
	// if (isShown) {
    //     drawerClasses = 'side-drawer white-bg open'
    // }
    
    const propsSpringOpen = useSpring({
        from: { opacity: 0 as any, transform: 'scale(0)' as any },
        to: { opacity: isShown ? 1 : 0 as any, transform: isShown ? 'scale(1)' : 'scale(0)' as any},
        config: {
            duration: 200,
        }
    });

	return (
		<animated.div className={drawerClasses} style={propsSpringOpen} id="user-drawer">
			<div className="text-centered bold">{user}</div>
			<button onClick={authMethods.logout} className="logout-btn input-border-radius pointer">Выйти</button>
		</animated.div>
	)
}

export default Drawer
