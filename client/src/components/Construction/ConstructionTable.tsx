// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
import { ClipboardPlus } from 'react-bootstrap-icons'
import { Files } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
import Construction from '../../model/Construction'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type ConstructionTableProps = {
	setConstruction: (c: Construction) => void
	specificationId: number
}

const ConstructionTable = ({
	setConstruction,
	specificationId,
}: ConstructionTableProps) => {
	const mark = useMark()
	const history = useHistory()
	const setPopup = useSetPopup()

	const [constructions, setConstructions] = useState([] as Construction[])

    const [copiedConstructionId, setCopiedConstructionId] = useState<number>(null)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (specificationId == -1) {
				history.push('/specifications')
				return
			}
			const fetchData = async () => {
				try {
					const constructionResponse = await httpClient.get(
						`/specifications/${specificationId}/constructions`
					)
					setConstructions(constructionResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/constructions/${id}`)
			constructions.splice(row, 1)
			setPopup(defaultPopup)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div>
			<h2 className="bold text-centered">Перечень видов конструкций</h2>

			<div className="full-width">
				<PlusCircle
					onClick={() =>
						history.push(
							`/specifications/${specificationId}/construction-create`
						)
					}
					color="#666"
					size={28}
					className="pointer mrg-top"
				/>
                <ClipboardPlus
					color={copiedConstructionId == null ? "#ccc" : "#666"}
					size={28}
					className={copiedConstructionId == null ? "mrg-top" : "pointer mrg-top"}
                    style={{marginLeft: 10}}
				/>
			</div>

			<Table bordered striped className="mrg-top no-bot-mrg shadow">
				<thead>
					<tr>
						<th>№</th>
						<th className="construction-name-col-width">
							Вид конструкции
						</th>
                        <th>Шифр</th>
						<th className="text-centered" colSpan={3}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{constructions.map((c, index) => {
						return (
							<tr key={index}>
								<td>{index + 1}</td>
								<td className="construction-name-col-width">
									{c.name}
								</td>
                                <td>{c.type.id}</td>
								<td
									onClick={() => {
										setConstruction(c)
										history.push(
											`/specifications/${specificationId}/constructions/${c.id}`
										)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopup({
											isShown: true,
											msg: `Вы действительно хотите удалить вид конструкции № ${index + 1}?`,
											onAccept: () =>
												onDeleteClick(index, c.id),
											onCancel: () =>
												setPopup(defaultPopup),
										})
									}
									className="pointer action-cell-width text-centered"
								>
									<Trash color="#666" size={26} />
								</td>
                                <td
                                    onClick = {() => setCopiedConstructionId(c.id)}
									className="pointer action-cell-width text-centered"
								>
									<Files color="#666" size={26} />
								</td>
							</tr>
						)
					})}
				</tbody>
			</Table>
		</div>
	)
}

export default ConstructionTable
