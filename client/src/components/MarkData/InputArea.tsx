import React from 'react'

type InputAreaProps = {
	label: string
	widthClassName: string
	onChangeFunc: (event: React.FormEvent<HTMLSelectElement>) => void
	value: string
	options: string[]
}

const InputArea = ({
	label,
	widthClassName,
	onChangeFunc,
	value,
	options,
}: InputAreaProps) => {
	return (
		<div className="flex-v">
			<p className="mrg-bot-1">{label}</p>
			<select
				onChange={onChangeFunc}
				value={value}
				className={
					widthClassName + ' border input-border-radius input-padding'
				}
			>
				<option key={-1}></option>
				{options.map((x, y) => (
					<option key={y}>{x}</option>
				))}
			</select>
		</div>
	)
}

export default InputArea
