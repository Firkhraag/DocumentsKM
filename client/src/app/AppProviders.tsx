import React from 'react'
import { UserProvider } from '../store/UserStore'

type AppProvidersProps = {
	children: React.ReactNode
}

const AppProviders = ({ children }: AppProvidersProps) => {
	return <UserProvider>{children}</UserProvider>
}

export default AppProviders
