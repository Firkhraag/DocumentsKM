import Project from './Project'

interface INode {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineerName: string
}

class Node {
	id: number
	project: Project
	code: string
	name: string
	chiefEngineerName: string

	constructor(obj?: INode) {
		this.id = (obj && obj.id) || 0
		this.project = (obj && obj.project) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.chiefEngineerName = (obj && obj.chiefEngineerName) || ''
	}
}

export default Node
