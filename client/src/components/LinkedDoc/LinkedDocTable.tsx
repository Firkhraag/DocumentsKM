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
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type LinkedDocTableProps = {
	setPopupObj: (popupObj: IPopupObj) => void
	setMarkLinkedDoc: (mld: MarkLinkedDoc) => void
}

const LinkedDocTable = ({
	setPopupObj,
	setMarkLinkedDoc,
}: LinkedDocTableProps) => {
	const mark = useMark()
	const history = useHistory()
	const [linkedDocs, setLinkedDocs] = useState([] as MarkLinkedDoc[])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const linkedDocsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/mark-linked-docs`
					)
					setLinkedDocs(linkedDocsFetchedResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/mark-linked-docs/${id}`)
			linkedDocs.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Ссылочные документы</h1>
			<PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={() => history.push('/linked-doc-add')}
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th>Шифр</th>
						<th>Обозначение</th>
						<th className="linked-doc-name-col-width">
							Наименование
						</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{linkedDocs.map((mld, index) => {
						const ld = mld.linkedDoc
						return (
							<tr key={index}>
								<td>{index + 1}</td>
								<td>{ld.code}</td>
								<td>{ld.designation}</td>
								<td className="linked-doc-name-col-width">
									{ld.name}
								</td>
								<td
									onClick={() => {
										setMarkLinkedDoc(mld)
										history.push(`/linked-docs/${mld.id}`)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
											isShown: true,
											msg: `Вы действительно хотите удалить ссылочный документ ${ld.code}?`,
											onAccept: () =>
												onDeleteClick(index, mld.id),
											onCancel: () =>
												setPopupObj(defaultPopupObj),
										})
									}
									className="pointer action-cell-width text-centered"
								>
									<Trash color="#666" size={26} />
								</td>
							</tr>
						)
					})}
				</tbody>
			</Table>
		</div>
	)
}

export default LinkedDocTable
