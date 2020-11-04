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
import LinkedDoc from '../../model/LinkedDoc'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'
import './LinkedDocs.css'

type LinkedDocsProps = {
    setPopupObj: (popupObj: IPopupObj) => void
    setLinkedDoc: (ld: LinkedDoc) => void
}

const LinkedDocs = ({ setPopupObj, setLinkedDoc }: LinkedDocsProps) => {
    const mark = useMark()
    const history = useHistory()
    const [linkedDocs, setLinkedDocs] = useState([] as LinkedDoc[])

    useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const linkedDocsFetchedResponse = await httpClient.get(
						`/marks/${mark.id}/linked-docs`
                    )
					setLinkedDocs(linkedDocsFetchedResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
    }, [mark])
    
    const onDeleteClick = async (row: number, id: number) => {
		try {
            await httpClient.delete(`/linked-docs/${id}`)
            linkedDocs.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

    return (
        <div className="component-cnt">
			<h1 className="text-centered">Ссылочные документы</h1>
            <PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={() => history.push('/linked-doc-create')}
			/>
            <Table bordered hover className="mrg-top">
				<thead>
					<tr>
                        <td>№</td>
						<td>Шифр</td>
						<td>Обозначение</td>
						<td className="linked-doc-name-col-width">Наименование</td>
						<td className="text-centered" colSpan={2}>Действия</td>
					</tr>
				</thead>
				<tbody>
					{linkedDocs.map((ld, index) => {
						return (
							<tr key={index}>
                                <td>{index + 1}</td>
								<td>{ld.code}</td>
								<td>{ld.designation}</td>
                                <td className="linked-doc-name-col-width">{ld.name}</td>
								<td
									onClick={() =>
										history.push(`/linked-docs/${ld.id}`)
									}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										setPopupObj({
                                            isShown: true,
                                            msg: `Вы действительно хотите удалить ссылочный документ ${ld.code}?`,
                                            onAccept: () =>
                                                onDeleteClick(
                                                    index,
                                                    ld.id
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

export default LinkedDocs
