// Global
import React, { useState, useEffect, useRef } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
// Util
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'
import Specification from '../../model/Specification'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'
import './Specifications.css'

type SpecificationsProps = {
    setPopupObj: (popupObj: IPopupObj) => void
}

const Specifications = ({ setPopupObj }: SpecificationsProps) => {

    const [specList, setSpecList] = useState<Array<Specification>>([])
    const radioRef = useRef()

    const history = useHistory()

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
            <PlusCircle color="#666" size={28} className="pointer" />
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>№</th>
						<th>Создан</th>
						<th className="note-cell-width">Примечание</th>
						<th>Текущий</th>
						<th className="text-centered" colSpan={2}>Действия</th>
                    </tr>
                </thead>
                <tbody>
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
                        <td onClick={() => history.push('/specification-data')} className="pointer action-cell-width text-centered"><Edit /></td>
                        <td onClick={() => setPopupObj({
                            isShown: true,
                            msg: 'Вы действительно хотите удалить выпуск спецификации №0?',
                            onAccept: () => setPopupObj(defaultPopupObj),
                            onCancel: () => setPopupObj(defaultPopupObj),
                        })} className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
                </tbody>
            </Table>
		</div>
	)
}

export default Specifications
