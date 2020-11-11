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

	const inputValues = useState({
		login: '',
		password: '',
	})[0]
	const [errMsg, setErrMsg] = useState('')

	const onLoginChange = (event: React.FormEvent<HTMLInputElement>) => {
		inputValues.login = event.currentTarget.value
	}
	const onPasswordChange = (event: React.FormEvent<HTMLInputElement>) => {
		inputValues.password = event.currentTarget.value
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
			<div className="shadow p-3 mb-5 bg-white rounded component-cnt-div">
				<Form>
					<Form.Group>
						<Form.Label>Логин</Form.Label>
						<Form.Control
							type="text"
							placeholder="Введите ваш логин"
							onBlur={onLoginChange}
						/>
					</Form.Group>
					<Form.Group>
						<Form.Label>Пароль</Form.Label>
						<Form.Control
							type="password"
							placeholder="Введите ваш пароль"
							onBlur={onPasswordChange}
						/>
					</Form.Group>

					<ErrorMsg errMsg={errMsg} hide={() => setErrMsg('')} />

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
