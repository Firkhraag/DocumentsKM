import React from 'react'
import Add from '../Svg/Add'
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'
import './Sheets.css'

const Sheets = () => {
    return (
        <div className="component-cnt">
			<h1 className="text-centered">Листы основного комплекта</h1>
            <span className="pointer">
                <Add />
            </span>
			<table className="spec-table white-bg">
				<tbody>
					<tr className="head-tr">
						<td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
					<tr>
                    <td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                    <td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                    <td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
					<tr>
                    <td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
                        <td className="pointer action-cell-width text-centered"><Edit /></td>
                        <td className="pointer action-cell-width text-centered"><Delete /></td>
					</tr>
				</tbody>
			</table>
		</div>
    )
}

export default Sheets
