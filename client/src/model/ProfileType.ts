interface IProfileClass {
	id: number
	name: string
}

class ProfileClass {
	id: number
	name: string

	constructor(obj?: IProfileClass) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default ProfileClass
