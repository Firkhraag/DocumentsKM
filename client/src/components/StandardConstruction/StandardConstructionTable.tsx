// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import StandardConstructionData from './StandardConstructionData'
import { useMark } from '../../store/MarkStore'
import StandardConstruction from '../../model/StandardConstruction'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type StandardConstructionTableProps = {
	specificationId: number
}

const StandardConstructionTable = ({ specificationId }: StandardConstructionTableProps) => {
	const mark = useMark()
	const setPopup = useSetPopup()

	const [standardConstructions, setStandardConstructions] = useState(
		[] as StandardConstruction[]
	)

	const [standardConstructionData, setStandardConstructionData] = useState({
		isCreateMode: false,
		standardConstruction: null,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const standardConstructionResponse = await httpClient.get(
						`/specifications/${specificationId}/standard-constructions`
					)
					setStandardConstructions(standardConstructionResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/standard-constructions/${id}`)
            var arr = [...standardConstructions]
			arr.splice(row, 1)
			setStandardConstructions(arr)
			setPopup(defaultPopup)
			setStandardConstructionData({
				standardConstruction: null,
				isCreateMode: false,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="mrg-bot">
			<h2 className="mrg-top-3 bold text-centered">
				Типовые конструкции
			</h2>

			<div className="full-width">
				{standardConstructionData.isCreateMode || standardConstructionData.standardConstruction != null ? <StandardConstructionData 
					standardConstructionData={standardConstructionData}
					setStandardConstructionData={setStandardConstructionData}
					standardConstructions={standardConstructions}
					setStandardConstructions={setStandardConstructions}
					specificationId={specificationId} /> : null}
				<PlusCircle
					onClick={() => {
						setStandardConstructionData({
							isCreateMode: true,
							standardConstruction: null,
						})
						// window.scrollTo(0, 0)
					}}
					color="#666"
					size={28}
					className="pointer"
				/>
				<Table bordered striped className="mrg-top no-bot-mrg">
					<thead>
						<tr>
							<th>№</th>
							<th className="st-construction-name-col-width">
								Наименование
							</th>
							<th>Количество, шт.</th>
							<th>№ листа</th>
							<th className="st-construction-other-col-width">
								Вес, т
							</th>
							<th className="text-centered" colSpan={2}>
								Действия
							</th>
						</tr>
					</thead>
					<tbody>
						{standardConstructions.map((sc, index) => {
							return (
								<tr key={index}>
									<td>{index + 1}</td>
									<td className="st-construction-name-col-width">
										{sc.name}
									</td>
									<td>{sc.num}</td>
									<td>{sc.sheet}</td>
									<td className="st-construction-other-col-width">
										{sc.weight}
									</td>
									<td
										onClick={() => {
											setStandardConstructionData({
												isCreateMode: false,
												standardConstruction: sc,
											})
											// window.scrollTo(0, 0)
										}}
										className="pointer action-cell-width text-centered"
									>
										<PencilSquare color="#666" size={26} />
									</td>
									<td
										onClick={() =>
											setPopup({
												isShown: true,
												msg: `Вы действительно хотите удалить типовую конструкцию № ${
													index + 1
												}?`,
												onAccept: () =>
													onDeleteClick(index, sc.id),
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
		</div>
	)
}

export default StandardConstructionTable
