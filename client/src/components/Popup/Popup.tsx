import React from 'react'
// Bootstrap
import Button from 'react-bootstrap/Button'
import './Popup.css'

export type IPopupObj = {
	isShown: boolean
	msg: string
	onAccept: () => void
	onCancel: () => void
}

export const defaultPopupObj = {
	isShown: false,
	msg: '',
	onAccept: null,
	onCancel: null,
} as IPopupObj

type PopupProps = {
	popupObj: IPopupObj
}

// const Popup = ({ isShown, msg, onAccept, onCancel }: PopupProps) => {
const Popup = ({ popupObj }: PopupProps) => {
	return popupObj.isShown ? (
		<div className="div-container component-cnt-div white-bg popup shadow p-3 mb-5 rounded">
			<p className="text-centered bold">{popupObj.msg}</p>
			<div className="flex btns-mrg full-width">
				<Button
					variant="secondary"
					className="flex-grow"
					onClick={popupObj.onAccept}
				>
					Да
				</Button>
				<Button
					variant="secondary"
					className="flex-grow mrg-left"
					onClick={popupObj.onCancel}
				>
					Нет
				</Button>
			</div>
		</div>
	) : null
}

export default Popup
