import React from 'react'
// Bootstrap
import Button from 'react-bootstrap/Button'
import { usePopup } from '../../store/PopupStore'
import './Popup.css'

const Popup = () => {
    const popup = usePopup()

	return popup.isShown ? (
		<div className="div-container component-cnt-div white-bg popup shadow p-3 mb-5 rounded">
			<p className="text-centered bold">{popup.msg}</p>
			<div className="flex btns-mrg full-width">
				<Button
					variant="secondary"
					className="flex-grow"
					onClick={popup.onAccept}
				>
					Да
				</Button>
				<Button
					variant="secondary"
					className="flex-grow mrg-left"
					onClick={popup.onCancel}
				>
					Нет
				</Button>
			</div>
		</div>
	) : null
}

export default Popup
