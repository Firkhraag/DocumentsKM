import LinkedDoc from './LinkedDoc'

interface IMarkLinkedDoc {
    id: number
    linkedDoc: LinkedDoc
}

class MarkLinkedDoc {
	id: number
    linkedDoc: LinkedDoc

	constructor(obj?: IMarkLinkedDoc) {
		this.id = (obj && obj.id) || 0
		this.linkedDoc = (obj && obj.linkedDoc) || null
	}
}

export default MarkLinkedDoc
