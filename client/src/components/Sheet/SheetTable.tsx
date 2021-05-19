// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import SheetData from './SheetData'
import { useMark } from '../../store/MarkStore'
import Doc from '../../model/Doc'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'
import { getEmployeeShortName } from '../../util/get-employee-short-name'

const SheetTable = () => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [sheets, setSheets] = useState([] as Doc[])
	const [sheetData, setSheetData] = useState({
		isCreateMode: false,
		sheet: null,
		index: -1,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const sheetsResponse = await httpClient.get(
						`/marks/${mark.id}/docs/sheets`
					)
					setSheets(sheetsResponse.data)
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
            var arr = [...sheets]
			arr.splice(row, 1)
			setSheets(arr)
			setPopup(defaultPopup)
			setSheetData({
				sheet: null,
				isCreateMode: false,
				index: -1,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">Листы основного комплекта</h1>
			{sheetData.isCreateMode || sheetData.sheet != null ? <SheetData 
				sheetData={sheetData}
				setSheetData={setSheetData}
				sheets={sheets}
				setSheets={setSheets} /> : null}
			<PlusCircle
				onClick={() => {
					if (sheets.length > 0) {
						const lastSheet = sheets[sheets.length - 1]
						setSheetData({
							isCreateMode: true,
							sheet: {
								id: -1,
								num: 1,
								form: 1.0,
								name: '',
								type: null,
								creator: lastSheet.creator,
								inspector: lastSheet.inspector,
								normContr: lastSheet.normContr,
								releaseNum: 0,
								numOfPages: 0,
								note: '',
							},
							index: -1,
						})
					} else {
						setSheetData({
							isCreateMode: true,
							sheet: null,
							index: -1,
						})
					}
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
						<th className="doc-name-col-width">Наименование</th>
						<th>Формат</th>
						<th className="doc-employee-col-width">Разработал</th>
						<th className="doc-employee-col-width">Проверил</th>
						<th className="doc-employee-col-width">Н.контр.</th>
						<th className="doc-note-col-width">Примечание</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{sheets.map((s, index) => {
						return (
							<tr key={s.id}>
								<td>{index + 1}</td>
								<td className="doc-name-col-width">{s.name}</td>
								<td>{s.form}</td>
								<td className="doc-employee-col-width">
									{getEmployeeShortName(s.creator.fullname)}
								</td>
								<td className="doc-employee-col-width">
									{s.inspector == null
										? ''
										: getEmployeeShortName(s.inspector.fullname)}
								</td>
								<td className="doc-employee-col-width">
									{s.normContr == null
										? ''
										: getEmployeeShortName(s.normContr.fullname)}
								</td>
								<td className="doc-note-col-width">{s.note}</td>
								<td
									onClick={() => {
										setSheetData({
											isCreateMode: false,
											sheet: s,
											index: index + 1,
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
											msg: `Вы действительно хотите удалить лист основного комплекта № ${index + 1}?`,
											onAccept: () =>
												onDeleteClick(index, s.id),
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

export default SheetTable
