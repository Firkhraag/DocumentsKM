interface IDepartment {
	id: number
	name: string
}

class Department {
	id: number
	name: string

	constructor(obj?: IDepartment) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default Department
