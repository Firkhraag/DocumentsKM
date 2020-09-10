import React from 'react'
import BrandData from './components/BrandData/BrandData'
import './App.css'

const App = () => {

	return (
		<div className="brand-data-cnt flex-v-cent-h full-height">
            <BrandData isCreateModeInitially={false} />
		</div>
	)
}

export default App
