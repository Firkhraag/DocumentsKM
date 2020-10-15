// Global
import React from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
// Components
import Edit from '../Svg/Edit'
import Delete from '../Svg/Delete'

const Sheets = () => {
    return (
        <div className="component-cnt">
			<h1 className="text-centered">Листы основного комплекта</h1>
            <PlusCircle color="#666" size={28} className="pointer" />
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>№</th>
						<th>Наименование</th>
						<th>Формат</th>
						<th>Разработал</th>
						<th>Проверил</th>
						<th>Нормоконтролер</th>
						<th>Примечание</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
						<td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
					</tr>
                    <tr>
						<td>№</td>
						<td>Наименование</td>
						<td>Формат</td>
						<td>Разработал</td>
						<td>Проверил</td>
						<td>Нормоконтролер</td>
						<td>Примечание</td>
					</tr>
                </tbody>
            </Table>
		</div>
    )
}

export default Sheets
