import Employee from './Employee'

interface IMarkApprovals {
	approvalSpecialist1: Employee
	approvalSpecialist2: Employee
	approvalSpecialist3: Employee
	approvalSpecialist4: Employee
	approvalSpecialist5: Employee
	approvalSpecialist6: Employee
	approvalSpecialist7: Employee
}

class MarkApprovals {
	approvalSpecialist1: Employee
	approvalSpecialist2: Employee
	approvalSpecialist3: Employee
	approvalSpecialist4: Employee
	approvalSpecialist5: Employee
	approvalSpecialist6: Employee
	approvalSpecialist7: Employee

	constructor(obj?: IMarkApprovals) {
		this.approvalSpecialist1 = (obj && obj.approvalSpecialist1) || null
		this.approvalSpecialist2 = (obj && obj.approvalSpecialist2) || null
		this.approvalSpecialist3 = (obj && obj.approvalSpecialist3) || null
		this.approvalSpecialist4 = (obj && obj.approvalSpecialist4) || null
		this.approvalSpecialist5 = (obj && obj.approvalSpecialist5) || null
		this.approvalSpecialist6 = (obj && obj.approvalSpecialist6) || null
		this.approvalSpecialist7 = (obj && obj.approvalSpecialist7) || null
    }
}

export default MarkApprovals
