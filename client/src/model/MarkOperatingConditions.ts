import EnvAggressiveness from './EnvAggressiveness'
import OperatingArea from './OperatingArea'
import GasGroup from './GasGroup'
import ConstructionMaterial from './ConstructionMaterial'
import PaintworkType from './PaintworkType'
import HighTensileBoltsType from './HighTensileBoltsType'

interface IMarkOperatingConditions {
    safetyCoeff: number
    envAggressiveness: EnvAggressiveness
    temperature: number
    operatingArea: OperatingArea
    gasGroup: GasGroup
    constructionMaterial: ConstructionMaterial
    paintworkType: PaintworkType
    highTensileBoltsType: HighTensileBoltsType
}

class MarkOperatingConditions {
    safetyCoeff: number
    envAggressiveness: EnvAggressiveness
    temperature: number
    operatingArea: OperatingArea
    gasGroup: GasGroup
    constructionMaterial: ConstructionMaterial
    paintworkType: PaintworkType
    highTensileBoltsType: HighTensileBoltsType

	constructor(obj?: IMarkOperatingConditions) {
        this.safetyCoeff = (obj && obj.safetyCoeff) || 0
        this.envAggressiveness = (obj && obj.envAggressiveness) || null
        this.temperature = (obj && obj.temperature) || 0
        this.operatingArea = (obj && obj.operatingArea) || null
        this.gasGroup = (obj && obj.gasGroup) || null
        this.constructionMaterial = (obj && obj.constructionMaterial) || null
        this.paintworkType = (obj && obj.paintworkType) || null
        this.highTensileBoltsType = (obj && obj.highTensileBoltsType) || null
    }
}

export default MarkOperatingConditions
