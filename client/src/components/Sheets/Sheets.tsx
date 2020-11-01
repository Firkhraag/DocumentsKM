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
import Sheet from '../../model/Sheet'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'
import './Sheet.css'

type SheetsProps = {
	setPopupObj: (popupObj: IPopupObj) => void
}

const Sheets = ({ setPopupObj }: SheetsProps) => {
    const mark = useMark()
    const history = useHistory()

	const [sheets, setSheets] = useState([] as Sheet[])

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
    
    const onDeleteClick = async (row: number, id: number) => {
		try {
            // await httpClient.delete(`/marks/${mark.id}/specifications/${id}`)
            sheets.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">Листы основного комплекта</h1>
			<PlusCircle onClick={() => history.push('/sheet-create')} color="#666" size={28} className="pointer" />
			<Table bordered hover className="mrg-top">
				<thead>
					<tr>
						<th>№</th>
						<th className="sheet-name-col-width">Наименование</th>
						<th>Формат</th>
						<th className="sheet-employee-col-width">Разработал</th>
						<th className="sheet-employee-col-width">Проверил</th>
						<th className="sheet-employee-col-width">Н.контр.</th>
						<th className="sheet-note-col-width">Примечание</th>
                        <th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{sheets.map((s, index) => {
						return (
							<tr key={s.id}>
								<td>{s.num}</td>
								<td className="sheet-name-col-width">{s.name}</td>
								<td>{s.form}</td>
								<td className="sheet-employee-col-width">{s.creator == null ? '' : s.creator.name}</td>
								<td className="sheet-employee-col-width">{s.inspector == null ? '' : s.inspector.name}</td>
								<td className="sheet-employee-col-width">{s.normController == null ? '' : s.normController.name}</td>
								<td className="sheet-note-col-width">{s.note}</td>
                                <td
									onClick={() =>
										history.push('/sheet-data')
									}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
                                            isShown: true,
                                            msg: `Вы действительно хотите удалить лист основного комплекта №${s.num}?`,
                                            onAccept: () =>
                                                onDeleteClick(
                                                    index,
                                                    s.id
                                                ),
                                            onCancel: () =>
                                                setPopupObj(
                                                    defaultPopupObj
                                                ),
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

export default Sheets
