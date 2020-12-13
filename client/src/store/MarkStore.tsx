import React, { createContext, useContext, useState, useEffect } from 'react'
import httpClient from '../axios'
import Mark from '../model/Mark'

const MarkContext = createContext<Mark>(null)
const MarkDispatchContext = createContext(null)
export const useMark = () => useContext(MarkContext)
export const useSetMark = () => useContext(MarkDispatchContext)

type MarkProviderProps = {
	children: React.ReactNode
}

export const MarkProvider = ({ children }: MarkProviderProps) => {
	const [mark, setMark] = useState<Mark>(null)

	useEffect(() => {
		const selectedMarkId = localStorage.getItem('selectedMarkId')
		if (selectedMarkId != null) {
			const fetchData = async () => {
				try {
					const response = await httpClient.get(
						`/marks/${selectedMarkId}`
					)
					setMark(response.data)
				} catch (e) {
					localStorage.removeItem('selectedMarkId')
					// localStorage.removeItem('recentMarkIds')
				}
			}
			fetchData()
		}
	}, [])

	return (
		<MarkContext.Provider value={mark}>
			<MarkDispatchContext.Provider value={setMark}>
				{children}
			</MarkDispatchContext.Provider>
		</MarkContext.Provider>
	)
}
