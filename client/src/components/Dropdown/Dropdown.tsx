import React, { useState, useRef, useEffect } from 'react'
import { useSpring, animated } from 'react-spring'
import ResizeObserver from 'resize-observer-polyfill'
import ArrowSvg from './Svg/ArrowSvg'
import './Dropdown.css'

type DropdownProps = {
	cntStyle: string
	label: string
	placeholder: string
	maxInputLength: number
	onClickFunc: (id: number) => void
	value: string
	// TBD
	// changeValue: (newValue: string) => void
	options: Array<any>
}

const Dropdown = ({
	cntStyle,
	label,
	placeholder,
	maxInputLength,
	onClickFunc,
	value,
	options,
}: DropdownProps) => {
	const [isInputFocused, setInputFocused] = useState(false)
	const [dropdownHeight, setDropdownHeight] = useState(0)

	// Input ref for focusing input field
	const inputRef = useRef()
	const dropdownRef = useRef()

	useEffect(() => {
		// Track the height of dropdown options
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

	const onInputBlur = (e: React.FocusEvent<HTMLInputElement>) => {
		setInputFocused(false)
	}

	const onArrowClick = () => {
		const inputElement = inputRef.current as any
		if (inputElement) {
			inputElement.focus()
		}
	}

	const springProp = useSpring({
		from: {
			opacity: 0 as any,
			height: 0,
			overflowY: 'hidden' as any,
		},
		to: [
			{
				opacity: isInputFocused ? 1 : (0 as any),
				height: isInputFocused ? dropdownHeight : 0,
				config: { duration: 200 },
			},
			{
				overflowY: 'auto' as any,
				config: { duration: 1 },
			},
		],
	})

	return (
		<div className={cntStyle}>
			<p className="label-area">{label}</p>
			<div className="relative">
				<input
					ref={inputRef}
					className="input-area"
					type="text"
					value={value}
					onChange={onInputChange}
					onFocus={onInputFocus}
					onBlur={onInputBlur}
					placeholder={placeholder}
					spellCheck="false"
					maxLength={maxInputLength}
				/>
				<div
					onClick={onArrowClick}
					className="arrow-area absolute pointer"
				>
					<ArrowSvg />
				</div>
				<animated.div
					style={springProp}
					className={
						isInputFocused
							? 'dropdown-area-opened dropdown-area flex-v absolute pointer white-bg'
							: 'dropdown-area flex-v absolute pointer white-bg'
					}
				>
					<div ref={dropdownRef} className="options-cnt">
						{options.map((option) => (
							<div
								onMouseDown={() => onClickFunc(option.id)}
								key={option.id}
								className="option-area"
							>
								{option.val}
							</div>
						))}
					</div>
				</animated.div>
			</div>
		</div>
	)
}

export default Dropdown
