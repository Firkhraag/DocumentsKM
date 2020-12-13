interface IUser {
    id: number
    login: string
    name: string
}

class User {
    id: number
    login: string
    name: string

	constructor(obj?: IUser) {
		this.id = (obj && obj.id) || 0
		this.login = (obj && obj.login) || ''
		this.name = (obj && obj.name) || ''
	}
}

export default User
