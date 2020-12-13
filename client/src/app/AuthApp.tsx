// Global
import React, { useState } from 'react'
import { Switch, Route } from 'react-router-dom'
// Other
import { MarkProvider } from '../store/MarkStore'
import Header from '../components/Header/Header'
import Popup from '../components/Popup/Popup'
import { defaultPopupObj } from '../components/Popup/Popup'
import Home from '../components/Home/Home'
import MarkSelect from '../components/Mark/MarkSelect'
import MarkData from '../components/Mark/MarkData'
import MarkApproval from '../components/MarkApproval/MarkApproval'
import SpecificationTable from '../components/Specification/SpecificationTable'
import SpecificationData from '../components/Specification/SpecificationData'
import ConstructionData from '../components/Construction/ConstructionData'
import SheetTable from '../components/Sheet/SheetTable'
import SheetData from '../components/Sheet/SheetData'
import DevelopingAttachedDocTable from '../components/DevelopingAttachedDoc/DevelopingAttachedDocTable'
import DevelopingAttachedDocData from '../components/DevelopingAttachedDoc/DevelopingAttachedDocData'
import OtherAttachedDocTable from '../components/OtherAttachedDoc/OtherAttachedDocTable'
import OtherAttachedDocData from '../components/OtherAttachedDoc/OtherAttachedDocData'
import LinkedDocTable from '../components/LinkedDoc/LinkedDocTable'
import LinkedDocData from '../components/LinkedDoc/LinkedDocData'
import OperatingConditions from '../components/OperatingConditions/OperatingConditions'
import GeneralData from '../components/MarkGeneralData/MarkGeneralData'
import UserGeneralData from '../components/UserGeneralData/UserGeneralData'
import Specification from '../model/Specification'
import Construction from '../model/Construction'
import Doc from '../model/Doc'
import AttachedDoc from '../model/AttachedDoc'
import MarkLinkedDoc from '../model/MarkLinkedDoc'

const AuthApp = () => {
	const [subnode, setSubnode] = useState(null)
	const [popupObj, setPopupObj] = useState(defaultPopupObj)
    const [specification, setSpecification] = useState<Specification>(null)
    const [construction, setConstruction] = useState<Construction>(null)
	const [sheet, setSheet] = useState<Doc>(null)
	const [developingAttachedDoc, setDevelopingAttachedDoc] = useState<Doc>(
		null
	)
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
								<MarkSelect setSubnode={setSubnode} />
							</div>
						</Route>
						<Route exact path="/marks/:markId">
							<div className="full-width div-container">
								<MarkData isCreateMode={false} subnodeForCreate={subnode} />
							</div>
						</Route>
						<Route exact path="/mark-create">
							<div className="full-width div-container">
								<MarkData isCreateMode={true} subnodeForCreate={subnode} />
							</div>
						</Route>

						<Route exact path="/approvals">
							<div className="full-width div-container">
								<MarkApproval />
							</div>
						</Route>
						<Route exact path="/specifications">
							<div className="full-width div-container">
								<SpecificationTable setPopupObj={setPopupObj} setSpecification={setSpecification} />
							</div>
						</Route>
						<Route exact path="/specifications/:specificationId">
							<div className="full-width div-container">
								<SpecificationData specification={specification} />
							</div>
						</Route>
                        <Route exact path="/specifications/:specificationId/constructions/:constructionId">
							<div className="full-width div-container">
								<ConstructionData construction={construction} isCreateMode={false} />
							</div>
						</Route>
                        <Route exact path="/specifications/:specificationId/construction-create">
							<div className="full-width div-container">
								<ConstructionData construction={construction} isCreateMode={true} />
							</div>
						</Route>

						<Route exact path="/sheets">
							<div className="full-width div-container">
								<SheetTable
									setPopupObj={setPopupObj}
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
								<DevelopingAttachedDocTable
									setPopupObj={setPopupObj}
									setDevelopingAttachedDoc={
										setDevelopingAttachedDoc
									}
								/>
							</div>
						</Route>
						<Route exact path="/developing-attached-doc-create">
							<div className="full-width div-container">
								<DevelopingAttachedDocData
									developingAttachedDoc={
										developingAttachedDoc
									}
									isCreateMode={true}
								/>
							</div>
						</Route>
						<Route
							exact
							path="/developing-attached-docs/:attachedDocId"
						>
							<div className="full-width div-container">
								<DevelopingAttachedDocData
									developingAttachedDoc={
										developingAttachedDoc
									}
									isCreateMode={false}
								/>
							</div>
						</Route>

						<Route exact path="/other-attached-docs">
							<div className="full-width div-container">
								<OtherAttachedDocTable
									setPopupObj={setPopupObj}
									setOtherAttachedDoc={setOtherAttachedDoc}
								/>
							</div>
						</Route>
						<Route exact path="/other-attached-doc-add">
							<div className="full-width div-container">
								<OtherAttachedDocData
									otherAttachedDoc={otherAttachedDoc}
									isCreateMode={true}
								/>
							</div>
						</Route>
						<Route exact path="/other-attached-docs/:attachedDocId">
							<div className="full-width div-container">
								<OtherAttachedDocData
									otherAttachedDoc={otherAttachedDoc}
									isCreateMode={false}
								/>
							</div>
						</Route>

						<Route exact path="/linked-docs">
							<div className="full-width div-container">
								<LinkedDocTable
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

                        <Route exact path="/general-data">
							<div className="full-width div-container">
								<GeneralData setPopupObj={setPopupObj} />
							</div>
						</Route>

                        <Route exact path="/user/general-data">
							<div className="full-width div-container">
								<UserGeneralData setPopupObj={setPopupObj} />
							</div>
						</Route>
					</div>
				</React.Fragment>
			</Switch>
		</MarkProvider>
	)
}

export default AuthApp
