import Subnode from './Subnode'
import Department from './Department'
import Employee from './Employee'

interface IMark {
	id: number
	subnode: Subnode
	code: string
	// additionalCode: string
	// name: string
	// department: Department
	// chiefSpecialist: Employee
	// groupLeader: Employee
	// mainBulder: Employee
	// agreedWorker1: Employee
	// agreedWorker2: Employee
	// agreedWorker3: Employee
	// agreedWorker4: Employee
	// agreedWorker5: Employee
	// agreedWorker6: Employee
	// agreedWorker7: Employee
}

class Mark {
	id: number
	subnode: Subnode
	code: string
	// additionalCode: string
	// name: string
	// department: Department
	// chiefSpecialist: Employee
	// groupLeader: Employee
	// mainBulder: Employee
	// agreedWorker1: Employee
	// agreedWorker2: Employee
	// agreedWorker3: Employee
	// agreedWorker4: Employee
	// agreedWorker5: Employee
	// agreedWorker6: Employee
	// agreedWorker7: Employee

	constructor(obj?: IMark) {
		this.id = (obj && obj.id) || 0
		this.subnode = (obj && obj.subnode) || null
		this.code = (obj && obj.code) || ''
		// this.additionalCode = (obj && obj.additionalCode) || ''
		// this.name = (obj && obj.name) || ''
		// this.department = (obj && obj.department) || null
		// this.chiefSpecialist = (obj && obj.chiefSpecialist) || null
		// this.groupLeader = (obj && obj.groupLeader) || null
		// this.mainBulder = (obj && obj.mainBulder) || null
		// this.agreedWorker1 = (obj && obj.agreedWorker1) || null
		// this.agreedWorker2 = (obj && obj.agreedWorker2) || null
		// this.agreedWorker3 = (obj && obj.agreedWorker3) || null
		// this.agreedWorker4 = (obj && obj.agreedWorker4) || null
		// this.agreedWorker5 = (obj && obj.agreedWorker5) || null
		// this.agreedWorker6 = (obj && obj.agreedWorker6) || null
		// this.agreedWorker7 = (obj && obj.agreedWorker7) || null
	}
}

export default Mark
