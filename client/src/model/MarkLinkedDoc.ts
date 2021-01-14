import LinkedDoc from './LinkedDoc'

interface IMarkLinkedDoc {
    id: number
    linkedDoc: LinkedDoc
    note: string
}

class MarkLinkedDoc {
	id: number
    linkedDoc: LinkedDoc
    note: string

	constructor(obj?: IMarkLinkedDoc) {
		this.id = (obj && obj.id) || 0
        this.linkedDoc = (obj && obj.linkedDoc) || null
        this.note = (obj && obj.note) || ''
	}
}

export default MarkLinkedDoc
