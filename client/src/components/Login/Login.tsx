// Global
import React, { useState } from 'react'
// Bootstrap
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
// Components
import ErrorMsg from '../ErrorMsg/ErrorMsg'
// Util
import { useAuthMethods } from '../../store/UserStore'

const Login = () => {
	const authMethods = useAuthMethods()

	const [inputValues, setInputValues] = useState({
		login: '',
		password: '',
	})
	const [errMsg, setErrMsg] = useState('')

	const onLoginChange = (event: React.FormEvent<HTMLInputElement>) => {
		const login = event.currentTarget.value
		setInputValues({ ...inputValues, login: login })
	}
	const onPasswordChange = (event: React.FormEvent<HTMLInputElement>) => {
		const password = event.currentTarget.value
		setInputValues({ ...inputValues, password: password })
	}

	const onButtonClick = async () => {
		try {
			await authMethods.login(inputValues.login, inputValues.password)
		} catch (e) {
			if (e.message === 'Request failed with status code 400') {
				e.message = 'Неверный логин или пароль'
			}
			setErrMsg(e.message)
		}
	}

	return (
		<div className="component-cnt component-width">
			<h1 className="text-centered">Вход в систему</h1>
			<div className="shadow p-3 mb-5 bg-white rounded">
				<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />
				<Form>
					<Form.Group controlId="formBasicLogin">
						<Form.Label>Логин</Form.Label>
						<Form.Control
							type="text"
							placeholder="Введите ваш логин"
							onBlur={onLoginChange}
						/>
					</Form.Group>
					<Form.Group controlId="formBasicPassword">
						<Form.Label>Пароль</Form.Label>
						<Form.Control
							type="password"
							placeholder="Введите ваш пароль"
							onBlur={onPasswordChange}
						/>
					</Form.Group>
					<Button
						variant="secondary"
						className="btn-mrg-top full-width"
						onClick={onButtonClick}
					>
						Войти
					</Button>
				</Form>
			</div>
		</div>
	)
}

export default Login
