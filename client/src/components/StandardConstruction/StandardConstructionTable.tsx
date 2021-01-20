// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
import MarkLinkedDoc from '../../model/MarkLinkedDoc'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

const StandardConstructionTable = () => {
	const mark = useMark()
	const history = useHistory()

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				// try {
				// 	const linkedDocsFetchedResponse = await httpClient.get(
				// 		`/marks/${mark.id}/mark-linked-docs`
				//     )
				//     console.log(linkedDocsFetchedResponse.data)
				// 	setLinkedDocs(linkedDocsFetchedResponse.data)
				// } catch (e) {
				// 	console.log('Failed to fetch the data', e)
				// }
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			// await httpClient.delete(`/mark-linked-docs/${id}`)
			// linkedDocs.splice(row, 1)
			// setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Типовые конструкции</h1>
			<PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={null}
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>Наименование</th>
						<th>толщина пакета, мм</th>
						<th>болтов, шт.</th>
						<th>гаек на болт, шт.</th>
						<th>шайб на болт, шт.</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td>диаметр болта, мм</td>
						<td>толщина пакета, мм</td>
						<td>болтов, шт.</td>
						<td>гаек на болт, шт.</td>
						<td>шайб на болт, шт.</td>
						<td
							onClick={null}
							className="pointer action-cell-width text-centered"
						>
							<PencilSquare color="#666" size={26} />
						</td>
						<td
							onClick={null}
							className="pointer action-cell-width text-centered"
						>
							<Trash color="#666" size={26} />
						</td>
					</tr>
				</tbody>
			</Table>
		</div>
	)
}

export default StandardConstructionTable
