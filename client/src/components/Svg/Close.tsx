import React from 'react'

type CloseProps = {
	onClick: () => void
}

const Close = ({ onClick }: CloseProps) => {
	return (
		<svg
			className="pointer"
			onClick={onClick}
			xmlns="http://www.w3.org/2000/svg"
			width="18.698"
			height="18.283"
			viewBox="0 0 18.698 18.283"
		>
			<path
				id="Union_4"
				data-name="Union 4"
				d="M10.349,11.8,3.868,18.284,1,15.416,7.481,8.935,1.415,2.868,4.282,0l6.067,6.067L16.416,0l2.868,2.868L13.217,8.935,19.7,15.416,16.83,18.284Z"
				transform="translate(-1)"
				fill="#90393c"
			/>
		</svg>
	)
}

export default Close
