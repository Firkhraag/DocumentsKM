const getNullableFieldValue = (selectedObj: any, obj: any) => {
	if (selectedObj == null) {
		if (obj == null) {
			return undefined
		}
		return -1
	}
	if (obj == null) {
		return selectedObj.id
	}
	if (selectedObj.id == obj.id) {
		return undefined
	}
	return selectedObj.id
}

export default getNullableFieldValue
