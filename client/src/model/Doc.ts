import DocType from './DocType'
import Employee from './Employee'

interface IDoc {
	id: number
	num: number
	name: string
	form: number
	type: DocType
	creator: Employee
	inspector: Employee
	normContr: Employee
	releaseNum: number
	numOfPages: number
	note: string
}

class Doc {
	id: number
	num: number
	name: string
	form: number
	type: DocType
	creator: Employee
	inspector: Employee
	normContr: Employee
	releaseNum: number
	numOfPages: number
	note: string

	constructor(obj?: IDoc) {
		this.id = (obj && obj.id) || 0
		this.num = (obj && obj.num) || 0
		this.name = (obj && obj.name) || ''
		this.form = (obj && obj.form) || 0
		this.type = (obj && obj.type) || null
		this.creator = (obj && obj.creator) || null
		this.inspector = (obj && obj.inspector) || null
		this.normContr = (obj && obj.normContr) || null
		this.releaseNum = (obj && obj.releaseNum) || 0
		this.numOfPages = (obj && obj.numOfPages) || 0
		this.note = (obj && obj.note) || ''
	}
}

export default Doc
