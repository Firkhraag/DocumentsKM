import React from 'react'
import { Redirect } from 'react-router-dom'
import Login from '../components/Login/Login'

const UnauthApp = () => {
	return (
		<div className="container full-height">
			<Login />
			<Redirect from="*" to="/" />
		</div>
	)
}

export default UnauthApp
