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
import Doc from '../../model/Doc'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type AdditionalWorkProps = {
}

const AdditionalWork = ({  }: AdditionalWorkProps) => {
	const mark = useMark()
    const history = useHistory()
    
    // useEffect(() => {
	// 	if (mark != null && mark.id != null) {
	// 		const fetchData = async () => {
	// 			try {
	// 				const sheetsFetchedResponse = await httpClient.get(
	// 					`/marks/${mark.id}/docs/sheets`
	// 				)
	// 				setSheets(sheetsFetchedResponse.data)
	// 			} catch (e) {
	// 				console.log('Failed to fetch the data', e)
	// 			}
	// 		}
	// 		fetchData()
	// 	}
	// }, [mark])

	return (
		<div className="component-cnt table-width">
			<h1 className="text-centered">Учет дополнительных проектных работ</h1>
			<PlusCircle
				onClick={() => history.push('/sheet-create')}
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
				
			</Table>
		</div>
	)
}

export default AdditionalWork
