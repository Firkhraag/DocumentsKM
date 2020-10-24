import Node from './Node'

interface ISubnode {
	id: number
	node: Node
	code: string
	name: string
}

class Subnode {
	id: number
	node: Node
	code: string
	name: string

	constructor(obj?: ISubnode) {
		this.id = (obj && obj.id) || 0
		this.node = (obj && obj.node) || null
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
	}
}

export default Subnode
