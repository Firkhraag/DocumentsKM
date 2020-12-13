import React from 'react'
import { BrowserRouter as Router } from 'react-router-dom'
import Spinner from '../components/Spinner/Spinner'
import AuthApp from './AuthApp'
import UnauthApp from './UnauthApp'
import { useUser } from '../store/UserStore'
import './App.css'

const App = () => {
	const user = useUser()
	return (
		<Router>
			{user == null ? (
				<div className="div-container full-height">
					<Spinner />
				</div>
			) : user.id > 0 ? (
				<AuthApp />
			) : (
				<UnauthApp />
			)}
		</Router>
	)
}

export default App
