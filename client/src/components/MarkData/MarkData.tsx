import React from 'react'
import './MarkData.css'

const MarkData = () => {
	return (
        <div className="component-cnt component-width">
            <h1 className="text-centered">Данные марки</h1>
            <div>
                <div className="flex-v mrg-bot">
                    <p className="label-area">Название марки</p>
                    <div className="info-area">Тест</div>
                </div>
                <div className="flex-v mrg-bot">
                    <p className="label-area">Шифр марки</p>
                    <div className="info-area">Тест</div>
                </div>
                <div className="flex-v mrg-bot">
                    <p className="label-area">ГИП</p>
                    <div className="info-area">Тест</div>
                </div>

                <div className="flex-v mrg-bot">
                    <p className="label-area">Наименование комплекса</p>
                    <div className="info-area">Тест</div>
                </div>

                <div className="flex-v mrg-bot">
                    <p className="label-area">Наименование объекта</p>
                    <div className="info-area">Тест</div>
                </div>
            </div>
        </div>
	)
}

export default MarkData
