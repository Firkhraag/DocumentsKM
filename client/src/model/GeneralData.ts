import GeneralDataSection from './GeneralDataSection'

interface IGeneralData {
	id: number
	section: GeneralDataSection
}

class GeneralData {
	id: number
	section: GeneralDataSection

	constructor(obj?: IGeneralData) {
		this.id = (obj && obj.id) || 0
		this.section = (obj && obj.section) || null
	}
}

export default GeneralData
