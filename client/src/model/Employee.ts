import Department from './Department'

interface IEmployee {
	id: number
	name: string
	department: Department
}

class Employee {
	id: number
	name: string
	department: Department

	constructor(obj?: IEmployee) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.department = (obj && obj.department) || null
	}
}

export default Employee
