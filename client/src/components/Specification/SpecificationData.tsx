import React from 'react'
import { useMark } from '../../store/MarkStore'

const SpecificationData = () => {
	const mark = useMark()

	return mark == null ? null : (
		<div className="component-cnt component-width">
			<h1 className="text-centered">Данные спецификации</h1>
			<div>
				<div>
					<p>Выпуск №1</p>
					<textarea />
				</div>
				<div>
					<p>Примечание</p>
					<textarea />
				</div>
				<p>Перечень видов конструкций</p>
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
