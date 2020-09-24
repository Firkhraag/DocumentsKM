import React from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import AuthApp from './AuthApp'
import UnauthApp from './UnauthApp'
import { useIsAuthenticated } from '../store/AuthStore'
import './App.css'

const App = () => {
	const isAuthenticated = useIsAuthenticated()
	return <Router>{isAuthenticated ? <AuthApp /> : <UnauthApp />}</Router>
}

export default App
