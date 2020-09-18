import React, { useState, useEffect, useRef } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import axios from 'axios'
import { protocol, host } from '../../env'
import Project from '../../model/Project'
import Node from '../../model/Node'
import Subnode from '../../model/Subnode'
import Mark from '../../model/Mark'
import Dropdown from '../Dropdown/Dropdown'
import './MarkSelect.css'

const MarkSelect = () => {
    // Max lengths of input fields strings
    const projectSeriesStringLength = 30
    const nodeCodeStringLength = 10
    const subnodeCodeStringLength = 10
    const markCodeStringLength = 40
    const subnodeNameStringLength = 50
    const markNameStringLength = 90

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
    const [isCreateMode, setIsCreateMode] = useState(false)
    // Object that holds selected values
    const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
    // Object that holds select options
    const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)
    // Heights
    const [nodeHeight, setNodeHeight] = useState(0)
    const [markHeight, setMarkHeight] = useState(0)
    const [buttonHeight, setButtonHeight] = useState(0)
    // Reference for getting heights
    const nodeHeightRef = useRef()
    const markHeightRef = useRef()
    const buttonHeightRef = useRef()

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
        const nodeHeightObserver = new ResizeObserver(([entry]) => {
			setNodeHeight(entry.target.scrollHeight)
		})
		if (nodeHeightRef.current) {
			nodeHeightObserver.observe(nodeHeightRef.current)
        }

        const markHeightObserver = new ResizeObserver(([entry]) => {
			setMarkHeight(entry.target.scrollHeight)
		})
		if (markHeightRef.current) {
			markHeightObserver.observe(markHeightRef.current)
        }

        const buttonResizeObserver = new ResizeObserver(([entry]) => {
			setButtonHeight(entry.target.scrollHeight)
		})
		if (buttonHeightRef.current) {
			buttonResizeObserver.observe(buttonHeightRef.current)
        }
		return () => {
            nodeHeightObserver.disconnect()
            markHeightObserver.disconnect()
            buttonResizeObserver.disconnect()
        }
    }, [nodeHeightRef, markHeightRef, buttonHeightRef])

    // getSpringStyle returns spring animation style
    const getSpringStyle = (isClosed: boolean, height: number) => {
        return useSpring({
            from: {
                opacity: 0 as any,
                height: 0,
                overflowY: 'hidden' as any
            },
            to: {
                opacity: isClosed ? (0 as any) : 1,
                height: isClosed ? 0 : height,
                overflowY: isClosed ? ('hidden' as any) : ('visible' as any)
            },
        })
    }
    
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
			<h1 className="text-centered">Выбрать марку</h1>
			<div className="tabs component-width">
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

				<div className="flex-v">

                    <p className="text-centered section-label">{isCreateMode ? 'Выберите подузел' : 'Выберите марку'}</p>

                    <Dropdown
                        cntStyle="flex-v mrg-bot"
                        label={ isCreateMode ? "Последние подузлы" : "Последние марки"}
                        maxInputLength={ isCreateMode ? subnodeNameStringLength : markNameStringLength }
                        onClickFunc={isCreateMode ? onRecentSubnodeSelect : onRecentMarkSelect}
                        value={selectedObject.recentMark} // TBD
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
                        cntStyle="flex-v mrg-bot"
                        label="Базовая серия"
                        maxInputLength={projectSeriesStringLength}
                        onClickFunc={onProjectSelect}
                        value={selectedObject.project == null ? '' : selectedObject.project.baseSeries}
                        options={optionsObject.projects.map(p => {
                            return {
                                id: p.id,
                                val: p.baseSeries
                            }
                        })}
                    />

                    <animated.div style={getSpringStyle(selectedObject.project == null, nodeHeight)}>
						<div ref={nodeHeightRef}>
                            <Dropdown
                                cntStyle="flex-v mrg-bot"
                                label="Узел"
                                maxInputLength={nodeCodeStringLength}
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

                    <animated.div style={getSpringStyle(selectedObject.node == null, isCreateMode ? markHeight : nodeHeight)}>
                        <Dropdown
                            cntStyle={ isCreateMode ? "flex-v" : "flex-v mrg-bot"}
                            label="Подузел"
                            maxInputLength={subnodeCodeStringLength}
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

                    <animated.div style={getSpringStyle(selectedObject.subnode == null || isCreateMode, markHeight)}>
                        <div ref={markHeightRef}>
                            <Dropdown
                                cntStyle="flex-v"
                                label="Марка"
                                maxInputLength={markCodeStringLength}
                                onClickFunc={onMarkSelect}
                                value={selectedObject.mark == null ? '' : selectedObject.mark.code}
                                options={optionsObject.marks.map(m => {
                                    return {
                                        id: m.id,
                                        val: m.code
                                    }
                                })}
                            />
                            </div>
					</animated.div>

                    <animated.div style={getSpringStyle(isCreateMode ? selectedObject.subnode == null : selectedObject.mark == null, buttonHeight)}>
                        <div ref={buttonHeightRef}>
                            <button className="final-btn input-border-radius pointer">
                                { isCreateMode ? 'Добавить новую марку' : 'Выбрать марку'}
                            </button>
                        </div>
                    </animated.div>

                    {/* <animated.div style={getSpringStyle(selectedObject.node, buttonHeight)}>
                        <div ref={buttonHeightRef}>
                            {isCreateMode ? (
                                selectedObject.subnode === null ? null : (
                                    <button className="final-btn input-border-radius pointer">
                                        Добавить новую марку
                                    </button>
                                )
                            ) : (
                                selectedObject.mark === null ? null : <button className="final-btn input-border-radius pointer">
                                    Выбрать марку
                                </button>
                            )}
                        </div>
                    </animated.div> */}
				</div>
			</div>
		</div>
	)
}

export default MarkSelect
