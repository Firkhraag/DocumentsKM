import Project from './Project'
import Employee from './Employee'

interface INode {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineerName: Employee
}

class Node {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineerName: Employee

	constructor(obj?: INode) {
		this.id = (obj && obj.id) || 0
		this.project = (obj && obj.project) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.chiefEngineerName = (obj && obj.chiefEngineerName) || null
	}
}

export default Node
