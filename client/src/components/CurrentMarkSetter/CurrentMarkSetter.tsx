// Global
import React, { useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { useHistory } from 'react-router-dom'
// Util
import httpClient from '../../axios'
import RecentMark from '../../model/RecentMark'
import Spinner from '../Spinner/Spinner'
import { useSetMark } from '../../store/MarkStore'

const CurrentMarkSetter = () => {
    const history = useHistory()
    const setMark = useSetMark()
    const { markId } = useParams<{markId: string}>()

	useEffect(() => {
        const fetchData = async () => {
            try {
                const markResponse = await httpClient.get(
                    `/marks/${parseInt(markId)}/with-parent-ids`
                )
                const mark = markResponse.data.mark
                if (mark != null) {
                    localStorage.setItem('selectedMarkId', markId)
    
                    let recentMarks = [] as RecentMark[]
                    const recentMarkStr = localStorage.getItem('recentMark')
                    if (recentMarkStr != null) {
                        recentMarks = JSON.parse(
                            recentMarkStr
                        ) as RecentMark[]
                    }
                    if (recentMarks.length >= 5) {
                        recentMarks.pop()
                    }
                    recentMarks.unshift(new RecentMark({
                        id: mark.id,
                        projectId: markResponse.data.projectId,
                        nodeId: markResponse.data.nodeId,
                        subnodeId: markResponse.data.subnodeId,
                    }))
                    let resStr = JSON.stringify(recentMarks)
                    localStorage.setItem('recentMark', resStr)
    
                    setMark(mark)
                }
            } catch (e) {
                console.log(e)
            } finally {
                history.push('/')
            }
        }
        fetchData()
	}, [])

	return (
		<Spinner />
	)
}

export default CurrentMarkSetter
