interface IBoltDiameter {
	id: number
	diameter: number
	nutWeight: number
	washerSteel: string
	washerWeight: number
	washerThickness: number
	boltTechSpec: string
	strengthClass: string
	nutTechSpec: string
	washerTechSpec: string
}

class BoltDiameter {
	id: number
	diameter: number
	nutWeight: number
	washerSteel: string
	washerWeight: number
	washerThickness: number
	boltTechSpec: string
	strengthClass: string
	nutTechSpec: string
	washerTechSpec: string

	constructor(obj?: IBoltDiameter) {
		this.id = (obj && obj.id) || 0
		this.diameter = (obj && obj.diameter) || 0
		this.nutWeight = (obj && obj.nutWeight) || 0
		this.washerSteel = (obj && obj.washerSteel) || ''
		this.washerWeight = (obj && obj.washerWeight) || 0
		this.washerThickness = (obj && obj.washerThickness) || 0
		this.boltTechSpec = (obj && obj.boltTechSpec) || ''
		this.strengthClass = (obj && obj.strengthClass) || ''
		this.nutTechSpec = (obj && obj.nutTechSpec) || ''
		this.washerTechSpec = (obj && obj.washerTechSpec) || ''
	}
}

export default BoltDiameter
