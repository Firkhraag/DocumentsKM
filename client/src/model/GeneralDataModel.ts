import GeneralDataSection from './GeneralDataSection'
import GeneralDataPoint from './GeneralDataPoint'

interface IGeneralDataModel {
	section: GeneralDataSection
    point: GeneralDataPoint
    pointText: string
}

class GeneralDataModel {
	section: GeneralDataSection
    point: GeneralDataPoint
    pointText: string

	constructor(obj?: IGeneralDataModel) {
		this.section = (obj && obj.section) || null
		this.point = (obj && obj.point) || null
		this.pointText = (obj && obj.pointText) || ''
	}
}

export default GeneralDataModel
