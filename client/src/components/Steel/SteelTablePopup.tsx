// Global
import React, { useState, useEffect, createRef } from 'react'
// Bootstrap
import Table from 'react-bootstrap/Table'
// Util
import httpClient from '../../axios'
import Steel from '../../model/Steel'
import { useMark } from '../../store/MarkStore'

// Not used

type SteelTablePopupProps = {
    setSteel: (s: Steel) => void
    close: () => void
}

const SteelTablePopup = ({ setSteel, close }: SteelTablePopupProps) => {
	const mark = useMark()
	const [steelArr, setSteelArr] = useState<Steel[]>([])

	useEffect(() => {
		if (mark != null && mark.id != null) {
			const fetchData = async () => {
				try {
					const steelResponse = await httpClient.get(
						`/steel`
					)
					setSteelArr(steelResponse.data)
				} catch (e) {
					console.log('Failed to fetch the data')
				}
			}
			fetchData()
		}
	}, [mark])

	const onSteelClick = (s: Steel) => {
        setSteel(s)
        close()
	}

	return (
		<div className="div-container component-cnt-div table-popup shadow" style={{border: 'none'}}>
            
			<Table bordered striped className="white-bg shadow no-bot-mrg">
                    <thead>
                        <tr>
                            <th>Шифр</th>
                            <th>Наименование</th>
                            <th>ГОСТ</th>
                        </tr>
                    </thead>
                    <tbody>
                        {steelArr.map((s, index) => {
                            return (
                                <tr onClick={() => onSteelClick(s)} key={index} className="pointer tr-hover">
                                    <td>{s.id}</td>
                                    <td>{s.name}</td>
                                    <td>{s.standard}</td>
                                </tr>
                            )
                        })}
                    </tbody>
                </Table>
		</div>
	)
}

export default SteelTablePopup
