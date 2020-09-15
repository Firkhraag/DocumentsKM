import React, { useState, useEffect, useRef } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import Mark from '../../model/Mark'
import Dropdown from './Dropdown'
import './MarkData.css'

type MarkDataProps = {
	isCreateModeInitially: boolean
}

const MarkData = ({ isCreateModeInitially }: MarkDataProps) => {

    // Max lengths of input fields strings
    const seriesStringLength = 30
    const nodeStringLength = 10
    const subnodeStringLength = 10
    const markStringLength = 40
    const fullNameStringLength = 90

    // Select and Create modes
	const [isCreateMode, setIsCreateMode] = useState<boolean>(
		isCreateModeInitially
    )
    
	const [mark, setMark] = useState<Mark>(new Mark(null))
	const [projects, setProjects] = useState<Array<string>>([])
	const [nodes, setNodes] = useState<Array<string>>([])
	const [subnodes, setSubnodes] = useState<Array<string>>([])
    const [marks, setMarks] = useState<Array<string>>([])
    
    const [markFullName, setMarkFullName] = useState('')
    const [latestMarks, setLatestMarks] = useState<string[]>([])

    const nodeRef = useRef()
    const [dropdownHeight, setDropdownHeight] = useState(0)

	useEffect(() => {
		const seriesFetched: Array<string> = ['M32788', 'V32788', 'G32788']
        setProjects(seriesFetched)
        
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
        
        const ro = new ResizeObserver(([entry]) => {
			setDropdownHeight(entry.target.scrollHeight)
		})

		if (nodeRef.current) {
			ro.observe(nodeRef.current)
		}

		return () => ro.disconnect()
    }, [nodeRef])

    const nodeSpringProp = useSpring({
		from: { opacity: 0 as any, height: 0 },
		to: {
            opacity: mark.project !== '' ? 1 : (0 as any),
            height: mark.project !== '' ? dropdownHeight : 0,
        },
    })
    
    const subnodeSpringProp = useSpring({
		from: { opacity: 0 as any, height: 0 },
		to: {
            opacity: mark.node !== '' ? 1 : (0 as any),
            height: mark.node !== '' ? dropdownHeight : 0,
        },
    })
    
    const markSpringProp = useSpring({
		from: { opacity: 0 as any, height: 0 },
		to: {
            opacity: mark.subnode !== '' ? 1 : (0 as any),
            height: mark.subnode !== '' ? dropdownHeight : 0,
        },
	})
    
    const onLatestMarkSelect = (event: React.MouseEvent<HTMLDivElement>) => {
        const v = event.currentTarget.textContent
        setNodes([])
        setSubnodes([])
		setMarks([])
		setMark({ ...new Mark(null), project: v })
		setMarkFullName(v)
    }

    const onProjectSelect = (event: React.MouseEvent<HTMLDivElement>) => {
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
		setMark({ ...new Mark(null), project: v })
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
			project: mark.project,
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
			<h1 className="text-centered">Марки</h1>
			<div className="tabs">
				<input
					type="radio"
					name="tab-btn"
					id="tab-btn-1"
					value=""
					onChange={() => setIsCreateMode(false)}
					checked={isCreateMode ? false : true}
				/>
				<label htmlFor="tab-btn-1">Редактировать</label>
				<input
					type="radio"
					name="tab-btn"
					id="tab-btn-2"
					value=""
					onChange={() => setIsCreateMode(true)}
					checked={isCreateMode ? true : false}
				/>
				<label htmlFor="tab-btn-2">Добавить</label>

				<div className="flex-v">
					{/* {isCreateMode ? null : (
                        <InputArea label="Последние марки" widthClassName={'w-latest-marks'} onChangeFunc={onLatestMarkSelect} value={markFullName} options={latestMarks} />
					)} */}
                    {/* <div className="flex"> */}

                    <p className="text-centered section-label">Выберите марку</p>

                    <Dropdown
                        label="Последние марки"
                        widthClassName={'1input-width-1'}
                        maxInputLength={fullNameStringLength}
                        onClickFunc={onLatestMarkSelect}
                        value={markFullName}
                        options={latestMarks}
                    />
                    {/* </div> */}

                    <p className="text-centered">или</p>

                    <Dropdown
                            label="Базовая серия"
                            widthClassName={'1input-width-2'}
                            maxInputLength={seriesStringLength}
                            onClickFunc={onProjectSelect}
                            value={mark.project}
                            options={projects}
                        />

                    <animated.div className="answer" style={nodeSpringProp}>
						<div ref={nodeRef}>
                            <Dropdown
                                label="Узел"
                                widthClassName={'1input-width-3'}
                                maxInputLength={nodeStringLength}
                                onClickFunc={onNodeSelect}
                                value={mark.node}
                                options={nodes}
                            />
						</div>
					</animated.div>

                    <animated.div className="answer" style={subnodeSpringProp}>
						<div>
                            <Dropdown
                                label="Подузел"
                                widthClassName={'1input-width-3'}
                                maxInputLength={subnodeStringLength}
                                onClickFunc={onSubnodeSelect}
                                value={mark.subnode}
                                options={subnodes}
                            />
						</div>
					</animated.div>

                    <animated.div className="answer" style={markSpringProp}>
						<div>
                            <Dropdown
                                label="Марка"
                                widthClassName={'1input-width-3'}
                                maxInputLength={markStringLength}
                                onClickFunc={onMarkSelect}
                                value={mark.mark}
                                options={marks}
                            />
						</div>
					</animated.div>

                    <p className="text-centered">Данные марки</p>
                    
                    {/* <p className="text-centered">Информация</p>

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
                        {mark.mark === '' ? null : (
							<InputArea label="Отдел" widthClassName={'input-width-1'} onChangeFunc={onSubnodeSelect} value={mark.subnode} options={subnodes} />

						)}
                        {mark.gipSurname === '' ? null : (
							<div className="mrg-bot">
								<p className="mrg-bot-1">Фамилия ГИПа</p>
								<p className="border input-border-radius input-padding">
									{mark.gipSurname}
								</p>
							</div>
						)} */}


                        {/* <Dropdown
                            label="Узел"
                            widthClassName={'1input-width-3'}
                            maxInputLength={nodeStringLength}
                            onClickFunc={onNodeSelect}
                            value={mark.node}
                            options={nodes}
                        />
                        <Dropdown
                            label="Подузел"
                            widthClassName={'1input-width-3'}
                            maxInputLength={subnodeStringLength}
                            onClickFunc={onSubnodeSelect}
                            value={mark.subnode}
                            options={subnodes}
                        /> */}

                    {/* <div className="flex-bot-v mrg-top mrg-bot">
                        
                    </div> */}
					
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
