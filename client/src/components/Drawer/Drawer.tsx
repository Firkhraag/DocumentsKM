import React from 'react'
import './Drawer.css'

type DrawerProps = {
	closeButtonClick: () => void
	isShown: boolean
}

const Drawer = ({ closeButtonClick, isShown }: DrawerProps) => {
	let drawerClasses = 'side-drawer white-bg sticky'
	if (isShown) {
		drawerClasses = 'side-drawer white-bg sticky open'
	}
	return (
		<div className={drawerClasses}>
			1
		</div>
	)
}

export default Drawer