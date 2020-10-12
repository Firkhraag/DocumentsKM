import React, { useState } from 'react'
import { Switch, Route } from 'react-router-dom'
import { MarkProvider } from '../store/MarkStore'
import Header from '../components/Header/Header'
import Popup from '../components/Popup/Popup'
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
import { defaultPopupObj } from '../components/Popup/Popup'

const AuthApp = () => {
    // const [isPopupShown, setPopupShown] = useState(true)
    const [popupObj, setPopupObj] = useState(defaultPopupObj)

	return (
		<MarkProvider>
			<Switch>
				<React.Fragment>
					<Header />
					<Popup
                        popupObj={popupObj}
						// isShown={isPopupShown}
						// msg={
						// 	'Вы действительно хотите удалить выпуск спецификации №1?'
						// }
						// onAccept={() => setPopupShown(false)}
						// onCancel={() => setPopupShown(false)}
					/>
					<div>
						<Route exact path="/">
							<Home />
						</Route>
						<Route exact path="/mark-select">
							<div className="full-width container">
								<MarkSelect />
							</div>
						</Route>
						<Route exact path="/mark-data">
							<div className="full-width container">
								<MarkData isCreateMode={false} />
							</div>
						</Route>
						<Route exact path="/mark-create">
							<div className="full-width container">
								<MarkData isCreateMode={true} />
							</div>
						</Route>
						<Route exact path="/mark-approval">
							<div className="full-width container">
								<MarkApproval />
							</div>
						</Route>
						<Route exact path="/specifications">
							<div className="full-width container">
								<Specifications setPopupObj={setPopupObj} />
							</div>
						</Route>
						<Route exact path="/specification-data">
							<div className="full-width container">
								<SpecificationData />
							</div>
						</Route>
						<Route exact path="/sheets">
							<div className="full-width container">
								<Sheets />
							</div>
						</Route>
						<Route exact path="/documents">
							<div className="full-width container">
								<AttachedDocs />
							</div>
						</Route>
						<Route exact path="/exploitation">
							<div className="full-width container">
								<Exploitation />
							</div>
						</Route>
						{/* <SpecificationData /> */}
						{/* <LinkedDocs /> */}
					</div>
				</React.Fragment>
			</Switch>
		</MarkProvider>
	)
}

export default AuthApp
