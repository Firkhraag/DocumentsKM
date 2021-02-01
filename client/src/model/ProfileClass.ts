interface IProfileClass {
	id: number
	name: string
	note: string
}

class ProfileClass {
	id: number
	name: string
	note: string

	constructor(obj?: IProfileClass) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.note = (obj && obj.note) || ''
	}
}

export default ProfileClass
