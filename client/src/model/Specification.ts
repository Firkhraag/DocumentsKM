interface ISpecification {
    id: number
    releaseNumber: number
	created: string
	note: string
}

class Specification {
    id: number
    releaseNumber: number
	created: string
	note: string

	constructor(obj?: ISpecification) {
		this.id = (obj && obj.id) || 0
		this.releaseNumber = (obj && obj.releaseNumber) || 0
		this.created = (obj && obj.created) || ''
		this.note = (obj && obj.note) || ''
	}
}

export default Specification
