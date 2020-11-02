import { StylesConfig } from 'react-select'

export const reactSelectstyle: StylesConfig = {
	control: (provided, state) => ({
		...provided,
		border: state.isFocused ? '1px solid rgba(0, 0, 0, 0.4)' : '1px solid #ced4da',
        boxShadow: 'none',
        cursor: 'pointer',
		'&:hover': {
			border: '1px solid rgba(0, 0, 0, 0.4)',
		},
    }),
    menu: (provided, state) => ({
        ...provided,
        border: '1px solid rgba(0, 0, 0, 0.4)',
    }),
	option: (provided, state) => ({
		...provided,
		background: state.isFocused ? '#ddd' : '#fff',
		color: '#000',
		cursor: 'pointer',
		'&:active': {
			background: '#ccc',
		},
	}),
}
