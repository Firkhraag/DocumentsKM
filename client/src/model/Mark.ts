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
    }
}

export default Mark
