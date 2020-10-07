import Subnode from './Subnode'
import Department from './Department'
import Employee from './Employee'

interface IMark {
	id: number
	subnode: Subnode
	code: string
	// additionalCode: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	mainBuilder: Employee
	// approvalSpecialist1: Employee
	// approvalSpecialist2: Employee
	// approvalSpecialist3: Employee
	// approvalSpecialist4: Employee
	// approvalSpecialist5: Employee
	// approvalSpecialist6: Employee
	// approvalSpecialist7: Employee
}

class Mark {
	id: number
	subnode: Subnode
	code: string
	// additionalCode: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	mainBuilder: Employee
	// approvalSpecialist1: Employee
	// approvalSpecialist2: Employee
	// approvalSpecialist3: Employee
	// approvalSpecialist4: Employee
	// approvalSpecialist5: Employee
	// approvalSpecialist6: Employee
	// approvalSpecialist7: Employee

	constructor(obj?: IMark) {
		this.id = (obj && obj.id) || 0
		this.subnode = (obj && obj.subnode) || null
		this.code = (obj && obj.code) || ''
		// this.additionalCode = (obj && obj.additionalCode) || ''
		this.name = (obj && obj.name) || ''
		this.department = (obj && obj.department) || null
		this.chiefSpecialist = (obj && obj.chiefSpecialist) || null
		this.groupLeader = (obj && obj.groupLeader) || null
		this.mainBuilder = (obj && obj.mainBuilder) || null
		// this.approvalSpecialist1 = (obj && obj.approvalSpecialist1) || null
		// this.approvalSpecialist2 = (obj && obj.approvalSpecialist2) || null
		// this.approvalSpecialist3 = (obj && obj.approvalSpecialist3) || null
		// this.approvalSpecialist4 = (obj && obj.approvalSpecialist4) || null
		// this.approvalSpecialist5 = (obj && obj.approvalSpecialist5) || null
		// this.approvalSpecialist6 = (obj && obj.approvalSpecialist6) || null
		// this.approvalSpecialist7 = (obj && obj.approvalSpecialist7) || null
    }
}

export default Mark
