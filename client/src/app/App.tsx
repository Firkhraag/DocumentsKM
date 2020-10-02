import React from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Spinner from '../components/Spinner/Spinner'
import AuthApp from './AuthApp'
import UnauthApp from './UnauthApp'
import { useIsAuthenticated } from '../store/UserStore'
import './App.css'

const App = () => {
	const isAuthenticated = useIsAuthenticated()
	return (
		<Router>
			{isAuthenticated == null ? (
				<div className="container full-height">
					<Spinner />
				</div>
			) : isAuthenticated ? (
				<AuthApp />
			) : (
				<UnauthApp />
			)}
		</Router>
	)
}

export default App
