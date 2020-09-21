interface ISpecification {
	position: number
	releaseDate: string
	note: string
}

class Specification {
	position: number
	releaseDate: string
	note: string

	constructor(obj?: ISpecification) {
		this.position = (obj && obj.position) || 0
		this.releaseDate = (obj && obj.releaseDate) || ''
		this.note = (obj && obj.note) || ''
	}
}

export default Specification
