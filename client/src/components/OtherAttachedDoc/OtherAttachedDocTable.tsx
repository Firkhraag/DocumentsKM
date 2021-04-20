// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import OtherAttachedDocData from './OtherAttachedDocData'
import { useMark } from '../../store/MarkStore'
import AttachedDoc from '../../model/AttachedDoc'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

const OtherAttachedDocTable = () => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [otherAttachedDocs, setOtherAttachedDocs] = useState(
		[] as AttachedDoc[]
	)
	const [otherAttachedDocData, setOtherAttachedDocData] = useState({
		isCreateMode: false,
		otherAttachedDoc: null,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const otherAttachedDocsResponse = await httpClient.get(
						`/marks/${mark.id}/attached-docs`
					)
					setOtherAttachedDocs(otherAttachedDocsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/attached-docs/${id}`)
            var arr = [...otherAttachedDocs]
			arr.splice(row, 1)
			setOtherAttachedDocs(arr)
			setPopup(defaultPopup)
			setOtherAttachedDocData({
				otherAttachedDoc: null,
				isCreateMode: false,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Прочие прилагаемые документы</h1>
			{otherAttachedDocData.isCreateMode || otherAttachedDocData.otherAttachedDoc != null ? <OtherAttachedDocData 
				otherAttachedDocData={otherAttachedDocData}
				setOtherAttachedDocData={setOtherAttachedDocData}
				otherAttachedDocs={otherAttachedDocs}
				setOtherAttachedDocs={setOtherAttachedDocs} /> : null}
			<PlusCircle
				onClick={() => {
					setOtherAttachedDocData({
						isCreateMode: true,
						otherAttachedDoc: null,
					})
					window.scrollTo(0, 0)
				}}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th>Обозначение</th>
						<th className="doc-note-col-width">Наименование</th>
						<th className="doc-note-col-width">Примечание</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{otherAttachedDocs.map((d, index) => {
						return (
							<tr key={d.id}>
								<td>{index + 1}</td>
								<td>{d.designation}</td>
								<td className="doc-note-col-width">{d.name}</td>
								<td className="doc-note-col-width">{d.note}</td>
								<td
									onClick={() => {
										setOtherAttachedDocData({
											isCreateMode: false,
											otherAttachedDoc: d,
										})
										window.scrollTo(0, 0)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopup({
											isShown: true,
											msg: `Вы действительно хотите удалить прилагаемый документ № ${
												index + 1
											}?`,
											onAccept: () =>
												onDeleteClick(index, d.id),
											onCancel: () =>
												setPopup(defaultPopup),
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

export default OtherAttachedDocTable
