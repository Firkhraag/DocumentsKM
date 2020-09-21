import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { MarkProvider } from './store/MarkStore'
import Login from './components/Login/Login'
import MarkSelect from './components/MarkSelect/MarkSelect'
import MarkData from './components/MarkData/MarkData'
import MarkApproval from './components/MarkApproval/MarkApproval'
import Specifications from './components/Specifications/Specifications'
import SpecificationData from './components/SpecificationData/SpecificationData'
import './App.css'

const App = () => {
	return (
        <MarkProvider>
            {/* <Router>
                <Routes> */}
                    <div className="flex-v-cent-h full-height">
                        <Login />
                        <MarkSelect />
                        <MarkData />
                        <MarkApproval />
                        <Specifications />
                        <SpecificationData />
                    </div>
                {/* </Routes>
            </Router> */}
        </MarkProvider>
	)
}

export default App
