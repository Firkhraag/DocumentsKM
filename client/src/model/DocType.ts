interface IDocType {
    id: number
    name: string
}

class DocType {
	id: number
	name: string

	constructor(obj?: IDocType) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default DocType
