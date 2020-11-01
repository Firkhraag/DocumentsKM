import Employee from './Employee'

interface ISheet {
    id: number
    num: number
	name: string
    form: number
    creator: Employee
    inspector: Employee
    normController: Employee
    releaseNum: number
    numOfPages: number
    note: string
}

class Sheet {
    id: number
    num: number
	name: string
    form: number
    creator: Employee
    inspector: Employee
    normController: Employee
    releaseNum: number
    numOfPages: number
    note: string

	constructor(obj?: ISheet) {
		this.id = (obj && obj.id) || 0
		this.num = (obj && obj.num) || 0
        this.name = (obj && obj.name) || ''
        this.form = (obj && obj.form) || 0
        this.creator = (obj && obj.creator) || null
        this.inspector = (obj && obj.inspector) || null
        this.normController = (obj && obj.normController) || null
        this.releaseNum = (obj && obj.releaseNum) || 0
        this.numOfPages = (obj && obj.numOfPages) || 0
		this.note = (obj && obj.note) || ''
	}
}

export default Sheet
