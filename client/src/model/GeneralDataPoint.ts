import GeneralDataSection from './GeneralDataSection'

interface IGeneralDataPoint {
	id: number
	section: GeneralDataSection
	text: string
	orderNum: number
}

class GeneralDataPoint {
	id: number
	section: GeneralDataSection
	text: string
	orderNum: number

	constructor(obj?: IGeneralDataPoint) {
		this.id = (obj && obj.id) || 0
		this.section = (obj && obj.section) || null
		this.text = (obj && obj.text) || ''
		this.orderNum = (obj && obj.orderNum) || 0
	}
}

export default GeneralDataPoint
