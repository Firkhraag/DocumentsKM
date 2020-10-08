import React, { useState, useEffect } from 'react'
import { useSpring, animated } from 'react-spring'
import { useHistory } from 'react-router-dom'
import httpClient from '../../axios'
import Project from '../../model/Project'
import Node from '../../model/Node'
import Subnode from '../../model/Subnode'
import Mark from '../../model/Mark'
import Dropdown from '../Dropdown/Dropdown'
import { makeMarkOrSubnodeName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import './MarkSelect.css'

const MarkSelect = () => {
	// Can use resize-observer-polyfill instead
	const nodeDivHeight = 82
	const markDivHeight = 62
	const buttonDivHeight = 72

	// Max lengths of input fields strings
	const projectSeriesStringLength = 30
	const nodeCodeStringLength = 10
	const subnodeCodeStringLength = 10
	const markCodeStringLength = 40
	const subnodeNameStringLength = 50
	const markNameStringLength = 90

	// Default state objects
	const defaultSelectedObject = {
		recentMarkString: '',
		recentSubnodeString: '',
		project: null as Project,
		node: null as Node,
		subnode: null as Subnode,
		mark: null as Mark,
	}
	const defaultOptionsObject = {
		recentMarks: [] as Mark[],
		recentSubnodes: [] as Subnode[],
		projects: [] as Project[],
		nodes: [] as Node[],
		subnodes: [] as Subnode[],
		marks: [] as Mark[],
	}

	const history = useHistory()

	// States
	// Select and Create modes
	const [isCreateMode, setIsCreateMode] = useState(false)
	// Object that holds selected values
	const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
	// Object that holds select options
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	useEffect(() => {
		// Cannot use async func as callback in useEffect
		// Function for fetching data
		const fetchData = async () => {
			try {
				const projectsFetchedResponse = await httpClient.get(
					'/projects'
				)
				const projectsFetched = projectsFetchedResponse.data

				const recentMarks = [] as Mark[]
				const recentMarkIdsStr = localStorage.getItem('recentMarkIds')
				if (recentMarkIdsStr != null) {
					const recentMarkIds = JSON.parse(
						recentMarkIdsStr
					) as number[]
					for (let id of recentMarkIds) {
						const markFetchedResponse = await httpClient.get(
							`/marks/${id}/parents`
						)
						recentMarks.push(markFetchedResponse.data)
					}
				}

				const recentSubnodes = [] as Subnode[]
				const recentSubnodeIdsStr = localStorage.getItem(
					'recentSubnodeIds'
				)
				if (recentSubnodeIdsStr != null) {
					const recentSubnodeIds = JSON.parse(
						recentSubnodeIdsStr
					) as number[]
					for (let id of recentSubnodeIds) {
						const subnodeFetchedResponse = await httpClient.get(
							`/subnodes/${id}/parents`
						)
						recentSubnodes.push(subnodeFetchedResponse.data)
					}
				}

				// Set fetched objects as select options
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: recentMarks,
					recentSubnodes: recentSubnodes,
					projects: projectsFetched,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
		fetchData()
	}, [])

	const getSpringStyle = (isClosed: boolean, height: number) => {
		return useSpring({
			from: {
				opacity: 0 as any,
				height: 0,
				overflowY: 'hidden' as any,
			},
			to: {
				opacity: isClosed ? (0 as any) : 1,
				height: isClosed ? 0 : height,
				overflowY: isClosed ? ('hidden' as any) : ('visible' as any),
			},
		})
	}

	const onRecentMarkSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.recentMarks,
			selectedObject.mark
		)
		if (v != null) {
			const s = v.subnode
			const n = s.node
			const p = n.project
			try {
				const fetchedNodesResponse = await httpClient.get(
					`/projects/${p.id}/nodes`
				)
				const fetchedNodes = fetchedNodesResponse.data
				const fetchedSubnodesResponse = await httpClient.get(
					`/nodes/${n.id}/subnodes`
				)
				const fetchedSubnodes = fetchedSubnodesResponse.data
				const fetchedMarksResponse = await httpClient.get(
					`/subnodes/${s.id}/marks`
				)
				const fetchedMarks = fetchedMarksResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					recentSubnodes: optionsObject.recentSubnodes,
					projects: optionsObject.projects,
					nodes: fetchedNodes,
					subnodes: fetchedSubnodes,
					marks: fetchedMarks,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
			setSelectedObject({
				...defaultSelectedObject,
				recentMarkString: makeMarkOrSubnodeName(
					p.baseSeries,
					n.code,
					s.code,
					v.code
				),
				project: p,
				node: n,
				subnode: s,
				mark: v,
			})
		}
	}

	const onRecentSubnodeSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.recentSubnodes,
			selectedObject.subnode
		)
		if (v != null) {
			const n = v.node
			const p = n.project
			try {
				const fetchedNodesResponse = await httpClient.get(
					`/projects/${p.id}/nodes`
				)
				const fetchedNodes = fetchedNodesResponse.data
				const fetchedSubnodesResponse = await httpClient.get(
					`/nodes/${n.id}/subnodes`
				)
				const fetchedSubnodes = fetchedSubnodesResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					recentSubnodes: optionsObject.recentSubnodes,
					projects: optionsObject.projects,
					nodes: fetchedNodes,
					subnodes: fetchedSubnodes,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
			setSelectedObject({
				...defaultSelectedObject,
				recentSubnodeString: makeMarkOrSubnodeName(
					p.baseSeries,
					n.code,
					v.code
				),
				project: p,
				node: n,
				subnode: v,
			})
		}
	}

	const onProjectSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.projects,
			selectedObject.project
		)
		if (v != null) {
			try {
				const fetchedNodesResponse = await httpClient.get(
					`/projects/${id}/nodes`
				)
				const fetchedNodes = fetchedNodesResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					recentSubnodes: optionsObject.recentSubnodes,
					projects: optionsObject.projects,
					nodes: fetchedNodes,
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: v,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
	}

	const onNodeSelect = async (id: number) => {
		const v = getFromOptions(id, optionsObject.nodes, selectedObject.node)
		if (v != null) {
			try {
				const fetchedSubnodesResponse = await httpClient.get(
					`/nodes/${id}/subnodes`
				)
				const fetchedSubnodes = fetchedSubnodesResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					recentSubnodes: optionsObject.recentSubnodes,
					projects: optionsObject.projects,
					nodes: optionsObject.nodes,
					subnodes: fetchedSubnodes,
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: selectedObject.project,
					node: v,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
	}

	const onSubnodeSelect = async (id: number) => {
		const v = getFromOptions(
			id,
			optionsObject.subnodes,
			selectedObject.subnode
		)
		if (v != null) {
			try {
				const fetchedMarksResponse = await httpClient.get(
					`/subnodes/${id}/marks`
				)
				const fetchedMarks = fetchedMarksResponse.data
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					recentSubnodes: optionsObject.recentSubnodes,
					projects: optionsObject.projects,
					nodes: optionsObject.nodes,
					subnodes: optionsObject.subnodes,
					marks: fetchedMarks,
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: selectedObject.project,
					node: selectedObject.node,
					subnode: v,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
		}
	}

	const onMarkSelect = (id: number) => {
		const v = getFromOptions(id, optionsObject.marks, selectedObject.mark)
		if (v != null) {
			setSelectedObject({
				...defaultSelectedObject,
				project: selectedObject.project,
				node: selectedObject.node,
				subnode: selectedObject.subnode,
				mark: v,
			})
		}
	}

	const onSelectMarkButtonClick = () => {
		const mark = selectedObject.mark
		mark.subnode = selectedObject.subnode
		mark.subnode.node = selectedObject.node
		mark.subnode.node.project = selectedObject.project
		localStorage.setItem('selectedMarkId', mark.id.toString())

		const filteredRecentMarks = optionsObject.recentMarks.filter(
			(m) => m.id !== mark.id
		)
		if (filteredRecentMarks.length >= 5) {
			filteredRecentMarks.shift()
		}
		filteredRecentMarks.unshift(mark)
		let resStr = JSON.stringify(filteredRecentMarks.map((m) => m.id))
		localStorage.setItem('recentMarkIds', resStr)

		const filteredRecentSubnodes = optionsObject.recentSubnodes.filter(
			(s) => s.id !== mark.subnode.id
		)
		if (filteredRecentSubnodes.length >= 5) {
			filteredRecentSubnodes.shift()
		}
		filteredRecentSubnodes.unshift(mark.subnode)
		resStr = JSON.stringify(filteredRecentSubnodes.map((s) => s.id))
		localStorage.setItem('recentSubnodeIds', resStr)

		history.push('/mark-data')
    }
    
    const onSelectSubnodeButtonClick = () => {
		const subnode = selectedObject.subnode
		subnode.node = selectedObject.node
		subnode.node.project = selectedObject.project

		const filteredRecentSubnodes = optionsObject.recentSubnodes.filter(
			(s) => s.id !== subnode.id
		)
		if (filteredRecentSubnodes.length >= 5) {
			filteredRecentSubnodes.shift()
		}
		filteredRecentSubnodes.unshift(subnode)
		const resStr = JSON.stringify(filteredRecentSubnodes.map((s) => s.id))
		localStorage.setItem('recentSubnodeIds', resStr)

        history.push('/mark-create')
	}

	return (
		<div className="mark-data-cnt">
			<h1 className="text-centered">Выбрать / создать марку</h1>
			<div className="tabs component-width white-bg">
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
				<label htmlFor="tab-btn-2">Создать</label>

				<div className="flex-v">
					<p className="text-centered section-label">
						{isCreateMode ? 'Выберите подузел' : 'Выберите марку'}
					</p>

					<Dropdown
						cntStyle="flex-v mrg-bot"
						label={
							isCreateMode
								? 'Последние подузлы'
								: 'Последние марки'
						}
						placeholder={'Не выбрано'}
						maxInputLength={
							isCreateMode
								? subnodeNameStringLength
								: markNameStringLength
						}
						onClickFunc={
							isCreateMode
								? onRecentSubnodeSelect
								: onRecentMarkSelect
						}
						value={
							isCreateMode
								? selectedObject.recentSubnodeString
								: selectedObject.recentMarkString
						}
						options={
							isCreateMode
								? optionsObject.recentSubnodes.map((s) => {
										const n = s.node
										const p = n.project
										const fullName = makeMarkOrSubnodeName(
											p.baseSeries,
											n.code,
											s.code
										)
										return {
											id: s.id,
											val: fullName,
										}
								  })
								: optionsObject.recentMarks.map((m) => {
										const s = m.subnode
										const n = s.node
										const p = n.project
										const fullName = makeMarkOrSubnodeName(
											p.baseSeries,
											n.code,
											s.code,
											m.code
										)
										return {
											id: m.id,
											val: fullName,
										}
								  })
						}
					/>

					<p className="text-centered">или</p>

					<Dropdown
						cntStyle="flex-v mrg-bot"
						label="Базовая серия"
						placeholder={'Выберите базовую серию'}
						maxInputLength={projectSeriesStringLength}
						onClickFunc={onProjectSelect}
						value={
							selectedObject.project == null
								? ''
								: selectedObject.project.baseSeries
						}
						options={optionsObject.projects.map((p) => {
							return {
								id: p.id,
								val: p.baseSeries,
							}
						})}
					/>
					<animated.div
						style={getSpringStyle(
							selectedObject.project == null,
							nodeDivHeight
						)}
					>
						<Dropdown
							cntStyle="flex-v mrg-bot"
							label="Узел"
							placeholder={'Выберите узел'}
							maxInputLength={nodeCodeStringLength}
							onClickFunc={onNodeSelect}
							value={
								selectedObject.node == null
									? ''
									: selectedObject.node.code
							}
							options={optionsObject.nodes.map((n) => {
								return {
									id: n.id,
									val: n.code,
								}
							})}
						/>
					</animated.div>
					<animated.div
						style={getSpringStyle(
							selectedObject.node == null,
							isCreateMode ? markDivHeight : nodeDivHeight
						)}
					>
						<Dropdown
							cntStyle={
								isCreateMode ? 'flex-v' : 'flex-v mrg-bot'
							}
							label="Подузел"
							placeholder={'Выберите подузел'}
							maxInputLength={subnodeCodeStringLength}
							onClickFunc={onSubnodeSelect}
							value={
								selectedObject.subnode == null
									? ''
									: selectedObject.subnode.code
							}
							options={optionsObject.subnodes.map((s) => {
								return {
									id: s.id,
									val: s.code,
								}
							})}
						/>
					</animated.div>
					<animated.div
						style={getSpringStyle(
							selectedObject.subnode == null || isCreateMode,
							markDivHeight
						)}
					>
						<Dropdown
							cntStyle="flex-v"
							label="Марка"
							placeholder={'Выберите марку'}
							maxInputLength={markCodeStringLength}
							onClickFunc={onMarkSelect}
							value={
								selectedObject.mark == null
									? ''
									: selectedObject.mark.code
							}
							options={optionsObject.marks.map((m) => {
								return {
									id: m.id,
									val: m.code,
								}
							})}
						/>
					</animated.div>
					<animated.div
						style={getSpringStyle(
							isCreateMode
								? selectedObject.subnode == null
								: selectedObject.mark == null,
							buttonDivHeight
						)}
					>
						<button
							onClick={isCreateMode ? onSelectSubnodeButtonClick : onSelectMarkButtonClick}
							className="final-btn input-border-radius pointer"
						>
							{isCreateMode ? 'Выбрать подузел' : 'Выбрать марку'}
						</button>
					</animated.div>
				</div>
			</div>
		</div>
	)
}

export default MarkSelect
