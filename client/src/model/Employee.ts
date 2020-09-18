import Department from './Department'
import Position from './Position'

interface IEmployee {
	id: number
    fullName: string
	// department: Department
	// position: Position
	// phoneNumber: string
	// isCanteen: boolean
    // vacationType: number
}

class Employee {
	id: number
	fullName: string
	// department: Department
	// position: Position
	// phoneNumber: string
	// isCanteen: boolean
	// vacationType: number

	constructor(obj?: IEmployee) {
		this.id = (obj && obj.id) || 0
		this.fullName = (obj && obj.fullName) || ''
		// this.department = (obj && obj.department) || null
		// this.position = (obj && obj.position) || null
		// this.phoneNumber = (obj && obj.phoneNumber) || ''
		// this.isCanteen = (obj && obj.isCanteen) || false
		// this.vacationType = (obj && obj.vacationType) || 0
	}
}

export default Employee
