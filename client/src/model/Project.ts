interface IProject {
	id: number
	name: string
	baseSeries: string
	bias: number
}

class Project {
	id: number
	name: string
	baseSeries: string
	bias: number

	constructor(obj?: IProject) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.baseSeries = (obj && obj.baseSeries) || ''
		this.bias = (obj && obj.bias) || 0
	}
}

export default Project
