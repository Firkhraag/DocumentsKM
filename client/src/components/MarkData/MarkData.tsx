import React, { useState, useEffect } from 'react'
import Mark from '../../model/Mark'
import InputArea from './InputArea'
import Dropdown from './Dropdown'
import './MarkData.css'

type MarkDataProps = {
	isCreateModeInitially: boolean
}

const MarkData = ({ isCreateModeInitially }: MarkDataProps) => {

    const seriesStringLength = 30
    const nodeStringLength = 10
    const subnodeStringLength = 10
    const markStringLength = 40
    const fullNameStringLength = 90

	const [isCreateMode, setIsCreateMode] = useState<boolean>(
		isCreateModeInitially
    )
    
	const [mark, setMark] = useState<Mark>(new Mark(null))
	const [series, setSeries] = useState<Array<string>>([])
	const [nodes, setNodes] = useState<Array<string>>([])
	const [subnodes, setSubnodes] = useState<Array<string>>([])
    const [marks, setMarks] = useState<Array<string>>([])
    
    const [markFullName, setMarkFullName] = useState('')
    const [latestMarks, setLatestMarks] = useState<string[]>([])

	useEffect(() => {
		const seriesFetched: Array<string> = ['M32788', 'V32788', 'G32788']
        setSeries(seriesFetched)
        
        const latestMarksFetched: Array<string> = [
            'M32788.111.111-KVB 8',
            'V62788.121.01-ZB11',
            'V62GHV.121.01-ZB11',
            'V62VB121.01-ZB11',
            'V62FD8.121.01-ZB11',
            'V6278V.21.01-ZB11',
            'V62788.121.01-ZB11',
            'D62SDS788.121.01-ZB11',
            'DSS.121.01-ZB11',
            'D6C88.121.01-ZB11',
            'D62SFDDS788.121.01-ZB11',
            'D62CVS788.121.01-ZB11',
            'D62CCSDS788.121.01-ZB11'
        ]
		setLatestMarks(latestMarksFetched)
    }, [])
    
    const onLatestMarkSelect = (event: React.MouseEvent<HTMLDivElement>) => {
        const v = event.currentTarget.textContent
        setNodes([])
        setSubnodes([])
		setMarks([])
		setMark({ ...new Mark(null), series: v })
		setMarkFullName(v)
    }

    const onSeriesSelect = (event: React.MouseEvent<HTMLDivElement>) => {
		const v = event.currentTarget.textContent
		if (v !== '') {
			// Fetch
			const nodes: Array<string> = ['527', '127', '134']

			setNodes(nodes)
		} else {
			setNodes([])
		}
		setSubnodes([])
		setMarks([])
		setMark({ ...new Mark(null), series: v })
	}

    const onNodeSelect = (event: React.MouseEvent<HTMLDivElement>) => {
		const v = event.currentTarget.textContent

		let gipSurname = ''
		if (v !== '') {
			// Fetch
			const subnodes: Array<string> = ['527', '127', '134']
			gipSurname = 'Влад'

			setSubnodes(subnodes)
		} else {
			setSubnodes([])
		}
		setMarks([])
		setMark({
			...new Mark(null),
			series: mark.series,
			node: v,
			gipSurname: gipSurname,
		})
	}

    const onSubnodeSelect = (event: React.MouseEvent<HTMLDivElement>) => {
		const v = event.currentTarget.textContent

		let facilityName = ''
		let objectName = ''
		if (v !== '') {
			// Fetch
			const codes: Array<string> = ['AVS 1', 'RTY 6', 'ZXE111']
			facilityName =
				'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.'
			objectName =
				'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.'

			setMarks(codes)
		} else {
			setMarks([])
		}
		setMark({
			...mark,
			subnode: v,
			facilityName: facilityName,
			objectName: objectName,
		})
    }
    
    const onMarkSelect = (event: React.MouseEvent<HTMLDivElement>) => {
        const v = event.currentTarget.textContent

		setMark({
			...mark,
			mark: v,
		})
	}

	return (
		<div className="mark-data-cnt">
			<h1 className="text-centered mark-data-title">Данные по марке</h1>
			<div className="tabs">
				<input
					type="radio"
					name="tab-btn"
					id="tab-btn-1"
					value=""
					onChange={() => setIsCreateMode(false)}
					checked={isCreateMode ? false : true}
				/>
				<label htmlFor="tab-btn-1">Выбрать</label>
				<input
					type="radio"
					name="tab-btn"
					id="tab-btn-2"
					value=""
					onChange={() => setIsCreateMode(true)}
					checked={isCreateMode ? true : false}
				/>
				<label htmlFor="tab-btn-2">Добавить</label>

				<div className="mark-data">
					{/* {isCreateMode ? null : (
                        <InputArea label="Последние марки" widthClassName={'w-latest-marks'} onChangeFunc={onLatestMarkSelect} value={markFullName} options={latestMarks} />
					)} */}
                    <div className="flex">
                    <Dropdown
                        label="Последние марки"
                        widthClassName={'input-width-1'}
                        maxInputLength={fullNameStringLength}
                        onClickFunc={onLatestMarkSelect}
                        value={markFullName}
                        options={latestMarks}
                    />
                    </div>

                    <div className="flex-bot-v mrg-top mrg-bot">
                        <Dropdown
                            label="Базовая серия"
                            widthClassName={'input-width-2'}
                            maxInputLength={seriesStringLength}
                            onClickFunc={onSeriesSelect}
                            value={mark.series}
                            options={series}
                        />
                        <div className="mrg-left" />
                        <Dropdown
                            label="Узел"
                            widthClassName={'input-width-3'}
                            maxInputLength={nodeStringLength}
                            onClickFunc={onNodeSelect}
                            value={mark.node}
                            options={nodes}
                        />
                        <div className="mrg-left" />
                        <Dropdown
                            label="Подузел"
                            widthClassName={'input-width-3'}
                            maxInputLength={subnodeStringLength}
                            onClickFunc={onSubnodeSelect}
                            value={mark.subnode}
                            options={subnodes}
                        />
                    </div>
					
                    {/* <div className="flex-bot-v mrg-top mrg-bot">
                        <InputArea label="Базовая серия" widthClassName={'input-width-1'} onChangeFunc={onSeriesSelect} value={mark.series} options={series} />
						<p className="mrg-left mrg-right">.</p>
                        <InputArea label="Узел" widthClassName={'input-width-0'} onChangeFunc={onNodeSelect} value={mark.node} options={nodes} />
						<p className="mrg-left mrg-right">.</p>
                        <InputArea label="Подузел" widthClassName={'input-width-0'} onChangeFunc={onSubnodeSelect} value={mark.subnode} options={subnodes} />
						{isCreateMode ? null : (
							<p className="mrg-left mrg-right">-</p>
						)}
						{isCreateMode ? null : (
                            <InputArea label="Марка" widthClassName={'input-width-0'} onChangeFunc={onCodeSelect} value={mark.code} options={codes} />
						)}
					</div> */}
					{/* <div className="mrg-bot">
                        {mark.code === '' ? null : (
							<div className="mrg-bot">
								<p className="mrg-bot-1">Обозначение марки</p>
								<p className="border input-border-radius input-padding">
									{mark.series+'.'+mark.node+'.'+mark.subnode+'-'+mark.code}
								</p>
							</div>
						)}
						{mark.facilityName === '' ? null : (
							<div className="mrg-bot">
								<p className="mrg-bot-1">
									Наименование комплекса
								</p>
								<p className="border input-border-radius input-padding">
									{mark.facilityName}
								</p>
							</div>
						)}
						{mark.objectName === '' ? null : (
							<div className="mrg-bot">
								<p className="mrg-bot-1">
									Наименование объекта
								</p>
								<p className="border input-border-radius input-padding">
									{mark.objectName}
								</p>
							</div>
						)}
                        {mark.code === '' ? null : (
							<InputArea label="Отдел" widthClassName={'input-width-1'} onChangeFunc={onSubnodeSelect} value={mark.subnode} options={subnodes} />

						)}
                        {mark.gipSurname === '' ? null : (
							<div className="mrg-bot">
								<p className="mrg-bot-1">Фамилия ГИПа</p>
								<p className="border input-border-radius input-padding">
									{mark.gipSurname}
								</p>
							</div>
						)}
					</div> */}

					{isCreateMode ? (
						mark.subnode === '' ? null : (
							<button className="input-border-radius pointer">
								Добавить новую марку
							</button>
						)
					) : (
						mark.mark === '' ? null : <button className="input-border-radius pointer">
							Сохранить изменения
						</button>
					)}

					{/* <div className="space-between-cent-v mrg-bot">
                    <div className="flex">
                        <p>Отдел</p>
                        <select className="input-width-2 mrg-left mrg-right border input-border-radius input-padding">
                            <option>Пункт 1</option>
                            <option>Пункт 2</option>
                        </select>
                    </div>
                    <div className="flex">
                        <p>Шифр марки</p>
                        <select className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                            <option>Пункт 1</option>
                            <option>Пункт 2</option>
                        </select>
                    </div>
                </div>
                <div className="mrg-bot">
                    Таблица
                </div>
                <div className="mrg-bot flex-cent-v">
                    <label htmlFor="agreements">Согласования</label>
                    <input className="mrg-left mrg-right checkbox" type="checkbox" id="agreements" name="agreements" />
                </div>
                <button className="input-border-radius pointer">Добавить новую марку</button> */}
				</div>
			</div>
		</div>
	)
}

export default MarkData
