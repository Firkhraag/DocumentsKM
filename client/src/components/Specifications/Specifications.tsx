import React, { useState, useEffect, useRef } from 'react'
import Add from '../Svg/Add'
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'
import Specification from '../../model/Specification'
import './Specifications.css'

const Specifications = () => {

    const [specList, setSpecList] = useState<Array<Specification>>([])
    const radioRef = useRef()

    useEffect(() => {
        
    }, [])

    const onCurrentTdClick = () => {
		const inputElement = radioRef.current as any
		if (inputElement) {
			inputElement.checked = true
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Выпуски спецификаций</h1>
            <span className="pointer">
                <Add />
            </span>
			<table className="spec-table white-bg">
				<tbody>
					<tr className="head-tr">
						<td>№</td>
						<td>Создан</td>
						<td className="note-cell-width">Примечание</td>
						<td>Текущий</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
					<tr>
						<td>0</td>
						<td>18.04.2017</td>
						<td className="note-cell-width">
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне. Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне. Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td onClick={onCurrentTdClick} className="pointer text-centered">
							<input
                                ref={radioRef}
								className="pointer"
								type="radio"
								id="is0"
								name="currentRelease"
							/>
						</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
						<td>1</td>
						<td>18.04.2017</td>
						<td className="note-cell-width">
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td className="pointer text-centered">
							<input
								className="pointer"
								type="radio"
								id="is1"
								name="currentRelease"
							/>
						</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
						<td>2</td>
						<td>18.04.2017</td>
						<td className="note-cell-width">
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td className="pointer text-centered">
							<input
								className="pointer"
								type="radio"
								id="is2"
								name="currentRelease"
							/>
						</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
						<td>3</td>
						<td>18.04.2017</td>
						<td className="note-cell-width">
							Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне. Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне. Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне. Lorem Ipsum - это текст-"рыба", часто используемый в
							печати и вэб-дизайне.
						</td>
						<td className="pointer text-centered">
							<input
								className="pointer"
								type="radio"
								id="is3"
								name="currentRelease"
							/>
						</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
				</tbody>
			</table>
            <button className="final-btn input-border-radius pointer">
				Сохранить изменения
			</button>
		</div>
	)
}

export default Specifications
