import Employee from './Employee'

interface IDepartment {
	number: number
	name: string
	shortName: string
	code: string
	isActive: boolean
	isIndustrial: boolean
	departmentHead: Employee
	employees: Array<Employee>
}

class Department {
	number: number
	name: string
	shortName: string
	code: string
	isActive: boolean
	isIndustrial: boolean
	departmentHead: Employee
	employees: Array<Employee>

	constructor(obj?: IDepartment) {
		this.number = (obj && obj.number) || 0
		this.name = (obj && obj.name) || ''
		this.shortName = (obj && obj.shortName) || ''
		this.code = (obj && obj.code) || ''
		this.isActive = (obj && obj.isActive) || false
		this.isIndustrial = (obj && obj.isIndustrial) || false
		this.departmentHead = (obj && obj.departmentHead) || null
		this.employees = (obj && obj.employees) || null
	}
}

export default Department
