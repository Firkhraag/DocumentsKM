import React from 'react'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
import './AttachedDocs.css'

const AttachedDocs = () => {
    return (
        <div className="component-cnt">
			<h1 className="text-centered">Разрабатываемые прилагаемые документы</h1>
			<table className="spec-table white-bg">
				<tbody>
					<tr className="head-tr">
						<td>№</td>
						<td>Шифр</td>
						<td>Выпуск</td>
						<td>Название</td>
						<td>Листов</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
					<tr>
						<td>№</td>
						<td>Шифр</td>
						<td>Выпуск</td>
						<td>Название</td>
						<td>Листов</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><PencilSquare /></td>
                        <td className="pointer action-cell-width text-centered"><Trash /></td>
					</tr>
					<tr>
						<td>№</td>
						<td>Шифр</td>
						<td>Выпуск</td>
						<td>Название</td>
						<td>Листов</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><PencilSquare /></td>
                        <td className="pointer action-cell-width text-centered"><Trash /></td>
					</tr>
				</tbody>
			</table>
		</div>
    )
}

export default AttachedDocs
