import Employee from './Employee'

interface IUser {
	id: number
	login: string
	employee: Employee
	// name: string
}

class User {
	id: number
	login: string
	employee: Employee

	constructor(obj?: IUser) {
		this.id = (obj && obj.id) || 0
		this.login = (obj && obj.login) || ''
		this.employee = (obj && obj.employee) || null
	}
}

export default User
