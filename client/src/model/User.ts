interface IUser {
	login: string
}

class User {
	login: string

	constructor(obj?: IUser) {
		this.login = (obj && obj.login) || ''
	}
}

export default User
