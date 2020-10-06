import React from 'react'
import { Redirect } from 'react-router-dom'
import Login from '../components/Login/Login'

const UnauthApp = () => {
	return (
		<div className="container full-height">
			<div className="full-width container">
				<Login />
			</div>
			<Redirect from="*" to="/" />
		</div>
	)
}

export default UnauthApp
