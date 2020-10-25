interface IDepartment {
	number: number
	name: string
	shortName: string
	code: string
	isActive: boolean
	isIndustrial: boolean
}

class Department {
	number: number
	name: string
	shortName: string
	code: string
	isActive: boolean
	isIndustrial: boolean

	constructor(obj?: IDepartment) {
		this.number = (obj && obj.number) || 0
		this.name = (obj && obj.name) || ''
		this.shortName = (obj && obj.shortName) || ''
		this.code = (obj && obj.code) || ''
		this.isActive = (obj && obj.isActive) || false
		this.isIndustrial = (obj && obj.isIndustrial) || false
	}
}

export default Department
