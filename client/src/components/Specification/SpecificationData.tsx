// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
// Bootstrap
import Table from 'react-bootstrap/Table'
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import { PlusCircle } from 'react-bootstrap-icons'
import { PencilSquare } from 'react-bootstrap-icons'
import { Trash } from 'react-bootstrap-icons'
// Util
import httpClient from '../../axios'
import Specification from '../../model/Specification'
import Construction from '../../model/Construction'
import { useMark } from '../../store/MarkStore'
import { IPopupObj, defaultPopupObj } from '../Popup/Popup'

type SpecificationDataProps = {
	specification: Specification
	setPopupObj: (popupObj: IPopupObj) => void
	setConstruction: (c: Construction) => void
}

const SpecificationData = ({
	specification,
	setPopupObj,
	setConstruction,
}: SpecificationDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState(specification)

	const [constructions, setConstructions] = useState([] as Construction[])
	// const [highTensileBolts, setHighTensileBolts] = useState([] as Specification[])
	// const [standardConstructions, setStandardConstructions] = useState([] as StandardConstruction[])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null) {
				history.push('/specifications')
				return
			}
			const fetchData = async () => {
				try {
					const constructionFetchedResponse = await httpClient.get(
						`/specifications/${specification.id}/constructions`
					)
					setConstructions(constructionFetchedResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data', e)
				}
			}
			fetchData()
		}
	}, [mark])

	const onNoteChange = async (
		event: React.FormEvent<HTMLTextAreaElement>
	) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	const onChangeButtonClick = async () => {
		try {
			await httpClient.patch(`/specifications/${selectedObject.id}`, {
				note: selectedObject.note,
			})
			history.push('/specifications')
		} catch (e) {
			console.log('Error')
		}
	}

	const onDeleteClick = async (row: number, id: number) => {
		try {
			await httpClient.delete(`/constructions/${id}`)
			constructions.splice(row, 1)
			setPopupObj(defaultPopupObj)
		} catch (e) {
			console.log('Error')
		}
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Данные выпуска спецификации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
                <Form.Group className="flex-cent-v">
					<Form.Label
						className="no-bot-mrg"
						style={{ marginRight: '1em' }}
					>
						Номер
					</Form.Label>
					<Form.Control
						type="text"
                        value={selectedObject.num}
                        readOnly={true}
                        className="text-centered"
                        style={{width: "75px"}}
					/>
				</Form.Group>
				<Form.Group className="no-bot-mrg">
					<Form.Label htmlFor="note">Примечание</Form.Label>
					<Form.Control
						id="note"
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Не введено"
						defaultValue={selectedObject.note}
						onBlur={onNoteChange}
					/>
				</Form.Group>
				<Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={onChangeButtonClick}
				>
					Сохранить изменения
				</Button>
			</div>
			<h2 className="mrg-top-2 bold text-centered">
				Перечень видов конструкций
			</h2>

			<div className="full-width">
				<PlusCircle
					onClick={() =>
						history.push(
							`/specifications/${selectedObject.id}/construction-create`
						)
					}
					color="#666"
					size={28}
					className="pointer mrg-top"
				/>
			</div>

			<Table bordered striped className="mrg-top no-bot-mrg">
				<thead>
					<tr>
						<th>№</th>
						<th className="construction-name-col-width">Вид конструкции</th>
						<th className="text-centered" colSpan={2}>
							Действия
						</th>
					</tr>
				</thead>
				<tbody>
					{constructions.map((c, index) => {
						return (
							<tr key={index}>
								<td>{index + 1}</td>
								<td className="construction-name-col-width">{c.name}</td>
								<td
									onClick={() => {
										setConstruction(c)
										history.push(
											`/specifications/${specification.id}/constructions/${c.id}`
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
											msg: `Вы действительно хотите удалить вид конструкции?`,
											onAccept: () =>
												onDeleteClick(index, c.id),
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
				</tbody>
			</Table>
		</div>
	)
}

export default SpecificationData
