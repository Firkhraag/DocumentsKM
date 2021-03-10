import Department from './Department'
import Employee from './Employee'

interface IDefaultValues {
    id: number
	department: Department
	creator: Employee
	inspector: Employee
	normContr: Employee
}

class DefaultValues {
	id: number
	department: Department
	creator: Employee
	inspector: Employee
	normContr: Employee

	constructor(obj?: IDefaultValues) {
		this.id = (obj && obj.id) || 0
		this.department = (obj && obj.department) || null
		this.creator = (obj && obj.creator) || null
		this.inspector = (obj && obj.inspector) || null
		this.normContr = (obj && obj.normContr) || null
	}
}

export default DefaultValues
