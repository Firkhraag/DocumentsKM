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
    // changeValue: (newValue: string) => void
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

	const inputRef = useRef()
	const dropdownRef = useRef()

	useEffect(() => {
		const ro = new ResizeObserver(([entry]) => {
			setDropdownHeight(entry.target.scrollHeight)
		})

		if (dropdownRef.current) {
			ro.observe(dropdownRef.current)
		}

		return () => ro.disconnect()
	}, [dropdownRef])

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
		<div className="flex-v mrg-bottom">
			<p className="label-area">{label}</p>
			<div className="dropdown-cnt relative">
				<input
					ref={inputRef}
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
				<div
					onClick={() => inputRef}
					className="arrow-area absolute pointer"
				>
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
						<div ref={dropdownRef}>
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
