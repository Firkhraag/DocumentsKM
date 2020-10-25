// Global
import React from 'react'
// Bootstrap
import { X } from 'react-bootstrap-icons'
// Style
import './ErrorMsg.css'

type ErrorMsgProps = {
	errMsg: string
	hide: () => void
}

const ErrorMsg = ({ errMsg, hide }: ErrorMsgProps) => {
	return errMsg === '' ? null : (
		<div
			className="err-msg space-between-cent-v border-radius"
		>
			{errMsg}
            <X size={36} onClick={hide} className="pointer" />
		</div>
	)
}

export default ErrorMsg
