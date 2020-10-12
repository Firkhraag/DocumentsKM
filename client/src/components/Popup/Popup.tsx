import React from 'react'
import { useSpring, animated, config } from 'react-spring'
import './Popup.css'

// type PopupProps = {
// 	isShown: boolean
// 	msg: string
//     onAccept: () => void
//     onCancel: ()  => void
// }

export type IPopupObj = {
    isShown: boolean
	msg: string
    onAccept: () => void
    onCancel: ()  => void
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
	const props = useSpring({
		config: config.stiff,
		from: { transform: 'scale(1.5)' as any },
		to: { transform: popupObj.isShown ? 'scale(1.0)' : ('scale(1.5)' as any) },
	})

	return popupObj.isShown ? (
		<animated.div
			className="container white-bg popup"
			style={props}
		>
			<p className="text-centered bold">{popupObj.msg}</p>
            <div className="flex full-width btns-mrg">
                <button onClick={popupObj.onAccept} className="bold pointer">Да</button>
                <button onClick={popupObj.onCancel} className="bold pointer">Нет</button>
            </div>
		</animated.div>
	) : null
}

export default Popup
