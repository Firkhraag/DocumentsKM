interface ISpecification {
	id: number
	num: number
	createdDate: string
	isCurrent: boolean
	note: string
}

class Specification {
	id: number
	num: number
	createdDate: string
	isCurrent: boolean
	note: string

	constructor(obj?: ISpecification) {
		this.id = (obj && obj.id) || 0
		this.num = (obj && obj.num) || 0
		this.createdDate = (obj && obj.createdDate) || ''
		this.isCurrent = (obj && obj.isCurrent) || false
		this.note = (obj && obj.note) || ''
	}
}

export default Specification
