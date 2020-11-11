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
import Doc from '../../model/Doc'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type DevelopingAttachedDocTableProps = {
	setPopupObj: (popupObj: IPopupObj) => void
	setDevelopingAttachedDoc: (d: Doc) => void
}

const DevelopingAttachedDocTable = ({
	setPopupObj,
	setDevelopingAttachedDoc,
}: DevelopingAttachedDocTableProps) => {
	const mark = useMark()
	const history = useHistory()

	const [docs, setDocs] = useState([] as Doc[])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const docsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/docs/attached`
					)
					setDocs(docsFetchedResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/docs/${id}`)
			docs.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">
				Разрабатываемые прилагаемые документы
			</h1>
			<PlusCircle
				onClick={() => history.push('/developing-attached-doc-create')}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th>Наименование</th>
						<th>Формат</th>
						<th>Разработал</th>
						<th>Проверил</th>
						<th>Н.контр.</th>
						<th>Примечание</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{docs.map((d, index) => {
						return (
							<tr key={d.id}>
								<td>{d.num}</td>
								<td>{d.name}</td>
								<td>{d.form}</td>
								<td>
									{d.creator == null ? '' : d.creator.name}
								</td>
								<td>
									{d.inspector == null
										? ''
										: d.inspector.name}
								</td>
								<td>
									{d.normContr == null
										? ''
										: d.normContr.name}
								</td>
								<td>{d.note}</td>
								<td
									onClick={() => {
										setDevelopingAttachedDoc(d)
										history.push(
											`/developing-attached-docs/${d.id}`
										)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
											isShown: true,
											msg: `Вы действительно хотите удалить прилагаемый документ №${d.num}?`,
											onAccept: () =>
												onDeleteClick(index, d.id),
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

export default DevelopingAttachedDocTable
