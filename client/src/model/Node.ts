interface INode {    
    series: string
    node: string
    subnode: string
    mark: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
}

class Node {
    series: string
    node: string
    subnode: string
    mark: string

    // Others
    gipSurname: string
    facilityName: string
    objectName: string
    
    constructor(obj?: INode) {    
        this.series = obj && obj.series || ''
        this.node = obj && obj.node || ''
        this.subnode = obj && obj.subnode || ''
        this.mark = obj && obj.mark || ''

        // Others
        this.gipSurname = obj && obj.gipSurname || ''
        this.facilityName = obj && obj.facilityName || ''
        this.objectName = obj && obj.objectName || ''
    }   
}

export default Node
