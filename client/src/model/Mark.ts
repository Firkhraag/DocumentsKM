import Subnode from './Subnode'
import Department from './Department'
import Employee from './Employee'

interface IMark {
	id: number
	subnode: Subnode
	code: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	normContr: Employee
}

class Mark {
	id: number
	subnode: Subnode
	code: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	normContr: Employee

	constructor(obj?: IMark) {
		this.id = (obj && obj.id) || 0
		this.subnode = (obj && obj.subnode) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.department = (obj && obj.department) || null
		this.chiefSpecialist = (obj && obj.chiefSpecialist) || null
		this.groupLeader = (obj && obj.groupLeader) || null
		this.normContr = (obj && obj.normContr) || null
	}
}

export default Mark
