import GeneralDataSection from './GeneralDataSection'
import GeneralDataPoint from './GeneralDataPoint'

interface IGeneralDataModel {
	section: GeneralDataSection
	point: GeneralDataPoint
	pointText: string
	sectionText: string
}

class GeneralDataModel {
	section: GeneralDataSection
	point: GeneralDataPoint
	pointText: string
    sectionText: string

	constructor(obj?: IGeneralDataModel) {
		this.section = (obj && obj.section) || null
		this.point = (obj && obj.point) || null
		this.pointText = (obj && obj.pointText) || ''
		this.sectionText = (obj && obj.sectionText) || ''
	}
}

export default GeneralDataModel
