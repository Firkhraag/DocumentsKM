import Department from './Department'

interface IArchiveMark {
	code: string
	name: string
	department: Department
}

class ArchiveMark {
	code: string
	name: string
	department: Department

	constructor(obj?: IArchiveMark) {
		this.code = (obj && obj.code) || ''
		this.name = (obj && obj.name) || ''
		this.department = (obj && obj.department) || null
	}
}

export default ArchiveMark
