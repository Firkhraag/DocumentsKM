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
import ConstructionBoltData from './ConstructionBoltData'
import { useMark } from '../../store/MarkStore'
import ConstructionBolt from '../../model/ConstructionBolt'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type ConstructionBoltTableProps = {
	constructionId: number
}

const ConstructionBoltTable = ({
	constructionId,
}: ConstructionBoltTableProps) => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [constructionBolts, setConstructionBolts] = useState(
		[] as ConstructionBolt[]
	)

	const [constructionBoltData, setConstructionBoltData] = useState({
		isCreateMode: false,
		constructionBolt: null,
	})

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
			var arr = [...constructionBolts]
			arr.splice(row, 1)
			setConstructionBolts(arr)
			setPopup(defaultPopup)
			setConstructionBoltData({
				constructionBolt: null,
				isCreateMode: false,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="mrg-bot">
			<h2 className="mrg-top-3 bold text-centered" id="construction-bolt-header">
				Высокопрочные болты
			</h2>
			{constructionBoltData.isCreateMode || constructionBoltData.constructionBolt != null ? <ConstructionBoltData 
				constructionBoltData={constructionBoltData}
				setConstructionBoltData={setConstructionBoltData}
				constructionBolts={constructionBolts}
				setConstructionBolts={setConstructionBolts}
				constructionId={constructionId} /> : null}
			<PlusCircle
				onClick={() => {
					Scroll.scroller.scrollTo(`construction-bolt-header`, {
						offset: -200,
					})
					setConstructionBoltData({
						isCreateMode: true,
						constructionBolt: null,
					})
				}}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top">
				<thead>
					<tr>
						<th>№</th>
						<th>Диаметр болта, мм</th>
						<th>Толщина пакета, мм</th>
						<th>Болтов, шт.</th>
						<th>Гаек на болт, шт.</th>
						<th>Шайб на болт, шт.</th>
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
										Scroll.scroller.scrollTo(`construction-bolt-header`, {
											offset: -200,
										})
										setConstructionBoltData({
											isCreateMode: false,
											constructionBolt: cb,
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
											msg: `Вы действительно хотите удалить выскопрочный болт № ${
												index + 1
											}?`,
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
