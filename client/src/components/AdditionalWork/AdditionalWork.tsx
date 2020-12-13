// // Global
// import React, { useState, useEffect } from 'react'
// import { useHistory } from 'react-router-dom'
// // Bootstrap
// import Table from 'react-bootstrap/Table'
// import { PlusCircle } from 'react-bootstrap-icons'
// import { PencilSquare } from 'react-bootstrap-icons'
// import { Trash } from 'react-bootstrap-icons'
// // Util
// import httpClient from '../../axios'
// import { useMark } from '../../store/MarkStore'
// import Doc from '../../model/Doc'
// import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

// type AdditionalWorkProps = {
// }

// const AdditionalWork = ({  }: AdditionalWorkProps) => {
// 	const mark = useMark()
// 	const history = useHistory()

// 	useEffect(() => {
// 		if (mark != null && mark.id != null) {
// 			const fetchData = async () => {
// 				try {
// 					const sheetsFetchedResponse = await httpClient.get(
// 						`/marks/${mark.id}/docs/sheets`
// 					)
// 					setSheets(sheetsFetchedResponse.data)
// 				} catch (e) {
// 					console.log('Failed to fetch the data', e)
// 				}
// 			}
// 			fetchData()
// 		}
// 	}, [mark])

// 	return (
// 		<div className="component-cnt table-width">
// 			<h1 className="text-centered">Учет дополнительных проектных работ</h1>
// 			<PlusCircle
// 				onClick={() => history.push('/sheet-create')}
// 				color="#666"
// 				size={28}
// 				className="pointer"
// 			/>
// 			<Table bordered striped className="mrg-top no-bot-mrg">
// 				<thead>
// 					<tr>
// 						<th className="doc-name-col-width">Исполнитель</th>
// 						<th>лист, a4</th>
// 						<th>прив. к A1</th>
// 						<th>строк</th>
// 						<th>прив. к A1</th>
// 						<th>Выполнено чертежей</th>
// 						<th>выполнено</th>
//                         <th>прив. к A1</th>
// 						<th className="text-centered" colSpan={2}>
// 							Действия
// 						</th>
// 					</tr>
// 				</thead>
// 				<tbody>
// 					{sheets.map((s, index) => {
// 						return (
// 							<tr key={s.id}>
// 								<td className="doc-name-col-width">{s.name}</td>
// 								<td>{s.form}</td>
// 								<td className="doc-employee-col-width">
// 									{s.creator == null ? '' : s.creator.name}
// 								</td>
// 								<td className="doc-employee-col-width">
// 									{s.inspector == null
// 										? ''
// 										: s.inspector.name}
// 								</td>
// 								<td className="doc-employee-col-width">
// 									{s.normContr == null
// 										? ''
// 										: s.normContr.name}
// 								</td>
// 								<td className="doc-note-col-width">{s.note}</td>
// 								<td
// 									onClick={() => {
// 										setSheet(s)
// 										history.push(`/sheets/${s.id}`)
// 									}}
// 									className="pointer action-cell-width text-centered"
// 								>
// 									<PencilSquare color="#666" size={26} />
// 								</td>
// 								<td
// 									onClick={() =>
// 										setPopupObj({
// 											isShown: true,
// 											msg: `Вы действительно хотите удалить лист основного комплекта №${s.num}?`,
// 											onAccept: () =>
// 												onDeleteClick(index, s.id),
// 											onCancel: () =>
// 												setPopupObj(defaultPopupObj),
// 										})
// 									}
// 									className="pointer action-cell-width text-centered"
// 								>
// 									<Trash color="#666" size={26} />
// 								</td>
// 							</tr>
// 						)
// 					})}
// 				</tbody>
// 			</Table>
// 		</div>
// 	)
// }

// export default AdditionalWork
