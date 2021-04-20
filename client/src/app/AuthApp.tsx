// Global
import React, { useState } from 'react'
import { Switch, Route } from 'react-router-dom'
// Other
import { MarkProvider } from '../store/MarkStore'
import { PopupProvider } from '../store/PopupStore'
import { ScrollProvider } from '../store/ScrollStore'
import Header from '../components/Header/Header'
import Popup from '../components/Popup/Popup'
import Home from '../components/Home/Home'
import MarkSelect from '../components/Mark/MarkSelect'
import MarkData from '../components/Mark/MarkData'
import MarkApproval from '../components/MarkApproval/MarkApproval'
import SpecificationTable from '../components/Specification/SpecificationTable'
import SpecificationData from '../components/Specification/SpecificationData'
import ConstructionData from '../components/Construction/ConstructionData'
import SheetTable from '../components/Sheet/SheetTable'
import DevelopingAttachedDocTable from '../components/DevelopingAttachedDoc/DevelopingAttachedDocTable'
import OtherAttachedDocTable from '../components/OtherAttachedDoc/OtherAttachedDocTable'
import LinkedDocTable from '../components/LinkedDoc/LinkedDocTable'
import OperatingConditions from '../components/OperatingConditions/OperatingConditions'
import AdditionalWorkTable from '../components/AdditionalWork/AdditionalWorkTable'
import GeneralData from '../components/MarkGeneralData/MarkGeneralData'
import UserGeneralData from '../components/UserGeneralData/UserGeneralData'
import EstimateTaskDocument from '../components/EstimateTask/EstimateTaskDocument'
import ProjectRegistration from '../components/ProjectRegistration/ProjectRegistration'
import EstimationDocument from '../components/Estimation/EstimationDocument'
import DefaultValuesData from '../components/DefaultValues/DefaultValuesData'
import Specification from '../model/Specification'
import Construction from '../model/Construction'

const AuthApp = () => {
	const [subnode, setSubnode] = useState(null)
	const [specification, setSpecification] = useState<Specification>(null)
	const [construction, setConstruction] = useState<Construction>(null)
	const [copiedConstruction, setCopiedConstruction] = useState<Construction>(
		null
	)

	return (
		<MarkProvider>
			<PopupProvider>
			<ScrollProvider>
				<Switch>
					<React.Fragment>
						<Header />
						<Popup />
						<div>
							<Route exact path="/">
								<Home setSpecification={setSpecification} />
							</Route>

							<Route exact path="/marks">
								<div className="full-width div-container">
									<MarkSelect setSubnode={setSubnode} />
								</div>
							</Route>
							<Route exact path="/marks/:markId">
								<div className="full-width div-container">
									<MarkData
										isCreateMode={false}
										currentSubnode={subnode}
									/>
								</div>
							</Route>
							<Route exact path="/mark-create">
								<div className="full-width div-container">
									<MarkData
										isCreateMode={true}
										currentSubnode={subnode}
									/>
								</div>
							</Route>

							<Route exact path="/approvals">
								<div className="full-width div-container">
									<MarkApproval />
								</div>
							</Route>
							<Route exact path="/specifications">
								<div className="full-width div-container">
									<SpecificationTable
										setSpecification={setSpecification}
									/>
								</div>
							</Route>
							<Route
								exact
								path="/specifications/:specificationId"
							>
								<div className="full-width div-container">
									<SpecificationData
										specification={specification}
										setConstruction={setConstruction}
										copiedConstruction={
											copiedConstruction
										}
										setCopiedConstruction={
											setCopiedConstruction
										}
									/>
								</div>
							</Route>
							<Route
								exact
								path="/specifications/:specificationId/constructions/:constructionId"
							>
								<div className="full-width div-container">
									<ConstructionData
										construction={construction}
										isCreateMode={false}
										specificationId={
											specification == null
												? -1
												: specification.id
										}
									/>
								</div>
							</Route>
							<Route
								exact
								path="/specifications/:specificationId/construction-create"
							>
								<div className="full-width div-container">
									<ConstructionData
										construction={construction}
										isCreateMode={true}
										specificationId={
											specification == null
												? -1
												: specification.id
										}
									/>
								</div>
							</Route>

							<Route exact path="/sheets">
								<div className="full-width div-container">
									<SheetTable />
								</div>
							</Route>

							<Route exact path="/developing-attached-docs">
								<div className="full-width div-container">
									<DevelopingAttachedDocTable />
								</div>
							</Route>

							<Route exact path="/other-attached-docs">
								<div className="full-width div-container">
									<OtherAttachedDocTable />
								</div>
							</Route>

							<Route exact path="/linked-docs">
								<div className="full-width div-container">
									<LinkedDocTable />
								</div>
							</Route>

							<Route exact path="/operating-conditions">
								<div className="full-width div-container">
									<OperatingConditions />
								</div>
							</Route>

							<Route exact path="/additional-work">
								<div className="full-width div-container">
									<AdditionalWorkTable />
								</div>
							</Route>

							<Route exact path="/general-data">
								<div className="full-width div-container">
									<GeneralData />
								</div>
							</Route>
							<Route exact path="/user/general-data">
								<div className="full-width div-container">
									<UserGeneralData />
								</div>
							</Route>

                            <Route exact path="/user/default-values">
								<div className="full-width div-container">
									<DefaultValuesData />
								</div>
							</Route>

                            <Route exact path="/estimate-task">
								<div className="full-width div-container">
									<EstimateTaskDocument />
								</div>
							</Route>
                            <Route exact path="/project-registration">
								<div className="full-width div-container">
									<ProjectRegistration />
								</div>
							</Route>
							<Route exact path="/estimation">
								<div className="full-width div-container">
									<EstimationDocument />
								</div>
							</Route>
						</div>
					</React.Fragment>
				</Switch>
            </ScrollProvider>
			</PopupProvider>
		</MarkProvider>
	)
}

export default AuthApp
