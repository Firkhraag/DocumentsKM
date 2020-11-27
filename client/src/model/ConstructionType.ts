interface IConstructionType {
	id: number
	name: string
}

class ConstructionType {
	id: number
	name: string

	constructor(obj?: IConstructionType) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default ConstructionType
