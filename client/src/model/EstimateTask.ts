import Employee from './Employee'

interface IEstimateTask {
	taskText: string
    additionalText: string
    approvalEmployee: Employee
}

class EstimateTask {
	taskText: string
    additionalText: string
    approvalEmployee: Employee

	constructor(obj?: IEstimateTask) {
		this.taskText = (obj && obj.taskText) || ''
		this.additionalText = (obj && obj.additionalText) || ''
		this.approvalEmployee = (obj && obj.approvalEmployee) || null
	}
}

export default EstimateTask
