export const removeValueFromArray = (arr: any[], value: any) => {
	const index = arr.indexOf(value)
	if (index > -1) {
		arr.splice(index, 1)
	}
	return arr
}
