interface IConstructionSubtype {
	id: number
	name: string
	valuation: string
}

class ConstructionSubtype {
	id: number
	name: string
	valuation: string

	constructor(obj?: IConstructionSubtype) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.valuation = (obj && obj.valuation) || ''
	}
}

export default ConstructionSubtype
