interface IStandardConstructionName {
	id: number
	name: string
}

class StandardConstructionName {
	id: number
	name: string

	constructor(obj?: IStandardConstructionName) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default StandardConstructionName
