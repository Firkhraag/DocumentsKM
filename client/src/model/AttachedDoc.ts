interface IAttachedDoc {
    id: number
    designation: string
    name: string
    note: string
}

class AttachedDoc {
	id: number
    designation: string
    name: string
    note: string

	constructor(obj?: IAttachedDoc) {
        this.id = (obj && obj.id) || 0
        this.designation = (obj && obj.designation) || ''
		this.name = (obj && obj.name) || ''
		this.note = (obj && obj.note) || ''
	}
}

export default AttachedDoc
