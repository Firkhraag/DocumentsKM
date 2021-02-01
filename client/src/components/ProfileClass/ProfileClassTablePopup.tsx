// Global
import React, { useState, useEffect, createRef } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
// Util
import httpClient from '../../axios'
import ProfileClass from '../../model/ProfileClass'
import { useMark } from '../../store/MarkStore'

type ProfileClassTablePopupProps = {
    setProfileClass: (s: ProfileClass) => void
    close: () => void
}

const ProfileClassTablePopup = ({ setProfileClass, close }: ProfileClassTablePopupProps) => {
	const mark = useMark()
	const [profileClasses, setProfileClasses] = useState<ProfileClass[]>([])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const profileClassResponse = await httpClient.get(
						`/profile-classes`
					)
					setProfileClasses(profileClassResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onProfileClassClick = (s: ProfileClass) => {
        setProfileClass(s)
        close()
	}

	return (
		<div className="div-container component-cnt-div table-popup shadow" style={{border: 'none'}}>
            
			<Table bordered striped className="white-bg shadow no-bot-mrg">
                    <thead>
                        <tr>
                            <th>Шифр</th>
                            <th>Наименование</th>
                        </tr>
                    </thead>
                    <tbody>
                        {profileClasses.map((pc, index) => {
                            return (
                                <tr onClick={() => onProfileClassClick(pc)} key={index} className="pointer tr-hover">
                                    <td>{pc.id}</td>
                                    <td>{pc.name}</td>
                                </tr>
                            )
                        })}
                    </tbody>
                </Table>
		</div>
	)
}

export default ProfileClassTablePopup
