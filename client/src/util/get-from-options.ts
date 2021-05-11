const getFromOptions = (id: number, arr: any[], selectedObj: any) => {
	let e: any = null
	for (let el of arr) {
		if (el == null)
			continue
		if (el.id === id) {
			e = el
			break
		}
	}
	if (e != null && selectedObj != null && selectedObj.id === e.id) {
		return null
	}
	return e
}

export default getFromOptions
