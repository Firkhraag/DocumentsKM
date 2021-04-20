export const getEmployeeShortName = (name: string) => {
	const split = name.split(' ')
    return split[0] + ' ' + split[1].charAt(0) + '. ' + split[2].charAt(0) + '.'
}
