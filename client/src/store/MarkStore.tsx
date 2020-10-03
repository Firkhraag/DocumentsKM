import React, { createContext, useContext, useState, useEffect } from 'react'
import Mark from '../model/Mark'

const MarkContext = createContext<Mark>(null)
const MarkDispatchContext = createContext(null)
export const useMark = () => useContext(MarkContext)
export const useSetMark = () => useContext(MarkDispatchContext)

type MarkProviderProps = {
	children: React.ReactNode
}

export const MarkProvider = ({ children }: MarkProviderProps) => {

    const getSelectedMark = () => {
        const markStr = localStorage.getItem('selectedMark')
        if (!markStr) {
            return null
        }
        return JSON.parse(markStr) as Mark
    }

	const [mark, setMark] = useState<Mark>(getSelectedMark())
	return (
		<MarkContext.Provider value={mark}>
			<MarkDispatchContext.Provider value={setMark}>
				{children}
			</MarkDispatchContext.Provider>
		</MarkContext.Provider>
	)
}
