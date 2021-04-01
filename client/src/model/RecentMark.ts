// import Department from './Department'
// import Employee from './Employee'
// import Project from './Project'
// import Node from './Node'
// import Subnode from './Subnode'

// interface IRecentMark {
// 	id: number
// 	project: Project
// 	node: Node
// 	subnode: Subnode
// 	code: string
// 	designation: string
// 	chiefEngineerName: string
// 	complexName: string
// 	objectName: string
// 	name: string
// 	department: Department
// 	chiefSpecialist: Employee
// 	groupLeader: Employee
// 	normContr: Employee
// }

// class RecentMark {
// 	id: number
//     project: Project
// 	node: Node
// 	subnode: Subnode
// 	code: string
// 	designation: string
// 	chiefEngineerName: string
// 	complexName: string
// 	objectName: string
// 	name: string
// 	department: Department
// 	chiefSpecialist: Employee
// 	groupLeader: Employee
// 	normContr: Employee

// 	constructor(obj?: IRecentMark) {
// 		this.id = (obj && obj.id) || 0
// 		this.project = (obj && obj.project) || null
// 		this.node = (obj && obj.node) || null
// 		this.subnode = (obj && obj.subnode) || null
// 		this.code = (obj && obj.code) || ''
// 		this.designation = (obj && obj.designation) || ''
// 		this.chiefEngineerName = (obj && obj.chiefEngineerName) || ''
// 		this.complexName = (obj && obj.complexName) || ''
// 		this.objectName = (obj && obj.objectName) || ''
// 		this.name = (obj && obj.name) || ''
// 		this.department = (obj && obj.department) || null
// 		this.chiefSpecialist = (obj && obj.chiefSpecialist) || null
// 		this.groupLeader = (obj && obj.groupLeader) || null
// 		this.normContr = (obj && obj.normContr) || null
// 	}
// }

// export default RecentMark

interface IRecentMark {
	id: number
	projectId: number
	nodeId: number
	subnodeId: number
}

class RecentMark {
	id: number
	projectId: number
	nodeId: number
	subnodeId: number

	constructor(obj?: IRecentMark) {
		this.id = (obj && obj.id) || 0
		this.projectId = (obj && obj.projectId) || 0
		this.nodeId = (obj && obj.nodeId) || 0
		this.subnodeId = (obj && obj.subnodeId) || 0
	}
}

export default RecentMark

