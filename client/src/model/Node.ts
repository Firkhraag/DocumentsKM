import Project from './Project'
import Employee from './Employee'

interface INode {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineer: Employee
}

class Node {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineer: Employee

	constructor(obj?: INode) {
		this.id = (obj && obj.id) || 0
		this.project = (obj && obj.project) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.chiefEngineer = (obj && obj.chiefEngineer) || null
	}
}

export default Node
