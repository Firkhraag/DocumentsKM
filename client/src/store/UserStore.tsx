import React, { createContext, useContext, useState, useEffect } from 'react'
import httpClient from '../axios'
import User from '../model/User'

const UserContext = createContext<User>(null)

type DispatchContextType = {
	login: (login: string, password: string) => Promise<void>
	logout: () => Promise<void>
}

const AuthDispatchContext = createContext(null as DispatchContextType)
export const useUser = () => useContext(UserContext)
export const useAuthMethods = () => useContext(AuthDispatchContext)

type UserProviderProps = {
	children: React.ReactNode
}

export const UserProvider = ({ children }: UserProviderProps) => {
	const [user, setUser] = useState<User>(null)

	const login = async (login: string, password: string) => {
		if (login.length > 0 && password.length > 0) {
			const response = await httpClient.post('/users/login', {
				login: login,
				password: password,
			})
			setUser(response.data)
		} else {
			throw new Error('Неверный логин или пароль')
		}
	}
	const logout = async () => {
		await httpClient.post('/users/logout')
		setUser({
            id: -1,
            login: '',
            name: ''
        })
		localStorage.removeItem('selectedMarkId')
		// localStorage.removeItem('recentMarkIds')
	}

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await httpClient.post('/users/refresh-token')
				setUser(response.data)
			} catch (e) {
				setUser({
                    id: -1,
                    login: '',
                    name: ''
                })
				localStorage.removeItem('selectedMarkId')
				// localStorage.removeItem('recentMarkIds')
			}
		}
		fetchData()
	}, [])

	return (
		<UserContext.Provider value={user}>
			<AuthDispatchContext.Provider value={{ login, logout }}>
				{children}
			</AuthDispatchContext.Provider>
		</UserContext.Provider>
	)
}
