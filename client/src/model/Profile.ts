import ProfileClass from './ProfileClass'
import ProfileType from './ProfileType'

interface IProfile {
	id: number
	class: ProfileClass
    name: string
    symbol: string
    weight: number
    area: number
    type: ProfileType
}

class Profile {
	id: number
	class: ProfileClass
    name: string
    symbol: string
    weight: number
    area: number
    type: ProfileType

	constructor(obj?: IProfile) {
		this.id = (obj && obj.id) || 0
		this.class = (obj && obj.class) || null
		this.name = (obj && obj.name) || ''
		this.symbol = (obj && obj.symbol) || ''
		this.weight = (obj && obj.weight) || 0
		this.area = (obj && obj.area) || 0
		this.type = (obj && obj.type) || null
	}
}

export default Profile
