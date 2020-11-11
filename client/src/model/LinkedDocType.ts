interface ILinkedDocType {
	id: number
	name: string
}

class LinkedDocType {
	id: number
	name: string

	constructor(obj?: ILinkedDocType) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default LinkedDocType
