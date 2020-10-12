import React from 'react'
import { useSpring, animated } from 'react-spring'
import Close from '../Svg/Close'
import './ErrorMsg.css'

type ErrorMsgProps = {
	errMsg: string
	hide: () => void
}

const ErrorMsg = ({ errMsg, hide }: ErrorMsgProps) => {
	const propsSpringOpen = useSpring({
		from: { opacity: 0 as any, transform: 'scale(0)' as any },
		to: {
			opacity: errMsg === '' ? 0 : 1 as any,
            transform: errMsg === '' ? 'scale(0)' : 'scale(1)' as any,
            
		},
		config: {
			duration: 200,
		},
	})

	return (
        errMsg === '' ? null :
		<animated.div
			className="err-msg space-between-cent-v"
			style={propsSpringOpen}
		>
			{errMsg}
			<Close onClick={hide} />
		</animated.div>
	)
}

export default ErrorMsg
