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
import SheetData from '../components/SheetData/SheetData'
import DevelopingAttachedDocs from '../components/DevelopingAttachedDocs/DevelopingAttachedDocs'
import OtherAttachedDocs from '../components/OtherAttachedDocs/OtherAttachedDocs'
import LinkedDocs from '../components/LinkedDocs/LinkedDocs'
import LinkedDocData from '../components/LinkedDocData/LinkedDocData'
import OtherAttachedDocData from '../components/OtherAttachedDocData/OtherAttachedDocData'
import OperatingConditions from '../components/OperatingConditions/OperatingConditions'
import { defaultPopupObj } from '../components/Popup/Popup'
import Doc from '../model/Doc'
import AttachedDoc from '../model/AttachedDoc'
import MarkLinkedDoc from '../model/MarkLinkedDoc'

const AuthApp = () => {
	const [popupObj, setPopupObj] = useState(defaultPopupObj)
    const [sheet, setSheet] = useState<Doc>(null)
    const [developingAttachedDoc, setDevelopingAttachedDoc] = useState<Doc>(null)
    const [otherAttachedDoc, setOtherAttachedDoc] = useState<AttachedDoc>(null)
	const [markLinkedDoc, setMarkLinkedDoc] = useState<MarkLinkedDoc>(null)

	return (
		<MarkProvider>
			<Switch>
				<React.Fragment>
					<Header />
					<Popup popupObj={popupObj} />
					<div>
						<Route exact path="/">
							<Home />
						</Route>

						<Route exact path="/marks">
							<div className="full-width div-container">
								<MarkSelect />
							</div>
						</Route>
						<Route exact path="/marks/:markId">
							<div className="full-width div-container">
								<MarkData isCreateMode={false} />
							</div>
						</Route>
						<Route exact path="/mark-create">
							<div className="full-width div-container">
								<MarkData isCreateMode={true} />
							</div>
						</Route>

						<Route exact path="/approvals">
							<div className="full-width div-container">
								<MarkApproval />
							</div>
						</Route>
						<Route exact path="/specifications">
							<div className="full-width div-container">
								<Specifications setPopupObj={setPopupObj} />
							</div>
						</Route>
						<Route exact path="/specifications/:specificationId">
							<div className="full-width div-container">
								<SpecificationData />
							</div>
						</Route>

						<Route exact path="/sheets">
							<div className="full-width div-container">
								<Sheets
									setPopupObj={setPopupObj}
									// setSheet={(s: Sheet) => setSheet(s)}
									setSheet={setSheet}
								/>
							</div>
						</Route>
						<Route exact path="/sheets/:sheetId">
							<div className="full-width div-container">
								<SheetData sheet={sheet} isCreateMode={false} />
							</div>
						</Route>
						<Route exact path="/sheet-create">
							<div className="full-width div-container">
								<SheetData sheet={sheet} isCreateMode={true} />
							</div>
						</Route>

						<Route exact path="/developing-attached-docs">
							<div className="full-width div-container">
								<DevelopingAttachedDocs setPopupObj={setPopupObj}
									setDevelopingAttachedDoc={setDevelopingAttachedDoc} />
							</div>
						</Route>

                        <Route exact path="/other-attached-docs">
							<div className="full-width div-container">
								<OtherAttachedDocs setPopupObj={setPopupObj}
									setOtherAttachedDoc={setOtherAttachedDoc} />
							</div>
						</Route>
                        <Route exact path="/other-attached-doc-add">
							<div className="full-width div-container">
								<OtherAttachedDocData otherAttachedDoc={otherAttachedDoc} isCreateMode={true} />
							</div>
						</Route>
                        <Route exact path="/other-attached-docs/:attachedDocId">
							<div className="full-width div-container">
                                <OtherAttachedDocData otherAttachedDoc={otherAttachedDoc} isCreateMode={false} />
							</div>
						</Route>

						<Route exact path="/linked-docs">
							<div className="full-width div-container">
								<LinkedDocs
									setPopupObj={setPopupObj}
									setMarkLinkedDoc={setMarkLinkedDoc}
								/>
							</div>
						</Route>
						<Route exact path="/linked-docs/:linkedDocId">
							<div className="full-width div-container">
								<LinkedDocData
									markLinkedDoc={markLinkedDoc}
									isCreateMode={false}
								/>
							</div>
						</Route>
						<Route exact path="/linked-doc-add">
							<div className="full-width div-container">
								<LinkedDocData
									markLinkedDoc={markLinkedDoc}
									isCreateMode={true}
								/>
							</div>
						</Route>

						<Route exact path="/operating-conditions">
							<div className="full-width div-container">
								<OperatingConditions />
							</div>
						</Route>
					</div>
				</React.Fragment>
			</Switch>
		</MarkProvider>
	)
}

export default AuthApp
