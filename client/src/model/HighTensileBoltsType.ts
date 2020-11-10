interface IHighTensileBoltsType {
	id: number
	name: string
}

class HighTensileBoltsType {
	id: number
	name: string

	constructor(obj?: IHighTensileBoltsType) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default HighTensileBoltsType
