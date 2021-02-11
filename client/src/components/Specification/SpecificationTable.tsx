// Global
import React, { useState, useEffect, createRef } from 'react'
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
import Specification from '../../model/Specification'
import { defaultPopup, useSetPopup } from '../../store/PopupStore'

type SpecificationTableProps = {
	setSpecification: (spec: Specification) => void
}

const SpecificationTable = ({ setSpecification }: SpecificationTableProps) => {
	const mark = useMark()
	const history = useHistory()
	const setPopup = useSetPopup()

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
						const specificationsResponse = await httpClient.get(
							`/marks/${mark.id}/specifications`
						)
						for (let s of specificationsResponse.data) {
							if (s.isCurrent) {
								setCurrentSpecId(s.id)
							}
							refs.push(createRef())
						}
						setSpecifications(specificationsResponse.data)
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
			await httpClient.patch(`/specifications/${id}`, {
				isCurrent: true,
			})
			const inputElement = refs[row].current as any
			if (inputElement) {
				inputElement.checked = true
			}
			setCurrentSpecId(id)
			for (const s of specifications) {
				s.isCurrent = false
			}
			specifications[row].isCurrent = true
			setPopup(defaultPopup)
		} catch (e) {
			console.log('Error')
		}
	}

	const onCreateClick = async () => {
		try {
			const newSpecificationResponse = await httpClient.post(
				`/marks/${mark.id}/specifications`
			)
			specifications.push(newSpecificationResponse.data)
			setSpecifications(specifications)
			refs.push(createRef())
			setCurrentSpecId(newSpecificationResponse.data.id)
			setPopup(defaultPopup)
		} catch (e) {
			console.log('Error')
		}
	}

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/specifications/${id}`)
			refs.splice(row, 1)
			const arr = [...specifications]
			arr.splice(row, 1)
			setSpecifications(arr)
			setPopup(defaultPopup)
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
					setPopup({
						isShown: true,
						msg:
							'Вы действительно хотите добавить новый выпуск спецификации?',
						onAccept: onCreateClick,
						onCancel: () => setPopup(defaultPopup),
					})
				}
			/>
			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th>Создан</th>
						<th className="spec-note-col-width">Примечание</th>
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
								<td>{s.num}</td>
								<td>
									{s.createdDate == null
										? ''
										: new Date(
												s.createdDate
										  ).toLocaleDateString()}
								</td>
								<td className="spec-note-col-width">
									{s.note}
								</td>
								<td
									onClick={() =>
										currentSpecId === s.id
											? null
											: setPopup({
													isShown: true,
													msg: `Вы действительно хотите сделать выпуск спецификации № ${s.num} текущим?`,
													onAccept: () =>
														onSelectCurrentClick(
															index,
															s.id
														),
													onCancel: () =>
														setPopup(defaultPopup),
											  })
									}
									className="pointer text-centered"
								>
									<Form.Check
										ref={refs[index]}
										id={`is${s.id}`}
										name="currentRelease"
										type="radio"
										style={{ pointerEvents: 'none' }}
									/>
								</td>
								<td
									onClick={() => {
										setSpecification(s)
										history.push(`/specifications/${s.id}`)
									}}
									className="pointer action-cell-width text-centered"
								>
									<PencilSquare color="#666" size={26} />
								</td>
								<td
									onClick={() =>
										currentSpecId === s.id
											? null
											: setPopup({
													isShown: true,
													msg: `Вы действительно хотите удалить выпуск спецификации № ${s.num}?`,
													onAccept: () =>
														onDeleteClick(
															index,
															s.id
														),
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

export default SpecificationTable
