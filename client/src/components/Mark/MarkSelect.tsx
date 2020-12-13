// Global
import React, { useState, useEffect } from 'react'
import { useHistory } from 'react-router-dom'
import Select from 'react-select'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Components
import ErrorMsg from '../ErrorMsg/ErrorMsg'
// Util
import Project from '../../model/Project'
import Node from '../../model/Node'
import Subnode from '../../model/Subnode'
import Mark from '../../model/Mark'
import httpClient from '../../axios'
import { makeMarkName } from '../../util/make-name'
import getFromOptions from '../../util/get-from-options'
import { reactSelectstyle } from '../../util/react-select-style'
import { useSetMark } from '../../store/MarkStore'

type MarkSelectProps = {
	setSubnode: (v: Subnode) => void
}

const MarkSelect = ({ setSubnode }: MarkSelectProps) => {
	const defaultSelectedObject = {
		recentMark: null as Mark,
		project: null as Project,
		node: null as Node,
		subnode: null as Subnode,
		mark: null as Mark,
	}
	const defaultOptionsObject = {
		recentMarks: [] as Mark[],
		projects: [] as Project[],
		nodes: [] as Node[],
		subnodes: [] as Subnode[],
		marks: [] as Mark[],
	}

	const history = useHistory()
	const setMark = useSetMark()

	const [selectedObject, setSelectedObject] = useState(defaultSelectedObject)
	const [optionsObject, setOptionsObject] = useState(defaultOptionsObject)

	const cachedNodes = useState(new Map<number, Node[]>())[0]
	const cachedSubnodes = useState(new Map<number, Subnode[]>())[0]
	const cachedMarks = useState(new Map<number, Mark[]>())[0]

	const [errMsg, setErrMsg] = useState('')

	useEffect(() => {
		const fetchData = async () => {
			let recentMarks = [] as Mark[]
			try {
				const recentMarkIdsStr = localStorage.getItem('recentMarkIds')
				if (recentMarkIdsStr != null) {
					const recentMarkIds = JSON.parse(
						recentMarkIdsStr
					) as number[]
					for (let id of recentMarkIds) {
						const markResponse = await httpClient.get(
							`/marks/${id}/parents`
						)
						recentMarks.push(markResponse.data)
					}
				}
			} catch (e) {
				console.log('Failed to get recent marks or subnodes')
			}
			try {
				const projectsResponse = await httpClient.get('/projects')
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: recentMarks,
					projects: projectsResponse.data,
				})
			} catch (e) {
				console.log('Failed to fetch projects')
			}
		}
		fetchData()
	}, [])

	const onRecentMarkSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...defaultOptionsObject,
			})
			setSelectedObject({
				...defaultSelectedObject,
			})
			return
		}
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
				const nodesResponse = await httpClient.get(
					`/projects/${p.id}/nodes`
				)
				const subnodesResponse = await httpClient.get(
					`/nodes/${n.id}/subnodes`
				)
				const marksResponse = await httpClient.get(
					`/subnodes/${s.id}/marks`
				)
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					projects: optionsObject.projects,
					nodes: nodesResponse.data,
					subnodes: subnodesResponse.data,
					marks: marksResponse.data,
				})
			} catch (e) {
				console.log('Failed to fetch the data')
			}
			setSelectedObject({
				...defaultSelectedObject,
				recentMark: v,
				project: p,
				node: n,
				subnode: s,
				mark: v,
			})
		}
	}

	const onProjectSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...defaultOptionsObject,
				recentMarks: optionsObject.recentMarks,
				projects: optionsObject.projects,
			})
			setSelectedObject({
				...defaultSelectedObject,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.projects,
			selectedObject.project
		)
		if (v != null) {
			if (cachedNodes.has(v.id)) {
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					projects: optionsObject.projects,
					nodes: cachedNodes.get(v.id),
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: v,
				})
			} else {
				try {
					const nodesResponse = await httpClient.get(
						`/projects/${id}/nodes`
					)
					cachedNodes.set(v.id, nodesResponse.data)
					setOptionsObject({
						...defaultOptionsObject,
						recentMarks: optionsObject.recentMarks,
						projects: optionsObject.projects,
						nodes: nodesResponse.data,
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
	}

	const onNodeSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...defaultOptionsObject,
				recentMarks: optionsObject.recentMarks,
				projects: optionsObject.projects,
				nodes: optionsObject.nodes,
			})
			setSelectedObject({
				...defaultSelectedObject,
				project: selectedObject.project,
			})
			return
		}
		const v = getFromOptions(id, optionsObject.nodes, selectedObject.node)
		if (v != null) {
			if (cachedSubnodes.has(v.id)) {
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					projects: optionsObject.projects,
					nodes: optionsObject.nodes,
					subnodes: cachedSubnodes.get(v.id),
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: selectedObject.project,
					node: v,
				})
			} else {
				try {
					const subnodesResponse = await httpClient.get(
						`/nodes/${id}/subnodes`
					)
					cachedSubnodes.set(v.id, subnodesResponse.data)
					setOptionsObject({
						...defaultOptionsObject,
						recentMarks: optionsObject.recentMarks,
						projects: optionsObject.projects,
						nodes: optionsObject.nodes,
						subnodes: subnodesResponse.data,
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
	}

	const onSubnodeSelect = async (id: number) => {
		if (id == null) {
			setOptionsObject({
				...defaultOptionsObject,
				recentMarks: optionsObject.recentMarks,
				projects: optionsObject.projects,
				nodes: optionsObject.nodes,
				subnodes: optionsObject.subnodes,
			})
			setSelectedObject({
				...defaultSelectedObject,
				project: selectedObject.project,
				node: selectedObject.node,
			})
			return
		}
		const v = getFromOptions(
			id,
			optionsObject.subnodes,
			selectedObject.subnode
		)
		if (v != null) {
			if (cachedMarks.has(v.id)) {
				setOptionsObject({
					...defaultOptionsObject,
					recentMarks: optionsObject.recentMarks,
					projects: optionsObject.projects,
					nodes: optionsObject.nodes,
					subnodes: optionsObject.subnodes,
					marks: cachedMarks.get(v.id),
				})
				setSelectedObject({
					...defaultSelectedObject,
					project: selectedObject.project,
					node: selectedObject.node,
					subnode: v,
				})
			} else {
				try {
					const marksResponse = await httpClient.get(
						`/subnodes/${id}/marks`
					)
					cachedMarks.set(v.id, marksResponse.data)
					setOptionsObject({
						...defaultOptionsObject,
						recentMarks: optionsObject.recentMarks,
						projects: optionsObject.projects,
						nodes: optionsObject.nodes,
						subnodes: optionsObject.subnodes,
						marks: marksResponse.data,
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
	}

	const onMarkSelect = (id: number) => {
		if (id == null) {
			setSelectedObject({
				...defaultSelectedObject,
				project: selectedObject.project,
				node: selectedObject.node,
				subnode: selectedObject.subnode,
			})
			return
		}
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
		if (
			selectedObject.mark == null ||
			selectedObject.subnode == null ||
			selectedObject.node == null ||
			selectedObject.project == null
		) {
			setErrMsg('Пожалуйста, заполните необходимые поля')
			return
		}
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

		const m = selectedObject.mark
		m.subnode = selectedObject.subnode
		m.subnode.node = selectedObject.node
		m.subnode.node.project = selectedObject.project
		setMark(m)
		history.push('/')
	}

	const onCreateMarkButtonClick = () => {
		if (
			selectedObject.subnode == null ||
			selectedObject.node == null ||
			selectedObject.project == null
		) {
			setErrMsg('Пожалуйста, заполните необходимые поля')
			return
		}
		const subnode = selectedObject.subnode
		subnode.node = selectedObject.node
        subnode.node.project = selectedObject.project
        setSubnode(subnode)
		history.push('/mark-create')
	}

	return (
		<div className="component-cnt">
			<h1 className="text-centered">Выбор / создание марки</h1>
			<div className="shadow p-3 mb-5 bg-white rounded component-cnt-div">
                <Form.Group>
                    <Form.Label
						htmlFor="recentMarks"
					>
						Последние марки
					</Form.Label>
					<Select
                        inputId="recentMarks"
						maxMenuHeight={250}
						isClearable={true}
						isSearchable={true}
						placeholder="Выберите последнюю марку"
						noOptionsMessage={() => 'Марки не найдены'}
						onChange={(selectedOption) =>
							onRecentMarkSelect((selectedOption as any)?.value)
						}
						value={
							selectedObject.recentMark == null
								? null
								: {
										value: selectedObject.recentMark.id,
										label: makeMarkName(
											selectedObject.recentMark.subnode
												.node.project.baseSeries,
											selectedObject.recentMark.subnode
												.node.code,
											selectedObject.recentMark.subnode
												.code,
											selectedObject.recentMark.code
										),
								  }
						}
						options={optionsObject.recentMarks.map((m) => {
							return {
								value: m.id,
								label: makeMarkName(
									m.subnode.node.project.baseSeries,
									m.subnode.node.code,
									m.subnode.code,
									m.code
								),
							}
						})}
						styles={reactSelectstyle}
					/>
				</Form.Group>

				<div className="flex mrg-top-2">
                    <Form.Group className="no-bot-mrg">
                        <Form.Label
                            htmlFor="project"
                        >
                            Базовая серия
                        </Form.Label>
						<Select
                            inputId="project"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите базовую серию"
							noOptionsMessage={() => 'Базовые серии не найдены'}
							className="input-width-2"
							onChange={(selectedOption) =>
								onProjectSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.project == null
									? null
									: {
											value: selectedObject.project.id,
											label:
												selectedObject.project
													.baseSeries === ''
													? '-'
													: selectedObject.project
															.baseSeries,
									  }
							}
							options={optionsObject.projects.map((p) => {
								return {
									value: p.id,
									label:
										p.baseSeries === ''
											? '-'
											: p.baseSeries,
								}
							})}
							styles={reactSelectstyle}
						/>
					</Form.Group>
					<Form.Group className="mrg-left no-bot-mrg">
                        <Form.Label
                            htmlFor="node"
                        >
                            Узел
                        </Form.Label>
						<Select
                            inputId="node"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите узел"
							className="input-width-3"
							noOptionsMessage={() => 'Узлы не найдены'}
							onChange={(selectedOption) =>
								onNodeSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.node == null
									? null
									: {
											value: selectedObject.node.id,
											label:
												selectedObject.node.code === ''
													? '-'
													: selectedObject.node.code,
									  }
							}
							options={optionsObject.nodes.map((n) => {
								return {
									value: n.id,
									label: n.code === '' ? '-' : n.code,
								}
							})}
							styles={reactSelectstyle}
						/>
					</Form.Group>
					<Form.Group className="mrg-left no-bot-mrg">
                        <Form.Label
                            htmlFor="subnode"
                        >
                            Подузел
                        </Form.Label>
						<Select
                            inputId="subnode"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите подузел"
							noOptionsMessage={() => 'Подузлы не найдены'}
							className="input-width-3"
							onChange={(selectedOption) =>
								onSubnodeSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.subnode == null
									? null
									: {
											value: selectedObject.subnode.id,
											label:
												selectedObject.subnode.code ===
												''
													? '-'
													: selectedObject.subnode
															.code,
									  }
							}
							options={optionsObject.subnodes.map((s) => {
								return {
									value: s.id,
									label: s.code === '' ? '-' : s.code,
								}
							})}
							styles={reactSelectstyle}
						/>
					</Form.Group>
					<Form.Group className="mrg-left no-bot-mrg">
                        <Form.Label
                            htmlFor="mark"
                        >
                            Марка
                        </Form.Label>
						<Select
                            inputId="mark"
							maxMenuHeight={250}
							isClearable={true}
							isSearchable={true}
							placeholder="Выберите марку"
							noOptionsMessage={() => 'Марки не найдены'}
							className="input-width-3"
							onChange={(selectedOption) =>
								onMarkSelect((selectedOption as any)?.value)
							}
							value={
								selectedObject.mark == null
									? null
									: {
											value: selectedObject.mark.id,
											label:
												selectedObject.mark.code === ''
													? '-'
													: selectedObject.mark.code,
									  }
							}
							options={optionsObject.marks.map((m) => {
								return {
									value: m.id,
									label: m.code === '' ? '-' : m.code,
								}
							})}
							styles={reactSelectstyle}
						/>
					</Form.Group>
				</div>
                <ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
				<div className="flex btn-mrg-top-2">
					<Button
						variant="secondary"
						className="flex-grow"
						onClick={onSelectMarkButtonClick}
					>
						Выбрать
					</Button>
					<Button
						variant="secondary"
						className="flex-grow mrg-left"
						onClick={onCreateMarkButtonClick}
					>
						Создать
					</Button>
				</div>
			</div>
		</div>
	)
}

export default MarkSelect
