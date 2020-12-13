interface IWeldingControl {
	id: number
	name: string
}

class WeldingControl {
	id: number
	name: string

	constructor(obj?: IWeldingControl) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default WeldingControl
