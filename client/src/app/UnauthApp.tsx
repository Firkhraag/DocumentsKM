import React from 'react'
import { Redirect } from 'react-router-dom'
import Login from '../components/Login/Login'

const UnauthApp = () => {
	return (
		<div className="full-height">
			<div className="full-width div-container">
				<Login />
			</div>
			<Redirect from="*" to="/" />
		</div>
	)
}

export default UnauthApp
