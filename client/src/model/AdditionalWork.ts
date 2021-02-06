import Employee from './Employee'

interface IAdditionalWork {
	id: number
	employee: Employee
	valuation: number
	metalOrder: number
	drawingsCompleted: number
	drawingsCheck: number
}

class AdditionalWork {
	id: number
	employee: Employee
	valuation: number
	metalOrder: number
	drawingsCompleted: number
	drawingsCheck: number

	constructor(obj?: IAdditionalWork) {
		this.id = (obj && obj.id) || 0
		this.employee = (obj && obj.employee) || null
		this.valuation = (obj && obj.valuation) || 0
		this.metalOrder = (obj && obj.metalOrder) || 0
		this.drawingsCompleted = (obj && obj.drawingsCompleted) || 0
		this.drawingsCheck = (obj && obj.drawingsCheck) || 0
	}
}

export default AdditionalWork
