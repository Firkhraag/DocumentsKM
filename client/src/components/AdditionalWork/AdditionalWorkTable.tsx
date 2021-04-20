// Global
import React, { useState, useEffect } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import AdditionalWorkData from './AdditionalWorkData'
import { useMark } from '../../store/MarkStore'
import AdditionalWork from '../../model/AdditionalWork'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'
import { getEmployeeShortName } from '../../util/get-employee-short-name'

const AdditionalWorkTable = () => {
	const reviewCoeff = 0.4
	const valuationCoeff = 0.05
	const orderCoeff = 0.004
	const valuationPagesCoeff = 8

	const mark = useMark()
	const setPopup = useSetPopup()

	const [additionalWorkArray, setAdditionalWorkArray] = useState(
		[] as AdditionalWork[]
	)
	const [additionalWorkData, setAdditionalWorkData] = useState({
		isCreateMode: false,
		additionalWork: null,
	})

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const additionalWorkResponse = await httpClient.get(
						`/marks/${mark.id}/additional-work`
					)
					setAdditionalWorkArray(additionalWorkResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/additional-work/${id}`)
			var arr = [...additionalWorkArray]
			arr.splice(row, 1)
			setAdditionalWorkArray(arr)
			setPopup(defaultPopup)
			setAdditionalWorkData({
				additionalWork: null,
				isCreateMode: false,
			})
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">
				Учет дополнительных проектных работ
			</h1>
			{additionalWorkData.isCreateMode || additionalWorkData.additionalWork != null ? <AdditionalWorkData 
				additionalWorkData={additionalWorkData}
				setAdditionalWorkData={setAdditionalWorkData}
				additionalWorkArray={additionalWorkArray}
				setAdditionalWorkArray={setAdditionalWorkArray} /> : null}
			<PlusCircle
				onClick={() => {
					setAdditionalWorkData({
						isCreateMode: true,
						additionalWork: null,
					})
					window.scrollTo(0, 0)
				}}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th rowSpan={3} style={{ verticalAlign: 'middle' }} className="add-work-name-col-width">
							Исполнитель
						</th>
						<th colSpan={4}>Дополнительные проектные работы</th>
						<th colSpan={3}>Основные проектные работы</th>
						<th
							className="text-centered"
							style={{ verticalAlign: 'middle' }}
							rowSpan={3}
							colSpan={2}
						>
							Действия
						</th>
					</tr>
					<tr>
						<th colSpan={2}>Расчет конструкций</th>
						<th colSpan={2}>Заказ металла</th>
						<th rowSpan={2} style={{ verticalAlign: 'middle' }} className="completed-num-col-width">
							Выполн. чертежей
						</th>
						<th colSpan={2}>Проверка чертежей</th>
					</tr>
					<tr>
						<th>лист, a4</th>
						<th>прив. к A1</th>
						<th>строк</th>
						<th>прив. к A1</th>
						<th>выполнено</th>
						<th>прив. к A1</th>
					</tr>
				</thead>
				<tbody>
					{additionalWorkArray.map((v, index) => {
						return (
							<tr key={index}>
								<td className="add-work-name-col-width">{getEmployeeShortName(v.employee.fullname)}</td>
								<td>{v.valuation === 0 ? '' : v.valuation}</td>
								<td>
									{v.valuation === 0
										? ''
										: Math.round(
												v.valuation *
													valuationCoeff *
													1000
										  ) / 1000}
								</td>
								<td>
									{v.metalOrder === 0 ? '' : v.metalOrder}
								</td>
								<td>
									{v.metalOrder === 0
										? ''
										: Math.round(
												v.metalOrder * orderCoeff * 1000
										  ) / 1000}
								</td>
								<td className="completed-num-col-width">
									{v.drawingsCompleted < 0.0000001
										? ''
										: v.drawingsCompleted}
								</td>
								<td>
									{v.drawingsCheck < 0.0000001
										? ''
										: v.drawingsCheck}
								</td>
								<td>
									{v.drawingsCheck < 0.0000001
										? ''
										: Math.round(
												v.drawingsCheck *
													valuationCoeff *
													1000
										  ) / 1000}
								</td>
								<td
									onClick={() => {
										setAdditionalWorkData({
											isCreateMode: false,
											additionalWork: v,
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
											msg: `Вы действительно хотите удалить исполнителя ${v.employee.fullname}?`,
											onAccept: () =>
												onDeleteClick(index, v.id),
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
					<tr>
						<th>Итого по проекту</th>
						<td>
							{additionalWorkArray
								.map((v) => v.valuation)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.valuation)
											.reduce((a, b) => a + b, 0) * 1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.valuation)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.valuation)
											.reduce((a, b) => a + b, 0) *
											valuationCoeff *
											1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.metalOrder)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.metalOrder)
											.reduce((a, b) => a + b, 0) * 1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.metalOrder)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.metalOrder)
											.reduce((a, b) => a + b, 0) *
											orderCoeff *
											1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.drawingsCompleted)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.drawingsCompleted)
											.reduce((a, b) => a + b, 0) * 1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.drawingsCheck)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.drawingsCheck)
											.reduce((a, b) => a + b, 0) * 1000
								  ) / 1000
								: ''}
						</td>
						<td>
							{additionalWorkArray
								.map((v) => v.drawingsCheck)
								.reduce((a, b) => a + b, 0) > 0.000001
								? Math.round(
										additionalWorkArray
											.map((v) => v.drawingsCheck)
											.reduce((a, b) => a + b, 0) *
											valuationCoeff *
											1000
								  ) / 1000
								: ''}
						</td>
					</tr>
				</tbody>
			</Table>
		</div>
	)
}

export default AdditionalWorkTable
