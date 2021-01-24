interface IStandardConstruction {
	id: number
	name: string
	num: number
	sheet: string
	weight: number
}

class StandardConstruction {
	id: number
	name: string
	num: number
	sheet: string
	weight: number

	constructor(obj?: IStandardConstruction) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.num = (obj && obj.num) || 0
		this.sheet = (obj && obj.sheet) || ''
		this.weight = (obj && obj.weight) || 0
	}
}

export default StandardConstruction
