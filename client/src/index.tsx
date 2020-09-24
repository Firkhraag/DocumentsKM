import React from 'react'
import ReactDOM from 'react-dom'
import AppProviders from './app/AppProviders'
import App from './app/App'
import './index.css'

ReactDOM.render(
	<AppProviders>
		<App />
	</AppProviders>,
	document.getElementById('root')
)
