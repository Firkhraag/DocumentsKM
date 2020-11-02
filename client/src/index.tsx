import React from 'react'
import ReactDOM from 'react-dom'
import AppProviders from './app/AppProviders'
import App from './app/App'
import './index.css'

// Bootstrap
import 'bootstrap/dist/css/bootstrap.min.css'

ReactDOM.render(
	<AppProviders>
		<App />
	</AppProviders>,
	document.getElementById('root')
)
