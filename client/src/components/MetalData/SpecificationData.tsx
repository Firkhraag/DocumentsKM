import React from 'react'
import './SpecificationData.css'

const SpecificationData = () => {
	return (
		<div className="spec-data-cnt">
			<h1 className="text-centered">Выпуски спецификаций</h1>
			<table>
				<tbody>
					<tr className="head-tr">
						<td>Выпуск</td>
						<td>Дата создания</td>
						<td>Примечание</td>
						<td>Текущий</td>
						<td>Действие</td>
					</tr>
					<tr>
						<td>0</td>
						<td>18.04.2017</td>
						<td>
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td>
							<input
								className="pointer"
								type="radio"
								id="is0"
								name="currentRelease"
							/>
						</td>
                        <td>0</td>
					</tr>
					<tr>
						<td>1</td>
						<td>18.04.2017</td>
						<td>
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td>
							<input
								className="pointer"
								type="radio"
								id="is1"
								name="currentRelease"
							/>
						</td>
                        <td>0</td>
					</tr>
					<tr>
						<td>2</td>
						<td>18.04.2017</td>
						<td>
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td>
							<input
								className="pointer"
								type="radio"
								id="is2"
								name="currentRelease"
							/>
						</td>
                        <td>0</td>
					</tr>
					<tr>
						<td>3</td>
						<td>18.04.2017</td>
						<td>
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td>
							<input
								className="pointer"
								type="radio"
								id="is3"
								name="currentRelease"
							/>
						</td>
                        <td>0</td>
					</tr>
				</tbody>
			</table>
		</div>
	)
}

export default SpecificationData
