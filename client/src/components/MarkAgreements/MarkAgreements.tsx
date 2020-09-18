import React from 'react'
import Dropdown from '../Dropdown/Dropdown'
import './MarkAgreements.css'

const MarkAgreements = () => {
	return (
        <div className="component-cnt component-width">
            <h1 className="text-centered">Согласования</h1>
            <table className="agreements-table">
				<tbody>
					<tr className="head-tr">
						<td>Отдел</td>
						<td>Специалист</td>
					</tr>
                    <tr>
						<td>
                            <Dropdown
                                cntStyle="flex-v"
                                label=""
                                maxInputLength={50}
                                onClickFunc={null}
                                value={''}
                                options={[{
                                    id: 0,
                                    val: 'test'
                                }]}
                            />
                        </td>
						<td>
                            <Dropdown
                                cntStyle="flex-v"
                                label=""
                                maxInputLength={50}
                                onClickFunc={null}
                                value={''}
                                options={[{
                                    id: 0,
                                    val: 'test'
                                }]}
                            />
                        </td>
					</tr>
                    <tr>
						<td>
                            <Dropdown
                                cntStyle="flex-v"
                                label=""
                                maxInputLength={50}
                                onClickFunc={null}
                                value={''}
                                options={[{
                                    id: 0,
                                    val: 'test'
                                }]}
                            />
                        </td>
						<td>
                            <Dropdown
                                cntStyle="flex-v"
                                label=""
                                maxInputLength={50}
                                onClickFunc={null}
                                value={''}
                                options={[{
                                    id: 0,
                                    val: 'test'
                                }]}
                            />
                        </td>
					</tr>
                </tbody>
            </table>
        </div>
	)
}

export default MarkAgreements
