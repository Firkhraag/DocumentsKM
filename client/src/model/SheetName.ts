interface ISheetName {
	id: number
	name: string
}

class SheetName {
	id: number
	name: string

	constructor(obj?: ISheetName) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default SheetName
