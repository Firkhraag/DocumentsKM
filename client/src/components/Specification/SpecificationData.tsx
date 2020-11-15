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
// Other
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
    
    const onNoteChange = (event: React.FormEvent<HTMLTextAreaElement>) => {
		setSelectedObject({
			...selectedObject,
			note: event.currentTarget.value,
		})
	}

	return selectedObject == null || mark == null ? null : (
		<div className="component-cnt flex-v-cent-h">
			<h1 className="text-centered">Данные выпуска спецификации</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-width component-cnt-div">
				<Form.Group style={{ marginBottom: 0 }}>
					<Form.Label>Примечание</Form.Label>
					<Form.Control
						as="textarea"
						rows={4}
						style={{ resize: 'none' }}
						placeholder="Не введено"
						defaultValue={selectedObject.note}
						onBlur={onNoteChange}
					/>
				</Form.Group>

                <div className="mrg-top-2 bold">
                    Перечень видов конструкций
                </div>

                <PlusCircle
                    onClick={() => history.push('/sheet-create')}
                    color="#666"
                    size={28}
                    className="pointer"
                />

				<table>
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
				</table>
			</div>

            <Table bordered striped className="mrg-top no-bot-mrg">
                    <thead>
                        <tr>
                            <th>№</th>
                            <th>Код</th>
                            <th>Вид конструкции</th>
                            <th>Включать</th>
                            <th className="text-centered" colSpan={2}>
                                Действия
                            </th>
                        </tr>
                    </thead>
                    {/* <tbody>
                        {sheets.map((s, index) => {
                            return (
                                <tr key={s.id}>
                                    <td>{s.num}</td>
                                    <td className="doc-name-col-width">{s.name}</td>
                                    <td>{s.form}</td>
                                    <td className="doc-employee-col-width">
                                        {s.creator == null ? '' : s.creator.name}
                                    </td>
                                    <td className="doc-employee-col-width">
                                        {s.inspector == null
                                            ? ''
                                            : s.inspector.name}
                                    </td>
                                    <td className="doc-employee-col-width">
                                        {s.normContr == null
                                            ? ''
                                            : s.normContr.name}
                                    </td>
                                    <td className="doc-note-col-width">{s.note}</td>
                                    <td
                                        onClick={() => {
                                            setSheet(s)
                                            history.push(`/sheets/${s.id}`)
                                        }}
                                        className="pointer action-cell-width text-centered"
                                    >
                                        <PencilSquare color="#666" size={26} />
                                    </td>
                                    <td
                                        onClick={() =>
                                            setPopupObj({
                                                isShown: true,
                                                msg: `Вы действительно хотите удалить лист основного комплекта №${s.num}?`,
                                                onAccept: () =>
                                                    onDeleteClick(index, s.id),
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
                    </tbody> */}
                </Table>

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
