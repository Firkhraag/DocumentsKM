import React, { createContext, useContext, useState } from 'react'

const ScrollContext = createContext<number>(null)
const ScrollDispatchContext = createContext(null)
export const useScroll = () => useContext(ScrollContext)
export const useSetScroll = () => useContext(ScrollDispatchContext)

type ScrollProviderProps = {
	children: React.ReactNode
}

export const ScrollProvider = ({ children }: ScrollProviderProps) => {
	const [scrollState, setScrollState] = useState(0)

	return (
		<ScrollContext.Provider value={scrollState}>
			<ScrollDispatchContext.Provider value={setScrollState}>
				{children}
			</ScrollDispatchContext.Provider>
		</ScrollContext.Provider>
	)
}
