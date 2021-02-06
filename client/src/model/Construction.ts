import ConstructionType from './ConstructionType'
import ConstructionSubtype from './ConstructionSubtype'
import WeldingControl from './WeldingControl'

interface IConstruction {
	id: number
	name: string
	type: ConstructionType
	subtype: ConstructionSubtype
	valuation: string
	standardAlbumCode: string
	numOfStandardConstructions: number
	paintworkCoeff: number
	weldingControl: WeldingControl
	hasEdgeBlunting: boolean
	hasDynamicLoad: boolean
	hasFlangedConnections: boolean
}

class Construction {
	id: number
	name: string
	type: ConstructionType
	subtype: ConstructionSubtype
	valuation: string
	standardAlbumCode: string
	numOfStandardConstructions: number
	paintworkCoeff: number
	weldingControl: WeldingControl
	hasEdgeBlunting: boolean
	hasDynamicLoad: boolean
	hasFlangedConnections: boolean

	constructor(obj?: IConstruction) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
		this.type = (obj && obj.type) || null
		this.subtype = (obj && obj.subtype) || null
		this.valuation = (obj && obj.valuation) || ''
		this.standardAlbumCode = (obj && obj.standardAlbumCode) || ''
		this.numOfStandardConstructions =
			(obj && obj.numOfStandardConstructions) || 0
		this.paintworkCoeff = (obj && obj.paintworkCoeff) || 0
		this.weldingControl = (obj && obj.weldingControl) || null
		this.hasEdgeBlunting = (obj && obj.hasEdgeBlunting) || false
		this.hasDynamicLoad = (obj && obj.hasDynamicLoad) || false
		this.hasFlangedConnections = (obj && obj.hasFlangedConnections) || false
	}
}

export default Construction
