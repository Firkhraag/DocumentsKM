import Department from './Department'
import Employee from './Employee'

interface IMark {
	id: number
	code: string
	designation: string
	chiefEngineer: string
	complexName: string
	objectName: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	normContr: Employee
}

class Mark {
	id: number
	code: string
	designation: string
	chiefEngineer: string
	complexName: string
	objectName: string
	name: string
	department: Department
	chiefSpecialist: Employee
	groupLeader: Employee
	normContr: Employee

	constructor(obj?: IMark) {
		this.id = (obj && obj.id) || 0
		this.code = (obj && obj.code) || ''
		this.designation = (obj && obj.designation) || ''
		this.chiefEngineer = (obj && obj.chiefEngineer) || ''
		this.complexName = (obj && obj.complexName) || ''
		this.objectName = (obj && obj.objectName) || ''
		this.name = (obj && obj.name) || ''
		this.department = (obj && obj.department) || null
		this.chiefSpecialist = (obj && obj.chiefSpecialist) || null
		this.groupLeader = (obj && obj.groupLeader) || null
		this.normContr = (obj && obj.normContr) || null
	}
}

export default Mark
