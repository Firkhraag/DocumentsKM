interface IOperatingArea {
	id: number
	name: string
}

class OperatingArea {
	id: number
	name: string

	constructor(obj?: IOperatingArea) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default OperatingArea
