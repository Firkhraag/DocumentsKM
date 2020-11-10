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
import AttachedDoc from '../../model/AttachedDoc'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type OtherAttachedDocsProps = {
	setPopupObj: (popupObj: IPopupObj) => void
	setOtherAttachedDoc: (s: AttachedDoc) => void
}

const OtherAttachedDocs = ({
	setPopupObj,
	setOtherAttachedDoc,
}: OtherAttachedDocsProps) => {
	const mark = useMark()
	const history = useHistory()

	const [otherAttachedDocs, setOtherAttachedDocs] = useState(
		[] as AttachedDoc[]
	)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const otherAttachedDocsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/attached-docs`
					)
					setOtherAttachedDocs(otherAttachedDocsFetchedResponse.data)
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
			otherAttachedDocs.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Прочие прилагаемые документы</h1>
			<PlusCircle
				onClick={() => history.push('/other-attached-doc-add')}
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
										setOtherAttachedDoc(d)
										history.push(
											`/other-attached-docs/${d.id}`
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
											msg: `Вы действительно хотите удалить прилагаемый документ ${d.designation}?`,
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

export default OtherAttachedDocs

// import React from 'react'
// import Edit from '../Svg/Edit'
// import Delete from '../Svg/Delete'
// import './OtherAttachedDocs.css'

// const OtherAttachedDocs = () => {
//     return (
//         <div className="component-cnt">
// 			<h1 className="text-centered">Прочие прилагаемые документы</h1>
// 			<table className="spec-table">
// 				<tbody>
// 					<tr className="head-tr">
// 						<td>Обозначение</td>
// 						<td>Наименование</td>
// 						<td>Примечание</td>
// 						<td className="text-centered" colSpan={2}>Действия</td>
// 					</tr>
// 					<tr>
//                         <td>Обозначение</td>
// 						<td>Наименование</td>
// 						<td>Примечание</td>
//                         <td className="pointer action-cell-width text-centered"><Edit /></td>
//                         <td className="pointer action-cell-width text-centered"><Delete /></td>
// 					</tr>
// 					<tr>
//                         <td>Обозначение</td>
// 						<td>Наименование</td>
// 						<td>Примечание</td>
//                         <td className="pointer action-cell-width text-centered"><Edit /></td>
//                         <td className="pointer action-cell-width text-centered"><Delete /></td>
// 					</tr>
// 					<tr>
//                         <td>Обозначение</td>
// 						<td>Наименование</td>
// 						<td>Примечание</td>
//                         <td className="pointer action-cell-width text-centered"><Edit /></td>
//                         <td className="pointer action-cell-width text-centered"><Delete /></td>
// 					</tr>
// 					<tr>
//                         <td>Обозначение</td>
// 						<td>Наименование</td>
// 						<td>Примечание</td>
//                         <td className="pointer action-cell-width text-centered"><Edit /></td>
//                         <td className="pointer action-cell-width text-centered"><Delete /></td>
// 					</tr>
// 				</tbody>
// 			</table>
// 		</div>
//     )
// }

// export default OtherAttachedDocs
