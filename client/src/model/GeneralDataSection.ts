interface IGeneralDataSection {
	id: number
	name: string
    orderNum: number
}

class GeneralDataSection {
	id: number
	name: string
    orderNum: number

	constructor(obj?: IGeneralDataSection) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.orderNum = (obj && obj.orderNum) || 0
	}
}

export default GeneralDataSection
