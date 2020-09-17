import React, { useState, useEffect, useRef } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import axios from 'axios'
import { protocol, host } from '../../env'
import Project from '../../model/Project'
import Node from '../../model/Node'
import Subnode from '../../model/Subnode'
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

    // Default state objects
    const defaultSelectedObject = {
        recentMark: '',
        project: null as Project,
        node: null as Node,
        subnode: null as Subnode,
        mark: null as Mark
    }
    const defaultOptionsObject = {
        recentMarks: [] as Mark[],
        recentSubnodes: [] as Subnode[],
        projects: [] as Project[],
        nodes: [] as Node[],
        subnodes: [] as Subnode[],
        marks: [] as Mark[]
    }

    // States
    // Select and Create modes
    const [isCreateMode, setIsCreateMode] = useState(isCreateModeInitially)
    // Object that holds selected values
    const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
    // Object that holds select options
    const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)
    // Heights
    const [dropdownComponentHeight, setDropdownComponentHeight] = useState(0)
    const [initialInfoSectionHeight, setInitialInfoSectionHeight] = useState(0)
    const [laterInfoSectionHeight, setLaterInfoSectionHeight] = useState(0)
    // Reference for getting heights
    const dropdownComponentHeightRef = useRef()
    const initialInfoSectionHeightRef = useRef()
    const laterInfoSectionHeightRef = useRef()

	useEffect(() => {
        // Cannot use async func as callback in useEffect
        // Function for fetching data
        const fetchData = async () => {
            try {
                // Fetch projects
                const projectsFetchedResponse = await axios.get(protocol + '://' + host + '/api/projects')
                const projectsFetched = projectsFetchedResponse.data

                // Not very nice, but we are not using GraphQL here
                const recentMarksFetchedResponse = await axios.get(protocol + '://' + host + '/api/recent_marks')
                const recentMarksFetched = recentMarksFetchedResponse.data

                const recentSubnodesIds: Array<number> = []
                const recentSubnodesFetched: Array<Subnode> = []
                for (let mark of recentMarksFetched) {
                    if (!recentSubnodesIds.includes(mark.subnode.id)) {
                        recentSubnodesIds.push(mark.subnode.id)
                        recentSubnodesFetched.push(mark.subnode)
                    }
                }

                // Set fetched objects as select options
                setOptionsObject({
                    ...defaultOptionsObject,
                    recentMarks: recentMarksFetched,
                    recentSubnodes: recentSubnodesFetched,
                    projects: projectsFetched
                })
            } catch (e) {
                console.log('Failed to fetch the data')
            }
        }
        fetchData()
        
        // Observe the heights
        const dropdownComponentResizeObserver = new ResizeObserver(([entry]) => {
			setDropdownComponentHeight(entry.target.scrollHeight)
		})
		if (dropdownComponentHeightRef.current) {
			dropdownComponentResizeObserver.observe(dropdownComponentHeightRef.current)
        }

        const initialInfoSectionResizeObserver = new ResizeObserver(([entry]) => {
			setInitialInfoSectionHeight(entry.target.scrollHeight)
		})
		if (initialInfoSectionHeightRef.current) {
			initialInfoSectionResizeObserver.observe(initialInfoSectionHeightRef.current)
        }
        
        const laterInfoSectionResizeObserver = new ResizeObserver(([entry]) => {
			setLaterInfoSectionHeight(entry.target.scrollHeight)
		})
		if (laterInfoSectionHeightRef.current) {
			laterInfoSectionResizeObserver.observe(laterInfoSectionHeightRef.current)
        }
        // Remove height observers on unmount
		return () => {
            dropdownComponentResizeObserver.disconnect()
            initialInfoSectionResizeObserver.disconnect()
            laterInfoSectionResizeObserver.disconnect()
        }
    }, [dropdownComponentHeightRef, initialInfoSectionHeightRef, laterInfoSectionHeightRef])

    // getSpringStyle returns spring animation style
    const getSpringStyle = (obj: any, height: number) => {
        return useSpring({
            from: {
                opacity: 0 as any,
                height: 0,
                overflowY: 'hidden' as any
            },
            to: {
                opacity: obj == null ? (0 as any) : 1,
                height: obj == null ? 0 : height,
                overflowY: obj == null ? ('hidden' as any) : ('visible' as any)
            },
        })
    }

    const marksSpringProp = useSpring({
        from: {
            opacity: 0 as any,
            height: 0,
            overflowY: 'hidden' as any
        },
        to: {
            opacity: selectedObject.subnode == null || isCreateMode ? (0 as any) : 1,
            height: selectedObject.subnode == null || isCreateMode ? 0 : dropdownComponentHeight,
            overflowY: selectedObject.subnode == null ? ('hidden' as any) : ('visible' as any)
        },
    })

    const initialInfoSectionSpringProp = useSpring({
        from: {
            opacity: 0 as any,
            height: 0,
            overflowY: 'hidden' as any
        },
        to: {
            opacity: selectedObject.node == null || isCreateMode ? (0 as any) : 1,
            height: selectedObject.node == null || isCreateMode ? 0 : initialInfoSectionHeight,
            overflowY: selectedObject.node == null ? ('hidden' as any) : ('visible' as any)
        },
    })
    
    const onRecentMarkSelect = (id: number) => {
        let v: Mark = null
        for (let mark of optionsObject.recentMarks) {
            if (mark.id === id) {
                v = mark
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.mark !== null) && (v.id === selectedObject.mark.id)) {
            return
        }

        // setOptionsObject({
        //     ...defaultOptionsObject,
        //     recentMarks: optionsObject.recentMarks,
        //     projects: optionsObject.projects,
        //     nodes: fetchedNodes,
        //     subnodes: fetchedSubnodes,
        //     marks: fetchedMarks
        // })
        // setSelectedObject({
        //     ...defaultSelectedObject,
        //     recentMark: v,
        //     project: 'M32788',
        //     node: '127',
        //     subnode: '33',
        //     mark: 'AVS 1'
        // })

        // try {
        //     const fetchedNodesResponse = await axios.get(protocol + '://' + host + `/api/projects/${id}/nodes`)
        //     const fetchedNodes = fetchedNodesResponse.data
        //     setOptionsObject({
        //         ...defaultOptionsObject,
        //         recentMarks: optionsObject.recentMarks,
        //         projects: optionsObject.projects,
        //         nodes: fetchedNodes
        //     })
        //     setSelectedObject({
        //         ...defaultSelectedObject,
        //         project: v
        //     })
        // } catch (e) {
        //     console.log('Failed to fetch the data')
        // }

        // TBD

        // const v = optionsObject.recentMarks[id]

        // const fetchedNodes: Array<string> = ['527', '127', '134']
        // const fetchedSubnodes: Array<string> = ['11', '22', '33']
        // const fetchedMarks: Array<string> = ['AVS 1', 'RTY 6', 'ZXE111']

        // setOptionsObject({
        //     ...defaultOptionsObject,
        //     recentMarks: optionsObject.recentMarks,
        //     projects: optionsObject.projects,
        //     nodes: fetchedNodes,
        //     subnodes: fetchedSubnodes,
        //     marks: fetchedMarks
        // })
        // setSelectedObject({
        //     ...defaultSelectedObject,
        //     recentMark: v,
        //     project: 'M32788',
        //     node: '127',
        //     subnode: '33',
        //     mark: 'AVS 1'
        // })
    }

    const onRecentSubnodeSelect = (id: number) => {
        let v: Subnode = null
        for (let subnode of optionsObject.recentSubnodes) {
            if (subnode.id === id) {
                v = subnode
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.subnode !== null) && (v.id === selectedObject.subnode.id)) {
            return
        }
    }

    const onProjectSelect = async (id: number) => {
        let v: Project = null
        for (let project of optionsObject.projects) {
            if (project.id === id) {
                v = project
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.project !== null) && (v.id === selectedObject.project.id)) {
            return
        }
        try {
            const fetchedNodesResponse = await axios.get(protocol + '://' + host + `/api/projects/${id}/nodes`)
            const fetchedNodes = fetchedNodesResponse.data
            setOptionsObject({
                ...defaultOptionsObject,
                recentMarks: optionsObject.recentMarks,
                recentSubnodes: optionsObject.recentSubnodes,
                projects: optionsObject.projects,
                nodes: fetchedNodes
            })
            setSelectedObject({
                ...defaultSelectedObject,
                project: v
            })
        } catch (e) {
            console.log('Failed to fetch the data')
        }
	}

    const onNodeSelect = async (id: number) => {
        let v: Node = null
        for (let node of optionsObject.nodes) {
            if (node.id === id) {
                v = node
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.node !== null) && (v.id === selectedObject.node.id)) {
            return
        }
        try {
            const fetchedSubnodesResponse = await axios.get(protocol + '://' + host + `/api/nodes/${id}/subnodes`)
            const fetchedSubnodes = fetchedSubnodesResponse.data
            setOptionsObject({
                ...defaultOptionsObject,
                recentMarks: optionsObject.recentMarks,
                recentSubnodes: optionsObject.recentSubnodes,
                projects: optionsObject.projects,
                nodes: optionsObject.nodes,
                subnodes: fetchedSubnodes
            })
            setSelectedObject({
                ...defaultSelectedObject,
                project: selectedObject.project,
                node: v
            })
        } catch (e) {
            console.log('Failed to fetch the data')
        }
        // let gipSurname = 'Влад'
	}

    const onSubnodeSelect = async (id: number) => {
        let v: Subnode = null
        for (let subnode of optionsObject.subnodes) {
            if (subnode.id === id) {
                v = subnode
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.subnode !== null) && (v.id === selectedObject.subnode.id)) {
            return
        }
        try {
            const fetchedMarksResponse = await axios.get(protocol + '://' + host + `/api/subnodes/${id}/marks`)
            const fetchedMarks = fetchedMarksResponse.data
            setOptionsObject({
                ...defaultOptionsObject,
                recentMarks: optionsObject.recentMarks,
                recentSubnodes: optionsObject.recentSubnodes,
                projects: optionsObject.projects,
                nodes: optionsObject.nodes,
                subnodes: optionsObject.subnodes,
                marks: fetchedMarks
            })
            setSelectedObject({
                ...defaultSelectedObject,
                project: selectedObject.project,
                node: selectedObject.node,
                subnode: v
            })
        } catch (e) {
            console.log('Failed to fetch the data')
        }
		// 	let facilityName =
		// 		'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.'
		// 	let objectName =
		// 		'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.'
    }
    
    const onMarkSelect = (id: number) => {
        let v: Mark = null
        for (let mark of optionsObject.marks) {
            if (mark.id === id) {
                v = mark
                break
            }
        }
        if (v == null) {
            return
        }
        if ((selectedObject.mark !== null) && (v.id === selectedObject.mark.id)) {
            return
        }
        setSelectedObject({
            ...defaultSelectedObject,
            project: selectedObject.project,
            node: selectedObject.node,
            subnode: selectedObject.subnode,
            mark: v
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
                        <InputArea label="Последние марки" widthClassName={'w-latest-marks'} onChangeFunc={onRecentMarkSelect} value={markFullName} options={latestMarks} />
					)} */}
                    {/* <div className="flex"> */}

                    <p className="text-centered section-label">{isCreateMode ? 'Выберите подузел' : 'Выберите марку'}</p>

                    {/* <animated.div style={recentMarksSpringProp}>
						<div>
                    <Dropdown
                        label="Последние марки"
                        widthClassName={'1input-width-1'}
                        maxInputLength={fullNameStringLength}
                        onClickFunc={onRecentMarkSelect}
                        value={selectedObject.recentMark}
                        options={optionsObject}
                    />
                    </div>
					</animated.div> */}

                    {/* <animated.div style={textSpringProp}>
                        <p ref={height2Ref} className="text-centered">или</p>
                    </animated.div> */}

                    <Dropdown
                        label={ isCreateMode ? "Последние подузлы" : "Последние марки"}
                        maxInputLength={fullNameStringLength}
                        onClickFunc={isCreateMode ? onRecentSubnodeSelect : onRecentMarkSelect}
                        value={selectedObject.recentMark}
                        options={isCreateMode ? optionsObject.recentSubnodes.map(s => {
                            const n = s.node
                            const p = n.project
                            const fullName = `${p.baseSeries}.${n.code}.${s.code}`
                            return {
                                id: s.id,
                                val: fullName
                            }
                        }) : optionsObject.recentMarks.map(m => {
                            const s = m.subnode
                            const n = s.node
                            const p = n.project
                            const fullName = isCreateMode ?
                                `${p.baseSeries}.${n.code}.${s.code}` :
                                `${p.baseSeries}.${n.code}.${s.code}-${m.code}`
                            return {
                                id: m.id,
                                val: fullName
                            }
                        })}
                    />

                    <p className="text-centered">или</p>

                    <Dropdown
                        label="Базовая серия"
                        maxInputLength={seriesStringLength}
                        onClickFunc={onProjectSelect}
                        value={selectedObject.project == null ? '' : selectedObject.project.baseSeries}
                        options={optionsObject.projects.map(p => {
                            return {
                                id: p.id,
                                val: p.baseSeries
                            }
                        })}
                    />

                    <animated.div style={getSpringStyle(selectedObject.project, dropdownComponentHeight)}>
						<div ref={dropdownComponentHeightRef}>
                            <Dropdown
                                label="Узел"
                                maxInputLength={nodeStringLength}
                                onClickFunc={onNodeSelect}
                                value={selectedObject.node == null ? '' : selectedObject.node.code}
                                options={optionsObject.nodes.map(n => {
                                    return {
                                        id: n.id,
                                        val: n.code
                                    }
                                })}
                            />
						</div>
					</animated.div>

                    <animated.div style={getSpringStyle(selectedObject.node, dropdownComponentHeight)}>
                        <Dropdown
                            label="Подузел"
                            maxInputLength={subnodeStringLength}
                            onClickFunc={onSubnodeSelect}
                            value={selectedObject.subnode == null ? '' : selectedObject.subnode.code}
                            options={optionsObject.subnodes.map(s => {
                                return {
                                    id: s.id,
                                    val: s.code
                                }
                            })}
                        />
					</animated.div>

                    <animated.div style={marksSpringProp}>
                        <Dropdown
                            label="Марка"
                            maxInputLength={markStringLength}
                            onClickFunc={onMarkSelect}
                            value={selectedObject.mark == null ? '' : selectedObject.mark.code}
                            options={optionsObject.marks.map(m => {
                                return {
                                    id: m.id,
                                    val: m.code
                                }
                            })}
                        />
					</animated.div>

                    

                    <animated.div style={getSpringStyle(selectedObject.node, initialInfoSectionHeight)}>
                        <div ref={initialInfoSectionHeightRef}>
                            <p className="text-centered section-label">Данные марки</p>
                            <div className="flex-v mrg-bottom">
                                <p className="label-area">ГИП</p>
                                <div className="info-area">
                                    { selectedObject.node == null ? '' : selectedObject.node.chiefEngineer.fullName  }
                                </div>
                            </div>
                        </div>
                    </animated.div>

                    <animated.div style={getSpringStyle(selectedObject.mark, laterInfoSectionHeight)}>
                        <div ref={laterInfoSectionHeightRef}>
                            <div className="mrg-top" />
                            <div className="flex-v mrg-bottom">
                                <p className="label-area">Наименование комплекса</p>
                                <div className="info-area">
                                Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века.
                                </div>
                            </div>

                            <div className="flex-v mrg-bottom">
                                <p className="label-area">Наименование объекта</p>
                                <div className="info-area">
                                Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века.
                                </div>
                            </div>

                            {isCreateMode ? (
                                selectedObject.subnode === null ? null : (
                                    <button className="input-border-radius pointer">
                                        Добавить новую марку
                                    </button>
                                )
                            ) : (
                                selectedObject.mark === null ? null : <button className="input-border-radius pointer">
                                    Сохранить изменения
                                </button>
                            )}
                        </div>
                    </animated.div>
                    
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
