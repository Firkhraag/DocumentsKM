import ProfileClass from './ProfileClass'
import ProfileType from './ProfileType'
import Steel from './Steel'

interface IConstructionElement {
    id: number
	profileClass: ProfileClass
	profileName: string
	symbol: string
	weight: number
	surfaceArea: number
	profileType: ProfileType
	steel: Steel
	length: number
	status: number
}

class ConstructionElement {
	id: number
	profileClass: ProfileClass
	profileName: string
	symbol: string
	weight: number
	surfaceArea: number
	profileType: ProfileType
	steel: Steel
	length: number
	status: number

	constructor(obj?: IConstructionElement) {
		this.id = (obj && obj.id) || 0
		this.profileClass = (obj && obj.profileClass) || null
		this.profileName = (obj && obj.profileName) || ''
		this.symbol = (obj && obj.symbol) || ''
		this.weight = (obj && obj.weight) || 0
		this.surfaceArea = (obj && obj.surfaceArea) || 0
		this.profileType = (obj && obj.profileType) || null
		this.steel = (obj && obj.steel) || null
		this.length = (obj && obj.length) || 0
		this.status = (obj && obj.status) || 0
	}
}

export default ConstructionElement
