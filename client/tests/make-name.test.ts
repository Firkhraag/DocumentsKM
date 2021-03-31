import { makeComplexAndObjectName, makeMarkName } from '../src/util/make-name'

describe('make the right mark name', () => {
	test.each`
		id   | projectBaseSeries | nodeCode    | subnodeCode | markCode | expected
		${1} | ${'М31208'}       | ${'01- 3К'} | ${'07'}     | ${'КМ'}  | ${'М31208.01.07- 3К-КМ'}
        ${2} | ${'М31208'}       | ${'-'}      | ${'07'}     | ${'КМ'}  | ${'М31208.07-КМ'}
        ${2} | ${'М31208'}       | ${'01- 3К'} | ${'07'}     | ${''}    | ${'М31208.01.07- 3К'}
	`(
		'should return $expected if input id is $id',
		({ projectBaseSeries, nodeCode, subnodeCode, markCode, expected }) => {
			expect(
				makeMarkName(projectBaseSeries, nodeCode, subnodeCode, markCode)
			).toBe(expected)
		}
	)
})

describe('make the right complex name', () => {
	test.each`
		id   | projectName 							 | nodeName    					| subnodeName | markName   | bias  | expected
		${1} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${0}  | ${'ОАО "ММК". Доменный цех. Печь №8'}
        ${2} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${25} | ${'ОАО "ММК". Доменный цех. Печь №8. Аспирационная установка'}
        ${3} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${-7} | ${'ОАО "ММК". Доменный цех'}
	`(
		'should return $expected if input id is $id',
		({ projectName, nodeName, subnodeName, markName, bias, expected }) => {
			expect(
				makeComplexAndObjectName(projectName, nodeName, subnodeName, markName, bias).complexName
			).toBe(expected)
		}
	)
})

describe('make the right object name', () => {
	test.each`
		id   | projectName 							 | nodeName    					| subnodeName | markName   | bias  | expected
		${1} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${0}  | ${'Аспирационная установка. Тест'}
        ${2} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${25} | ${'Тест'}
        ${3} | ${'ОАО "ММК". Доменный цех. Печь №8'} | ${'Аспирационная установка'} | ${''}       | ${'Тест'}  | ${-7} | ${'Печь №8. Аспирационная установка. Тест'}
	`(
		'should return $expected if input id is $id',
		({ projectName, nodeName, subnodeName, markName, bias, expected }) => {
			expect(
				makeComplexAndObjectName(projectName, nodeName, subnodeName, markName, bias).objectName
			).toBe(expected)
		}
	)
})
