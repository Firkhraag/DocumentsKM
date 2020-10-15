import React from 'react'
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'
import './OtherAttachedDocs.css'

const OtherAttachedDocs = () => {
    return (
        <div className="component-cnt">
			<h1 className="text-centered">Прочие прилагаемые документы</h1>
			<table className="spec-table">
				<tbody>
					<tr className="head-tr">
						<td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
					<tr>
                        <td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
				</tbody>
			</table>
		</div>
    )
}

export default OtherAttachedDocs
