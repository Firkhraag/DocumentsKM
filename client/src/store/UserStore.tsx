import React, { createContext, useContext, useState, useEffect } from 'react'
import httpClient from '../axios'

const AuthContext = createContext<string>(null)

type DispatchContextType = {
    login: (login: string, password: string) => void
    logout: () => void
}

const AuthDispatchContext = createContext(null as DispatchContextType)
export const useIsAuthenticated = () => useContext(AuthContext)
export const useAuthMethods = () => useContext(AuthDispatchContext)

type AuthProviderProps = {
	children: React.ReactNode
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [userName, setUserName] = useState<string>(null)

    const login = async (login: string, password: string) => {
        if (
			login.length > 0 &&
			login.length < 256 &&
			password.length > 0 &&
			password.length < 256
		) {
			const response = await httpClient.post('/api/users/login', {
				login: login,
				password: password,
            })
            setUserName(response.data.fullName)
		} else {
            throw new Error('Неверный логин или пароль')
        }
    }
    const logout = async () => {
        await httpClient.post('/api/users/logout')
    }
    
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await httpClient.post('/api/users/refresh-token')
                // setToken(response.data.token)
                setUserName(response.data.fullName)
            } catch (e) {
                setUserName('')
            }
        }
        fetchData()
    }, []);

	return (
		<AuthContext.Provider value={userName}>
			<AuthDispatchContext.Provider value={{login, logout}}>
				{children}
			</AuthDispatchContext.Provider>
		</AuthContext.Provider>
	)
}
