import { makeMarkName } from '../src/util/make-name'

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
