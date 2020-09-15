import Project from './Project'
import Employee from './Employee'
import Subnode from './Subnode'

interface INode {
	id: number
	project: Project
	code: string
	name: string
	additionalName: string
	chiefEngineer: Employee
    activeNode: string
    subnodes: Array<Subnode>
}

class Node {
	id: number
	project: Project
	code: string
	name: string
	additionalName: string
	chiefEngineer: Employee
    activeNode: string
    subnodes: Array<Subnode>

	constructor(obj?: INode) {
		this.id = (obj && obj.id) || 0
		this.project = (obj && obj.project) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.additionalName = (obj && obj.additionalName) || ''
		this.chiefEngineer = (obj && obj.chiefEngineer) || null
		this.activeNode = (obj && obj.activeNode) || ''
		this.subnodes = (obj && obj.subnodes) || null
	}
}

export default Node
