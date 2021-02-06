import React, { createContext, useContext, useState } from 'react'

type IPopup = {
	isShown: boolean
	msg: string
	onAccept: () => void
	onCancel: () => void
}

export const defaultPopup = {
	isShown: false,
	msg: '',
	onAccept: null,
	onCancel: null,
} as IPopup

const PopupContext = createContext<IPopup>(null)
const PopupDispatchContext = createContext(null)
export const usePopup = () => useContext(PopupContext)
export const useSetPopup = () => useContext(PopupDispatchContext)

type PopupProviderProps = {
	children: React.ReactNode
}

export const PopupProvider = ({ children }: PopupProviderProps) => {
	const [popup, setPopup] = useState(defaultPopup)

	return (
		<PopupContext.Provider value={popup}>
			<PopupDispatchContext.Provider value={setPopup}>
				{children}
			</PopupDispatchContext.Provider>
		</PopupContext.Provider>
	)
}
