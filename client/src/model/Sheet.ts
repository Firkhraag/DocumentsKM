import Employee from './Employee'

interface ISheet {
    id: number
    number: number
	name: string
    format: number
    creator: Employee
    inspector: Employee
    normController: Employee
    release: number
    numberOfPages: number
    note: string
}

class Sheet {
    id: number
    number: number
	name: string
    format: number
    creator: Employee
    inspector: Employee
    normController: Employee
    release: number
    numberOfPages: number
    note: string

	constructor(obj?: ISheet) {
		this.id = (obj && obj.id) || 0
		this.number = (obj && obj.number) || 0
        this.name = (obj && obj.name) || ''
        this.format = (obj && obj.format) || 0
        this.creator = (obj && obj.creator) || null
        this.inspector = (obj && obj.inspector) || null
        this.normController = (obj && obj.normController) || null
        this.release = (obj && obj.release) || 0
        this.numberOfPages = (obj && obj.numberOfPages) || 0
		this.note = (obj && obj.note) || ''
	}
}

export default Sheet
