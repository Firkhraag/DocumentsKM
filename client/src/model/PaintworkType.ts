interface IPaintworkType {
	id: number
	name: string
}

class PaintworkType {
	id: number
	name: string

	constructor(obj?: IPaintworkType) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default PaintworkType
