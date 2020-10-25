// Global
import React, { useState, useEffect, createRef } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import { useMark } from '../../store/MarkStore'
import Specification from '../../model/Specification'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'
import './Specifications.css'

type SpecificationsProps = {
	setPopupObj: (popupObj: IPopupObj) => void
}

const Specifications = ({ setPopupObj }: SpecificationsProps) => {
	const mark = useMark()
	const history = useHistory()

	const [specifications, setSpecifications] = useState([] as Specification[])

	const refs = useState([] as React.MutableRefObject<undefined>[])[0]
	const [currentSpecId, setCurrentSpecId] = useState(-1)
	const [initialRender, setInitialRender] = useState(true)

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (
				currentSpecId !== -1 &&
				refs.length > 0 &&
				specifications.length > 0
			) {
				for (const [i, s] of specifications.entries()) {
					if (s.id === currentSpecId) {
						const inputElement = refs[i].current as any
						if (inputElement) {
							inputElement.checked = true
						}
					}
				}
				return
            }
            if (initialRender) {
                const fetchData = async () => {
                    setInitialRender(false)
                    try {
                        const specificationsFetchedResponse = await httpClient.get(
                            `/marks/${mark.id}/specifications`
                        )
                        for (let s of specificationsFetchedResponse.data) {
                            if (s.isCurrent) {
                                setCurrentSpecId(s.id)
                            }
                            refs.push(createRef())
                        }
                        setSpecifications(specificationsFetchedResponse.data)
                        // setRerender(!rerender)
                    } catch (e) {
                        console.log('Failed to fetch the data', e)
                    }
                }
                fetchData()
            }
		}
	}, [mark, currentSpecId, specifications])

	const onSelectCurrentClick = async (row: number, id: number) => {
		try {
			await httpClient.patch(`marks/${mark.id}/specifications/${id}`, {
				isCurrent: true,
			})
			const inputElement = refs[row].current as any
			if (inputElement) {
				inputElement.checked = true
			}
			setCurrentSpecId(id)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	const onCreateClick = async () => {
		try {
			const newSpecificationFetched = await httpClient.post(
				`/marks/${mark.id}/specifications`
			)
			specifications.push(newSpecificationFetched.data)
			setSpecifications(specifications)
			refs.push(createRef())
			setCurrentSpecId(newSpecificationFetched.data.id)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/marks/${mark.id}/specifications/${id}`)
            refs.splice(row, 1)
            const newSpecArr = [...specifications]
			newSpecArr.splice(row, 1)
			setSpecifications(newSpecArr)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Выпуски спецификаций</h1>
			<PlusCircle
				color="#666"
				size={28}
				className="pointer"
				onClick={() =>
					setPopupObj({
						isShown: true,
						msg:
							'Вы действительно хотите добавить новый выпуск спецификации?',
						onAccept: onCreateClick,
						onCancel: () => setPopupObj(defaultPopupObj),
					})
				}
			/>
			<Table striped bordered hover className="mrg-top">
				<thead>
					<tr>
						<th>№</th>
						<th>Создан</th>
						<th className="note-cell-width">Примечание</th>
						<th>Текущий</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{specifications.map((s, index) => {
						return (
							<tr key={index}>
								<td>{s.releaseNumber}</td>
								<td>
									{new Date(s.created).toLocaleDateString()}
								</td>
								<td className="note-cell-width">{s.note}</td>
								<td
									onClick={() =>
										currentSpecId === s.id
											? null
											: setPopupObj({
													isShown: true,
													msg: `Вы действительно хотите сделать выпуск спецификации №${s.releaseNumber} текущим?`,
													onAccept: () =>
														onSelectCurrentClick(
															index,
															s.id
														),
													onCancel: () =>
														setPopupObj(
															defaultPopupObj
														),
											  })
									}
									className="pointer text-centered"
								>
									<input
										ref={refs[index]}
										className="current-radio-btn pointer"
										type="radio"
										id={`is${s.id}`}
										name="currentRelease"
									/>
								</td>
								<td
									onClick={() =>
										history.push('/specification-data')
									}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										currentSpecId === s.id
											? null
											: setPopupObj({
													isShown: true,
													msg: `Вы действительно хотите удалить выпуск спецификации №${s.releaseNumber}?`,
													onAccept: () =>
														onDeleteClick(
															index,
															s.id
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

export default Specifications
