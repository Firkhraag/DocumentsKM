import Node from './Node'
import Mark from './Mark'

interface ISubnode {
	id: number
	node: Node
	code: string
	// name: string
	// additionalName: string
	// marks: Array<Mark>
}

class Subnode {
	id: number
	node: Node
	code: string
	// name: string
	// additionalName: string
	// marks: Array<Mark>

	constructor(obj?: ISubnode) {
		this.id = (obj && obj.id) || 0
		this.node = (obj && obj.node) || null
		this.code = (obj && obj.code) || ''
		// this.name = (obj && obj.name) || ''
		// this.additionalName = (obj && obj.additionalName) || ''
		// this.marks = (obj && obj.marks) || null
	}
}

export default Subnode
