interface ISpecification {
    id: number
    releaseNumber: number
    created: string
    isCurrent: boolean
	note: string
}

class Specification {
    id: number
    releaseNumber: number
    created: string
    isCurrent: boolean
	note: string

	constructor(obj?: ISpecification) {
		this.id = (obj && obj.id) || 0
		this.releaseNumber = (obj && obj.releaseNumber) || 0
        this.created = (obj && obj.created) || ''
        this.isCurrent = (obj && obj.isCurrent) || false
		this.note = (obj && obj.note) || ''
	}
}

export default Specification
