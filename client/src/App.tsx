import React from 'react'
import MarkData from './components/MarkData/MarkData'
import './App.css'

const App = () => {

	return (
		<div className="flex-v-cent-h full-height">
            <MarkData isCreateModeInitially={false} />
		</div>
	)
}

export default App
