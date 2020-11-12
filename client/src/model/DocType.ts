interface IDocType {
	id: number
	code: string
}

class DocType {
	id: number
	code: string

	constructor(obj?: IDocType) {
		this.id = (obj && obj.id) || 0
		this.code = (obj && obj.code) || ''
	}
}

export default DocType
