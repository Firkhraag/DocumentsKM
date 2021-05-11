// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import DevelopingAttachedDocData from './DevelopingAttachedDocData'
import { useMark } from '../../store/MarkStore'
import Doc from '../../model/Doc'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'
import { getEmployeeShortName } from '../../util/get-employee-short-name'

const DevelopingAttachedDocTable = () => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [docs, setDocs] = useState([] as Doc[])
	const [docData, setDocData] = useState({
		isCreateMode: false,
		doc: null,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const docsResponse = await httpClient.get(
						`/marks/${mark.id}/docs/attached`
					)
					setDocs(docsResponse.data)
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
            var arr = [...docs]
			arr.splice(row, 1)
			setDocs(arr)
			setPopup(defaultPopup)
			setDocData({
				doc: null,
				isCreateMode: false,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">
				Разрабатываемые прилагаемые документы
			</h1>
			{docData.isCreateMode || docData.doc != null ? <DevelopingAttachedDocData 
				docData={docData}
				setDocData={setDocData}
				docs={docs}
				setDocs={setDocs} /> : null}
			<PlusCircle
				onClick={() => {
					if (docs.length > 0) {
						const lastDoc = docs[docs.length - 1]
						setDocData({
							isCreateMode: true,
							doc: {
								id: -1,
								num: 1,
								numOfPages: 1,
								form: 0.125,
								name: '',
								type: null,
								creator: lastDoc.creator,
								inspector: lastDoc.inspector,
								normContr: lastDoc.normContr,
								releaseNum: 0,
								note: '',
							},
						})
					} else {
						setDocData({
							isCreateMode: true,
							doc: null,
						})
					}
					// setDocData({
					// 	isCreateMode: true,
					// 	doc: null,
					// })
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
						<th>Шифр</th>
						<th>Наименование</th>
						<th>Листов</th>
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
								<td>{index + 1}</td>
								<td>{d.type.code}</td>
								<td>{d.name}</td>
								<td>{d.numOfPages}</td>
								<td>{d.form}</td>
								<td>
									{d.creator == null ? '' : getEmployeeShortName(d.creator.fullname)}
								</td>
								<td>
									{d.inspector == null
										? ''
										: getEmployeeShortName(d.inspector.fullname)}
								</td>
								<td>
									{d.normContr == null
										? ''
										: getEmployeeShortName(d.normContr.fullname)}
								</td>
								<td>{d.note}</td>
								<td
									onClick={() => {
										setDocData({
											isCreateMode: false,
											doc: d,
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

export default DevelopingAttachedDocTable
