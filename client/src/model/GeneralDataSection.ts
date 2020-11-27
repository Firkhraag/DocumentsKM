interface IGeneralDataSection {
	id: number
	name: string
}

class GeneralDataSection {
	id: number
	name: string

	constructor(obj?: IGeneralDataSection) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default GeneralDataSection
