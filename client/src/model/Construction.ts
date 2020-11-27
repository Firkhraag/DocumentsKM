import ConstructionType from './ConstructionType'
import ConstructionSubtype from './ConstructionSubtype'
import WeldingControl from './WeldingControl'

interface IConstruction {
	id: number
    name: string
    type: ConstructionType
    subtype: ConstructionSubtype
    valuation: string
    weldingControl: WeldingControl
}

class Construction {
	id: number
    name: string
    type: ConstructionType
    subtype: ConstructionSubtype
    valuation: string
    weldingControl: WeldingControl

	constructor(obj?: IConstruction) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.type = (obj && obj.type) || null
        this.subtype = (obj && obj.subtype) || null
        this.valuation = (obj && obj.valuation) || ''
		this.weldingControl = (obj && obj.weldingControl) || null
	}
}

export default Construction
