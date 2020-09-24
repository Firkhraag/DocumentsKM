import React from 'react'
import { AuthProvider } from '../store/AuthStore'

type AppProvidersProps = {
	children: React.ReactNode
}

const AppProviders = ({ children }: AppProvidersProps) => {
	return <AuthProvider>{children}</AuthProvider>
}

export default AppProviders
