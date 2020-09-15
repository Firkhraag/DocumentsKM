interface IPosition {
	code: number
	name: string
	shortName: string
}

class Position {
	code: number
	name: string
	shortName: string

	constructor(obj?: IPosition) {
		this.code = (obj && obj.code) || 0
		this.name = (obj && obj.name) || ''
		this.shortName = (obj && obj.shortName) || ''
	}
}

export default Position
