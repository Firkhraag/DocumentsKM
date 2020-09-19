import React, { useState, useEffect, useRef } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import axios from 'axios'
import { protocol, host } from '../../env'
import Dropdown from '../Dropdown/Dropdown'
import './MarkData.css'

const MarkData = () => {

    const specialistNameStringLength = 50

    const [height, setHeight] = useState(0)
    const heightRef = useRef()

    useEffect(() => {
        // Cannot use async func as callback in useEffect
        // Function for fetching data
        const fetchData = async () => {
            try {
                // // Fetch projects
                // const projectsFetchedResponse = await axios.get(protocol + '://' + host + '/api/projects')
                // const projectsFetched = projectsFetchedResponse.data

                // // Not very nice, but we are not using GraphQL here
                // const recentMarksFetchedResponse = await axios.get(protocol + '://' + host + '/api/recent_marks')
                // const recentMarksFetched = recentMarksFetchedResponse.data

                // const recentSubnodesIds: Array<number> = []
                // const recentSubnodesFetched: Array<Subnode> = []
                // for (let mark of recentMarksFetched) {
                //     if (!recentSubnodesIds.includes(mark.subnode.id)) {
                //         recentSubnodesIds.push(mark.subnode.id)
                //         recentSubnodesFetched.push(mark.subnode)
                //     }
                // }

                // // Set fetched objects as select options
                // setOptionsObject({
                //     ...defaultOptionsObject,
                //     recentMarks: recentMarksFetched,
                //     recentSubnodes: recentSubnodesFetched,
                //     projects: projectsFetched
                // })
            } catch (e) {
                console.log('Failed to fetch the data')
            }
        }
        fetchData()
        
        // Observe the heights
        const heightObserver = new ResizeObserver(([entry]) => {
			setHeight(entry.target.scrollHeight)
		})
		if (heightRef.current) {
			heightObserver.observe(heightRef.current)
        }
		return () => {
            heightObserver.disconnect()
        }
    }, [heightRef])

    const springStyle = useSpring({
        from: {
            opacity: 0 as any,
            height: 0,
            overflowY: 'hidden' as any
        },
        to: {
            opacity: false ? (0 as any) : 1,
            height: false ? 0 : height,
            overflowY: false ? ('hidden' as any) : ('visible' as any)
        },
    })

	return (
        <div className="component-cnt component-width">
            <h1 className="text-centered">Данные марки</h1>
            <div>
            <p className="text-centered data-section-label label-mrgn-top-1">Общая информация</p>
                <div className="flex-v mrg-bot">
                    <p className="label-area">Обозначение марки</p>
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
                <div className="flex-v mrg-bot">
                    <p className="label-area">Главный инженер проекта</p>
                    <div className="info-area">Тест</div>
                </div>
                

                <p className="text-centered data-section-label label-mrgn-top-2">Редактируемая информация</p>

                <div className="flex-v mrg-bot">
                    <p className="label-area">Шифр марки</p>
                    <div className="info-area">Тест</div>
                </div>
                <div className="flex-v mrg-bot">
                    <p className="label-area">Наименование марки</p>
                    <div className="info-area">Тест</div>
                </div>
                <Dropdown
					cntStyle="flex-v mrg-bot"
					label="Отдел"
					maxInputLength={specialistNameStringLength}
					onClickFunc={null}
					value={''}
					options={[{
                        id: 0,
                        val: 'Test',
                    }]}
				/>

                <animated.div style={springStyle}>
					<div ref={heightRef}>
                        <Dropdown
                            cntStyle="flex-v mrg-bot"
                            label="Заведующий группы"
                            maxInputLength={specialistNameStringLength}
                            onClickFunc={null}
                            value={''}
                            options={[{
                                id: 0,
                                val: 'Test',
                            }]}
                        />
                        <Dropdown
                            cntStyle="flex-v mrg-bot"
                            label="Главный специалист"
                            maxInputLength={specialistNameStringLength}
                            onClickFunc={null}
                            value={''}
                            options={[{
                                id: 0,
                                val: 'Test',
                            }]}
                        />
                        <Dropdown
                            cntStyle="flex-v mrg-bot"
                            label="Начальник отдела"
                            maxInputLength={specialistNameStringLength}
                            onClickFunc={null}
                            value={''}
                            options={[{
                                id: 0,
                                val: 'Test',
                            }]}
                        />
                        <Dropdown
                            cntStyle="flex-v"
                            label="Главный строитель"
                            maxInputLength={specialistNameStringLength}
                            onClickFunc={null}
                            value={''}
                            options={[{
                                id: 0,
                                val: 'Test',
                            }]}
                        />
					</div>
				</animated.div>

                <button className="final-btn input-border-radius pointer">
                    Сохранить изменения
                </button>
            </div>
        </div>
	)
}

export default MarkData
