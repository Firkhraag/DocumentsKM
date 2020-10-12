const getFromOptions = (id: number, arr: any[], selectedObj: any, isDepartment: boolean = false) => {
	let e: any = null
	for (let el of arr) {
        if (isDepartment) {
            if (el.number === id) {
                e = el
                break
            }
        } else {
            if (el.id === id) {
                e = el
                break
            }
        }
    }
    if (isDepartment) {
        if (e != null && selectedObj != null && selectedObj.number === e.number) {
            return null
        }
    } else {
        if (e != null && selectedObj != null && selectedObj.id === e.id) {
            return null
        }
    }
	return e
}

export default getFromOptions
