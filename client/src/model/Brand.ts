interface IBrand {    
    series: string
    node: string
    subnode: string
    code: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
}

class Brand {
    series: string
    node: string
    subnode: string
    code: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
    
    constructor(obj?: IBrand) {    
        this.series = obj && obj.series || ''
        this.node = obj && obj.node || ''
        this.subnode = obj && obj.subnode || ''
        this.code = obj && obj.code || ''

        // Others
        this.gipSurname = obj && obj.gipSurname || ''
        this.facilityName = obj && obj.facilityName || ''
        this.objectName = obj && obj.objectName || ''
    }   
}

export default Brand
