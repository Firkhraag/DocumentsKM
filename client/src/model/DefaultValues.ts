import Department from './Department'
import Employee from './Employee'

interface IDefaultValues {
	department: Department
	creator: Employee
	inspector: Employee
	normContr: Employee
}

class DefaultValues {
	department: Department
	creator: Employee
	inspector: Employee
	normContr: Employee

	constructor(obj?: IDefaultValues) {
		this.department = (obj && obj.department) || null
		this.creator = (obj && obj.creator) || null
		this.inspector = (obj && obj.inspector) || null
		this.normContr = (obj && obj.normContr) || null
	}
}

export default DefaultValues
