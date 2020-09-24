import React from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import { MarkProvider } from '../store/MarkStore'
import Header from '../components/Header/Header'
import Home from '../components/Home/Home'
import MarkSelect from '../components/MarkSelect/MarkSelect'
import MarkData from '../components/MarkData/MarkData'
import MarkApproval from '../components/MarkApproval/MarkApproval'
import Specifications from '../components/Specifications/Specifications'
import SpecificationData from '../components/SpecificationData/SpecificationData'
import Sheets from '../components/Sheets/Sheets'
import AttachedDocs from '../components/AttachedDocs/AttachedDocs'
import LinkedDocs from '../components/LinkedDocs/LinkedDocs'
import Exploitation from '../components/Exploitation/Exploitation'

const AuthApp = () => {
	return (
		<MarkProvider>
			{/* <Router> */}
				<Switch>
					<React.Fragment>
						{/* <Header /> */}
						<div className="flex-v-cent-h full-height">
							<Route exact path="/">
								<Home />
							</Route>
							<Route exact path="/mark-select">
								<MarkSelect />
							</Route>
							<Route exact path="/mark-data">
								<MarkData />
							</Route>
							<Route exact path="/mark-data">
								<MarkData />
							</Route>
							<Route exact path="/mark-approval">
								<MarkApproval />
							</Route>
							<Route exact path="/specifications">
								<Specifications />
							</Route>
							<Route exact path="/sheets">
								<Sheets />
							</Route>
							<Route exact path="/documents">
								<AttachedDocs />
							</Route>
							<Route exact path="/exploitation">
								<Exploitation />
							</Route>
							{/* <SpecificationData /> */}
							{/* <LinkedDocs /> */}
						</div>
					</React.Fragment>
				</Switch>
			{/* </Router> */}
		</MarkProvider>
	)
}

export default AuthApp
