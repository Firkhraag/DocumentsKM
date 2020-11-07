interface IGasGroup {
	id: number
	name: string
}

class GasGroup {
	id: number
	name: string

	constructor(obj?: IGasGroup) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default GasGroup
