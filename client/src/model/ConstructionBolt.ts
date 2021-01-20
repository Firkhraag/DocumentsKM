import BoltDiameter from './BoltDiameter'

interface IConstructionBolt {
	id: number
	diameter: BoltDiameter
	packet: number
	num: number
	nutNum: number
	washerNum: number
}

class ConstructionBolt {
	id: number
	diameter: BoltDiameter
	packet: number
	num: number
	nutNum: number
	washerNum: number

	constructor(obj?: IConstructionBolt) {
		this.id = (obj && obj.id) || 0
		this.diameter = (obj && obj.diameter) || null
		this.packet = (obj && obj.packet) || 0
		this.num = (obj && obj.num) || 0
		this.nutNum = (obj && obj.nutNum) || 0
		this.washerNum = (obj && obj.washerNum) || 0
	}
}

export default ConstructionBolt
