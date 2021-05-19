// Global
import React, { useState, useEffect } from 'react'
import Scroll from 'react-scroll'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import ConstructionElementData from './ConstructionElementData'
import { useMark } from '../../store/MarkStore'
import ConstructionElement from '../../model/ConstructionElement'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type ConstructionElementTableProps = {
	constructionId: number
}

const ConstructionElementTable = ({
	constructionId,
}: ConstructionElementTableProps) => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [constructionElements, setConstructionElements] = useState([] as ConstructionElement[])

	const [constructionElementData, setConstructionElementData] = useState({
		isCreateMode: false,
		constructionElement: null,
		index: -1,
	})

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
			var arr = [...constructionElements]
			arr.splice(row, 1)
			setConstructionElements(arr)
			setPopup(defaultPopup)
			setConstructionElementData({
				constructionElement: null,
				isCreateMode: false,
				index: -1,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div>
			<h2 className="bold text-centered" id="construction-element-header">Перечень элементов</h2>
			{constructionElementData.isCreateMode || constructionElementData.constructionElement != null ? <ConstructionElementData 
				constructionElementData={constructionElementData}
				setConstructionElementData={setConstructionElementData}
				constructionElements={constructionElements}
				setConstructionElements={setConstructionElements}
				constructionId={constructionId} /> : null}
			<PlusCircle
				onClick={() => {
					Scroll.scroller.scrollTo(`construction-element-header`, {
						offset: -200,
					})
					setConstructionElementData({
						isCreateMode: true,
						constructionElement: null,
						index: -1,
					})
				}}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th className="profile-class-name-col-width">
							Вид профиля
						</th>
						<th>Имя профиля</th>
						<th>Символ</th>
						<th>Марка стали</th>
						<th>Длина площадь</th>
						<th>Вес 1 м</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{constructionElements.map(
						(ce, index) => {
							return (
								<tr key={index}>
									<td>{index + 1}</td>
									<td className="profile-class-name-col-width">
										{ce.profile.class.name}
									</td>
									<td>{ce.profile.name}</td>
									<td>{ce.profile.symbol}</td>
									<td>{ce.steel.name}</td>
									<td>{ce.length}</td>
									<td>{ce.profile.weight}</td>
									<td
										onClick={() => {
											Scroll.scroller.scrollTo(`construction-element-header`, {
												offset: -200,
											})
											setConstructionElementData({
												isCreateMode: false,
												constructionElement: ce,
												index: index + 1,
											})
										}}
										className="pointer action-cell-width text-centered"
									>
										<PencilSquare color="#666" size={26} />
									</td>
									<td
										onClick={() =>
											setPopup({
												isShown: true,
												msg: `Вы действительно хотите удалить элемент конструкции № ${
													index + 1
												}?`,
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
						}
					)}
				</tbody>
			</Table>
		</div>
	)
}

export default ConstructionElementTable
