import LinkedDocType from './LinkedDocType'

interface ILinkedDoc {
	id: number
	code: string
	name: string
	type: LinkedDocType
	designation: string
}

class LinkedDoc {
	id: number
	code: string
	name: string
	type: LinkedDocType
	designation: string

	constructor(obj?: ILinkedDoc) {
		this.id = (obj && obj.id) || 0
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.type = (obj && obj.type) || null
		this.designation = (obj && obj.designation) || ''
	}
}

export default LinkedDoc
