import React, { useState, useRef, useEffect } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import ArrowSvg from './Svg/ArrowSvg'
import './Dropdown.css'

type DropdownProps = {
	label: string
	widthClassName: string
	maxInputLength: number
	onClickFunc: (event: React.MouseEvent<HTMLDivElement>) => void
	value: string
	options: string[]
}

const Dropdown = ({
	label,
	widthClassName,
	maxInputLength,
	onClickFunc,
	value,
	options,
}: DropdownProps) => {
	const [isInputFocused, setInputFocused] = useState(false)
	const [dropdownHeight, setDropdownHeight] = useState(0)

	const ref = useRef()

	useEffect(() => {
		const ro = new ResizeObserver(([entry]) => {
			setDropdownHeight(entry.target.scrollHeight)
		})

		if (ref.current) {
			ro.observe(ref.current)
		}

		return () => ro.disconnect()
	}, [ref])

	const onInputChange = (event: React.FormEvent<HTMLInputElement>) => {
		const newValue = event.currentTarget.value
		// setValue(newValue)
	}

	const onInputFocus = () => {
		setInputFocused(true)
	}

	const onInputBlur = () => {
		setInputFocused(false)
	}

	const springProp = useSpring({
		from: { opacity: 0 as any, height: 0, overflowY: 'hidden' as any },
		to: [
			{
				opacity: isInputFocused ? 1 : (0 as any),
				height: isInputFocused ? dropdownHeight : 0,
				config: { duration: 300 },
			},
			{ overflowY: 'auto' as any, config: { duration: 1 } },
		],
	})

	return (
		<div className="flex-v">
			<p className="label-area">{label}</p>
			<div className="dropdown-cnt relative">
				<input
					className={widthClassName + ' input-area'}
					type="text"
					value={value}
					onChange={onInputChange}
					onFocus={onInputFocus}
					onBlur={onInputBlur}
					placeholder="Не выбрано"
					spellCheck="false"
					maxLength={maxInputLength}
				/>
				<div className="arrow-area absolute">
					<ArrowSvg />
				</div>
				<div
					className={
						isInputFocused
							? `${widthClassName} dropdown-area-opened dropdown-area flex-v absolute pointer white-bg`
							: `${widthClassName} dropdown-area flex-v absolute pointer white-bg`
					}
				>
					<animated.div className="answer" style={springProp}>
						<div ref={ref}>
							{options.map((option, key) => (
								<div
									onClick={onClickFunc}
									key={key}
									className="option-area"
								>
									{option}
								</div>
							))}
						</div>
					</animated.div>
				</div>
			</div>
		</div>
	)
}

export default Dropdown
