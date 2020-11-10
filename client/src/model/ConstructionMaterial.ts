interface IConstructionMaterial {
	id: number
	name: string
}

class ConstructionMaterial {
	id: number
	name: string

	constructor(obj?: IConstructionMaterial) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default ConstructionMaterial
