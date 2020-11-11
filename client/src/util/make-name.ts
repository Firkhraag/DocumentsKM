// Создание обозначения марки или подузла
export const makeMarkName = (
	projectBaseSeries: string,
	nodeCode: string,
	subnodeCode: string,
	markCode: string
) => {
	let markName = projectBaseSeries

	let overhaul = ''
	if (nodeCode !== '-' && nodeCode !== '') {
		const nodeCodeSplitted = nodeCode.split('-')
		const nodeValue = nodeCodeSplitted[0]
		if (nodeCodeSplitted.length === 2) {
			overhaul = nodeCodeSplitted[1]
		}

		markName += `.${nodeValue}`
	}
	if (subnodeCode !== '-' && subnodeCode !== '') {
		markName += `.${subnodeCode}`
		if (overhaul !== '') {
			markName += `-${overhaul}`
		}
	}
	if (markCode !== '-' && markCode !== '') {
		markName += `-${markCode}`
	}
	return markName
}

// Создание основной надписи
export const makeComplexAndObjectName = (
	projectName: string,
	nodeName: string,
	subnodeName: string,
	markName: string
) => {
	// Стандартный случай
	let complexName = projectName
	let objectName = nodeName + '. ' + subnodeName + '. ' + markName

	return { complexName, objectName }
}
