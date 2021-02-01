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
import ConstructionElement from '../../model/ConstructionElement'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type ConstructionElementTableProps = {
	setConstructionElement: (cb: ConstructionElement) => void
	specificationId: number
	constructionId: number
}

const ConstructionElementTable = ({
	setConstructionElement,
	specificationId,
	constructionId,
}: ConstructionElementTableProps) => {
	const mark = useMark()
	const history = useHistory()
	const setPopup = useSetPopup()

	const [constructionElements, setConstructionElements] = useState(
		[] as ConstructionElement[]
	)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const constructionElementsResponse = await httpClient.get(
						`/constructions/${constructionId}/elements`
					)
					setConstructionElements(constructionElementsResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/construction-elements/${id}`)
			constructionElements.splice(row, 1)
			setPopup(defaultPopup)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="mrg-bot">
			<h2 className="mrg-top-3 bold text-centered">Перечень элементов</h2>
			<PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={() =>
					history.push(
						`/specifications/${specificationId}/constructions/${constructionId}/element-create`
					)
				}
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>вид профиля</th>
						<th>имя профиля</th>
						<th>марка стали</th>
						<th>длина площадь</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{constructionElements.map((ce, index) => {
						return (
							<tr key={index}>
								<td>{ce.profileClass.id}</td>
								<td>{ce.profileName}</td>
								<td>{ce.steel.id}</td>
								<td>{ce.surfaceArea}</td>
								<td
									onClick={() => {
										setConstructionElement(ce)
										history.push(
											`/specifications/${specificationId}/constructions/${constructionId}/elements/${ce.id}`
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
											msg: `Вы действительно хотите удалить элемент конструкции?`,
											onAccept: () =>
												onDeleteClick(index, ce.id),
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

export default ConstructionElementTable
