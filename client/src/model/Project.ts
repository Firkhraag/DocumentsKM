interface IProject {
	id: number
	name: string
	baseSeries: string
}

class Project {
	id: number
	name: string
	baseSeries: string

	constructor(obj?: IProject) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.baseSeries = (obj && obj.baseSeries) || ''
	}
}

export default Project
