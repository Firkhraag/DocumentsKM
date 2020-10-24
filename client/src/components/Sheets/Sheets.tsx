// Global
import React, { useState, useEffect, useRef } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
import Sheet from '../../model/Sheet'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

const Sheets = () => {
	const mark = useMark()
	const history = useHistory()

	const [sheets, setSheets] = useState([] as Sheet[])

	const radioRef = useRef()

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sheetsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/sheets`
					)
					setSheets(sheetsFetchedResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Листы основного комплекта</h1>
			<PlusCircle color="#666" size={28} className="pointer" />
			<Table striped bordered hover className="mrg-top">
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
					{sheets.map((s) => {
						return (
							<tr key={s.id}>
								<td>{s.number}</td>
								<td>{s.name}</td>
								<td>{s.format}</td>
								<td>{s.creator == null ? '' : s.creator.fullName}</td>
								<td>{s.inspector == null ? '' : s.inspector.fullName}</td>
								<td>{s.normController == null ? '' : s.normController.fullName}</td>
								<td>{s.note}</td>
							</tr>
						)
					})}
				</tbody>
			</Table>
		</div>
	)
}

export default Sheets
