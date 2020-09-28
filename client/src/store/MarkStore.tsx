import React, { createContext, useContext, useState, useEffect } from 'react'
import Mark from '../model/Mark'

const MarkContext = createContext<Mark>(null)
const MarkDispatchContext = createContext(null)
export const useMark = () => {
	return [useContext(MarkContext), useContext(MarkDispatchContext)]
}

type MarkProviderProps = {
	children: React.ReactNode
}

export const MarkProvider = ({ children }: MarkProviderProps) => {

    const getSelectedMark = () => {
        const markStr = localStorage.getItem('selectedMark')
        if (!markStr) {
            return '-'
        }
        const mark = JSON.parse(markStr)
        const now = new Date()
        if (now.getTime() > mark.expiry) {
            localStorage.removeItem('selectedMark')
            return '-'
        }
        return mark.name
    }

	const [mark, setMark] = useState<Mark>(null)
	return (
		<MarkContext.Provider value={mark}>
			<MarkDispatchContext.Provider value={setMark}>
				{children}
			</MarkDispatchContext.Provider>
		</MarkContext.Provider>
	)
}
