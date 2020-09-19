import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Login from './components/Login/Login'
import MarkSelect from './components/MarkSelect/MarkSelect'
import MarkData from './components/MarkData/MarkData'
import MarkApproval from './components/MarkApproval/MarkApproval'
import SpecificationData from './components/MetalData/SpecificationData'
import MetalData from './components/MetalData/MetalData'
import './App.css'

const App = () => {
	return (
        // <Router>
        //     <Routes>
                <div className="flex-v-cent-h full-height">
                    <Login />
                    <MarkSelect />
                    <MarkData />
                    <MarkApproval />
                    <SpecificationData />
                    <MetalData />
                </div>
        //     </Routes>
        // </Router>
	)
}

export default App
