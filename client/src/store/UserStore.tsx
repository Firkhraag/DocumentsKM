import React, { createContext, useContext, useState, useEffect } from 'react'
import httpClient from '../axios'

const UserContext = createContext<string>(null)

type DispatchContextType = {
	login: (login: string, password: string) => void
	logout: () => void
}

const AuthDispatchContext = createContext(null as DispatchContextType)
export const useUser = () => useContext(UserContext)
export const useAuthMethods = () => useContext(AuthDispatchContext)

type UserProviderProps = {
	children: React.ReactNode
}

export const UserProvider = ({ children }: UserProviderProps) => {
	const [userName, setUserName] = useState<string>(null)

	const login = async (login: string, password: string) => {
		if (
			login.length > 0 &&
			login.length < 256 &&
			password.length > 0 &&
			password.length < 256
		) {
			const response = await httpClient.post('/users/login', {
				login: login,
				password: password,
			})
			setUserName(response.data.fullName)
		} else {
			throw new Error('Неверный логин или пароль')
		}
	}
	const logout = async () => {
        await httpClient.post('/users/logout')
        setUserName('')
		localStorage.removeItem('selectedMarkId')
		localStorage.removeItem('recentSubnodeIds')
		localStorage.removeItem('recentMarkIds')
	}

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await httpClient.post('/users/refresh-token')
				setUserName(response.data.fullName)
			} catch (e) {
				setUserName('')
				localStorage.removeItem('selectedMarkId')
				localStorage.removeItem('recentSubnodeIds')
				localStorage.removeItem('recentMarkIds')
			}
		}
		fetchData()
	}, [])

	return (
		<UserContext.Provider value={userName}>
			<AuthDispatchContext.Provider value={{ login, logout }}>
				{children}
			</AuthDispatchContext.Provider>
		</UserContext.Provider>
	)
}
