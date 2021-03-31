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
	markName: string,
	bias: number
) => {
	let complexName = projectName
	let objectName = ''
	let firstPartAdded = false
	if (nodeName !== '' && nodeName != null) {
		objectName += nodeName
		firstPartAdded = true
	}
	if (subnodeName !== '' && subnodeName != null) {
		if (firstPartAdded) {
			objectName += '. '
		} else {
			firstPartAdded = true
		}
		objectName += subnodeName
	}
	if (markName !== '' && markName != null) {
		if (firstPartAdded) {
			objectName += '. '
		}
		objectName += markName
	}
	if (bias > 0) {
		complexName = projectName + '. ' + objectName.substring(0, bias -2)
		objectName = objectName.substring(bias, objectName.length)
	} else if (bias < 0) {
		complexName = projectName.substring(0, projectName.length + bias - 2)
		objectName = projectName.substring(projectName.length + bias) + '. ' + objectName
	}

	return { complexName, objectName }
}
