import Employee from './Employee'
import Node from './Node'

interface IProject {
	id: number
	type: number
	name: string
	additionalName: string
	baseSeries: string
	approved1: Employee
	approved2: Employee
	nodes: Array<Node>
}

class Project {
	id: number
	type: number
	name: string
	additionalName: string
	baseSeries: string
	approved1: Employee
	approved2: Employee
	nodes: Array<Node>

	constructor(obj?: IProject) {
		this.id = (obj && obj.id) || 0
		this.type = (obj && obj.type) || 0
		this.name = (obj && obj.name) || ''
		this.additionalName = (obj && obj.additionalName) || ''
		this.baseSeries = (obj && obj.baseSeries) || ''
		this.approved1 = (obj && obj.approved1) || null
		this.approved2 = (obj && obj.approved2) || null
		this.nodes = (obj && obj.nodes) || null
	}
}

export default Project
