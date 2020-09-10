interface IMark {    
    series: string
    node: string
    subnode: string
    code: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
}

class Mark {
    series: string
    node: string
    subnode: string
    code: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
    
    constructor(obj?: IMark) {    
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

export default Mark
