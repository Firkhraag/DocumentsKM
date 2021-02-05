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
import ConstructionBolt from '../../model/ConstructionBolt'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type ConstructionBoltTableProps = {
	setConstructionBolt: (cb: ConstructionBolt) => void
	specificationId: number
	constructionId: number
}

const ConstructionBoltTable = ({
	setConstructionBolt,
	specificationId,
	constructionId,
}: ConstructionBoltTableProps) => {
	const mark = useMark()
	const history = useHistory()
	const setPopup = useSetPopup()

	const [constructionBolts, setConstructionBolts] = useState(
		[] as ConstructionBolt[]
	)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const constructionBoltsResponse = await httpClient.get(
						`/constructions/${constructionId}/bolts`
					)
					setConstructionBolts(constructionBoltsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/construction-bolts/${id}`)
			constructionBolts.splice(row, 1)
			setPopup(defaultPopup)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
        <div>
			<h2 className="bold text-centered">Высокопрочные болты</h2>
			<PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={() =>
					history.push(
						`/specifications/${specificationId}/constructions/${constructionId}/bolt-create`
					)
				}
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
                        <th>№</th>
						<th>диаметр болта, мм</th>
						<th>толщина пакета, мм</th>
						<th>болтов, шт.</th>
						<th>гаек на болт, шт.</th>
						<th>шайб на болт, шт.</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{constructionBolts.map((cb, index) => {
						return (
							<tr key={index}>
                                <td>{index + 1}</td>
								<td>{cb.diameter.diameter}</td>
								<td>{cb.packet}</td>
								<td>{cb.num}</td>
								<td>{cb.nutNum}</td>
								<td>{cb.washerNum}</td>
								<td
									onClick={() => {
										setConstructionBolt(cb)
										history.push(
											`/specifications/${specificationId}/constructions/${constructionId}/bolts/${cb.id}`
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
											msg: `Вы действительно хотите удалить выскопрочный болт № ${index + 1}?`,
											onAccept: () =>
												onDeleteClick(index, cb.id),
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

export default ConstructionBoltTable