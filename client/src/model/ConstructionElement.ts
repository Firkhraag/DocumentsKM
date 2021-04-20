import ProfileClass from './ProfileClass'
import Steel from './Steel'

import Profile from './Profile'

interface IConstructionElement {
	id: number
	profileClass: ProfileClass
	profile: Profile
	steel: Steel
	length: number
	arithmeticExpression: string
}

class ConstructionElement {
	id: number
	profileClass: ProfileClass
	profile: Profile
	steel: Steel
	length: number
	arithmeticExpression: string

	constructor(obj?: IConstructionElement) {
		this.id = (obj && obj.id) || 0
		this.profileClass = (obj && obj.profileClass) || null
		this.profile = (obj && obj.profile) || null
		this.steel = (obj && obj.steel) || null
		this.length = (obj && obj.length) || 0
		this.arithmeticExpression = (obj && obj.arithmeticExpression) || ''
	}
}

export default ConstructionElement
