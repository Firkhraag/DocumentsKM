// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import Form from 'react-bootstrap/Form'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
import Doc from '../../model/Doc'
import AdditionalWork from '../../model/AdditionalWork'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type AdditionalWorkTableProps = {
    setPopupObj: (popupObj: IPopupObj) => void
	setAdditionalWork: (w: AdditionalWork) => void
}

const AdditionalWorkTable = ({ setPopupObj, setAdditionalWork }: AdditionalWorkTableProps) => {
    const reviewCoeff = 0.4
    const valuationCoeff = 0.05
    const orderCoeff = 0.004

    const valuationPagesCoeff = 8

	const mark = useMark()
    const history = useHistory()

    const [docsGroupedByCreator, setDocsGroupedByCreator] = useState([] as Doc[])
    const [docsGroupedByNormContr, setDocsGroupedByNormContr] = useState([] as Doc[])

    const [additionalWorkArray, setAdditionalWorkArray] = useState([] as AdditionalWork[])

    useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
                    const additionalWorkFetchedResponse = await httpClient.get(
                        `/marks/${mark.id}/additional-work`
                    )
                    setAdditionalWorkArray(additionalWorkFetchedResponse.data)
					// const docsFetchedResponse = await httpClient.get(
					// 	`/marks/${mark.id}/docs`
					// )
                    // setDocsGroupedByCreator(docsFetchedResponse.data.docsGroupedByCreator)
                    // setDocsGroupedByNormContr(docsFetchedResponse.data.docsGroupedByNormContr)

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
			additionalWorkArray.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">Учет дополнительных проектных работ</h1>
			<PlusCircle
				onClick={() => history.push('/additional-work-add')}
				color="#666"
				size={28}
				className="pointer"
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th rowSpan={3} style={{verticalAlign: "middle"}}>Исполнитель</th>
                        <th colSpan={4}>Дополнительные проектные работы</th>
                        <th colSpan={3}>Основные проектные работы</th>
                        <th className="text-centered" style={{verticalAlign: "middle"}} rowSpan={3} colSpan={2}>Действия</th>
					</tr>
                    <tr>
                        <th colSpan={2}>Расчет конструкций</th>
                        <th colSpan={2}>Заказ металла</th>
                        <th rowSpan={2} style={{verticalAlign: "middle"}}>Выполн. чертежей</th>
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
                    {/* {docsGroupedByCreator.map((v, index) => {
                        const filteredDocs = docsGroupedByNormContr.filter(d => d.normContr.id == v.creator.id)
                        let format = 0
                        if (filteredDocs.length === 1) {
                            format = filteredDocs[0].form
                        }
                        return (
                            <tr key={index}>
                                <td>{v.creator.name}</td>
                                <td>{v.numOfPages * valuationPagesCoeff}</td>
                                <td>{v.numOfPages * valuationPagesCoeff * valuationCoeff}</td>
                                <td>4</td>
                                <td>{4 * orderCoeff}</td>
                                <td>{v.form}</td>
                                <td>{format}</td>
                                <td>{format * reviewCoeff}</td>
                                <td
									onClick={() => {
										null
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
											isShown: true,
											msg: `Вы действительно хотите удалить исполнителя?`,
											onAccept: () =>
												onDeleteClick(index, v.id),
											onCancel: () =>
												setPopupObj(defaultPopupObj),
										})
									}
									className="pointer action-cell-width text-centered"
								>
									<Trash color="#666" size={26} />
								</td>
                            </tr>
                        )
                    })} */}
                    {additionalWorkArray.map((v, index) => {
                        return (
                            <tr key={index}>
                                <td>{v.employee.name}</td>
                                <td>{v.valuation}</td>
                                <td>{v.valuation * valuationCoeff}</td>
                                <td>{v.metalOrder}</td>
                                <td>{v.metalOrder * orderCoeff}</td>
                                <td>a</td>
                                <td>a</td>
                                <td>a</td>
                                <td
									onClick={() => {
										setAdditionalWork(v)
										history.push(
											`/additional-work/${v.id}`
										)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
											isShown: true,
											msg: `Вы действительно хотите удалить исполнителя?`,
											onAccept: () =>
												onDeleteClick(index, v.id),
											onCancel: () =>
												setPopupObj(defaultPopupObj),
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
                        <td>{docsGroupedByCreator.map(v => v.numOfPages).reduce((a, b) => (a + b), 0) * valuationPagesCoeff}</td>
                        <td>{docsGroupedByCreator.map(v => v.numOfPages).reduce((a, b) => (a + b), 0) * valuationPagesCoeff * valuationCoeff}</td>
                        <td>4</td>
                        <td>5</td>
                        <td>{docsGroupedByCreator.map(v => v.form).reduce((a, b) => a + b, 0)}</td>
                        <td>{docsGroupedByNormContr.map(v => v.form).reduce((a, b) => a + b, 0)}</td>
                        <td>{docsGroupedByNormContr.map(v => v.form).reduce((a, b) => (a + b), 0) * reviewCoeff}</td>
                    </tr>
				</tbody>
			</Table>
		</div>
	)
}

export default AdditionalWorkTable
