import React from 'react'
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'
import './LinkedDocs.css'

const LinkedDocs = () => {
    return (
        <div className="component-cnt">
			<h1 className="text-centered">Ссылочные документы</h1>
			<table className="spec-table">
				<tbody>
					<tr className="head-tr">
						<td>Шифр</td>
						<td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
					<tr>
                        <td>Шифр</td>
						<td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Шифр</td>
						<td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Шифр</td>
						<td>Обозначение</td>
						<td>Наименование</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                        <td>Шифр</td>
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

export default LinkedDocs
