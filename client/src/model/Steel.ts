interface ISteel {
	id: number
	name: string
	standard: string
	strength: number
}

class Steel {
	id: number
	name: string
	standard: string
	strength: number

	constructor(obj?: ISteel) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.standard = (obj && obj.standard) || ''
		this.strength = (obj && obj.strength) || 0
	}
}

export default Steel
