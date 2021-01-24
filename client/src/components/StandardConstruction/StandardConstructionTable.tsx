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
		<div>
			<h2 className="mrg-top-2 bold text-centered">
				Типовые конструкции
			</h2>

			<div className="full-width">
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
							<th>Количество, шт.</th>
							<th>№ листа</th>
							<th>Вес, т</th>
							<th className="text-centered" colSpan={2}>
								Действия
							</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>1</td>
							<td>2</td>
							<td>3</td>
							<td>4</td>
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
		</div>
	)
}

export default StandardConstructionTable
