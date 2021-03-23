import Department from './Department'

interface IEmployee {
	id: number
	fullname: string
	department: Department
}

class Employee {
	id: number
	fullname: string
	department: Department

	constructor(obj?: IEmployee) {
		this.id = (obj && obj.id) || 0
		this.fullname = (obj && obj.fullname) || ''
		this.department = (obj && obj.department) || null
	}
}

export default Employee
