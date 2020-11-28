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
import { useMark } from '../../store/MarkStore'

type SpecificationDataProps = {
	specification: Specification
}

const SpecificationData = ({ specification }: SpecificationDataProps) => {
	const history = useHistory()
	const mark = useMark()

	const [selectedObject, setSelectedObject] = useState(specification)

	// const [constructions, setConstructions] = useState([] as ConstructionType[])
	// const [highTensileBolts, setHighTensileBolts] = useState([] as Specification[])
	// const [standardConstructions, setStandardConstructions] = useState([] as StandardConstruction[])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			if (selectedObject == null) {
				history.push('/specifications')
				return
			}
		}
	}, [mark])

	const onNoteChange = async (event: React.FormEvent<HTMLTextAreaElement>) => {
        try {
            await httpClient.patch(`/specifications/${selectedObject.id}`, {
                note: event.currentTarget.value,
            })
        } catch (e) {
            console.log('Error')
        }
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Данные выпуска спецификации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width-2 component-cnt-div">
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

				<h2 className="mrg-top-2 bold text-centered">Перечень видов конструкций</h2>

				<PlusCircle
					onClick={() => history.push(`/specifications/${selectedObject.id}/construction-create`)}
					color="#666"
					size={28}
					className="pointer mrg-top"
				/>

				<Table bordered striped className="mrg-top no-bot-mrg">
					<thead>
						<tr>
							<th>№</th>
							<th>Вид конструкции</th>
							<th className="text-centered" colSpan={2}>
								Действия
							</th>
						</tr>
					</thead>
				</Table>

				{/* <table>
					<tbody>
						<tr className="head-tr">
							<td>Вид конструкции</td>
							<td>Шифр</td>
							<td>Вкл</td>
						</tr>
						<tr>
							<td>Балки</td>
							<td>11</td>
							<td>+</td>
						</tr>
						<tr>
							<td>Связи</td>
							<td>11</td>
							<td>+</td>
						</tr>
						<tr>
							<td>Прогоны</td>
							<td>11</td>
							<td>+</td>
						</tr>
						<tr>
							<td>
								Lorem Ipsum - это текст-"рыба", часто
								используемый в печати и вэб-дизайне.
							</td>
							<td>11</td>
							<td>+</td>
						</tr>
					</tbody>
				</table> */}

				{/* <Button
					variant="secondary"
					className="btn-mrg-top-2 full-width"
					onClick={null}
				>
					Сохранить изменения
				</Button> */}
			</div>
		</div>
	)

	// <div className="component-cnt component-width">
	//     <h1 className="text-centered">Данные спецификации</h1>
	//     <div>
	//         <p>Выпуск: 0</p>
	//         <div className="flex-v mrg-bottom">
	//             <p className="label-area">Выпуск</p>
	//             <div className="info-area">
	//                 2
	//             </div>
	//         </div>
	//         <p>Текущий: M32788.111.111-KVB 8.AA</p>
	// <p>Перечень видов конструкций</p>
	// <table>
	//     <tbody>
	//         <tr>
	//             <td>Вид</td>
	//             <td>Шифр</td>
	//             <td>Вкл</td>
	//         </tr>
	//         <tr>
	//             <td>Балки</td>
	//             <td>11</td>
	//             <td>+</td>
	//         </tr>
	//         <tr>
	//             <td>Связи</td>
	//             <td>11</td>
	//             <td>+</td>
	//         </tr>
	//         <tr>
	//             <td>Прогоны</td>
	//             <td>11</td>
	//             <td>+</td>
	//         </tr>
	//         <tr>
	//             <td>Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне.</td>
	//             <td>11</td>
	//             <td>+</td>
	//         </tr>
	//     </tbody>
	// </table>
	//         <div>
	//             <p>Высокопрочные болты</p>
	//         </div>
	//         <div>
	//             <p>Типовые конструкции</p>
	//         </div>
	//         <div>
	//             <p>Данные по виду конструкции</p>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Название вида конструкции</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Шифр вида конструкции</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Шифр подвида конструкции</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Расценка</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Шифр типового альбома конструкции</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Число типовых конструкций</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Притупление кромок</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Динамическая нагрузка</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Фланцевые соединения</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Контроль плотности сварных швов</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Коэффициент окрашивания</p>
	//                 <div className="info-area">
	//                     2
	//                 </div>
	//             </div>

	//             <div className="flex-v mrg-bottom">
	//                 <p className="label-area">Включить вид конструкции в спецификацию</p>
	//                 <div className="info-area">
	//                     +/-
	//                 </div>
	//             </div>

	//             {/* <p>Вид конструкции</p>
	//             <textarea />
	//             <p>Включить в спецификацию +/-</p>
	//             <p>Коэффициент окрашивания</p>
	//             <input type="text" /> */}
	//         </div>

	//         <div>
	//             <p>Перечень элементов вида конструкции</p>
	//             <p>TBD</p>
	//         </div>
	//     </div>
	// </div>
}

export default SpecificationData
