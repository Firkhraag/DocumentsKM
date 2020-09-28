import React, { useState } from 'react'
import { useAuthMethods } from '../../store/AuthStore'
import Close from '../Svg/Close'
import './Login.css'

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
            if (e.message === 'Network Error') {
                e.message = 'Ошибка сети'
            } else if (e.message === 'Request failed with status code 401') {
                e.message = 'Неверный логин или пароль'
            }
            setErrMsg(e.message)
        }
	}

	return (
		<div className="component-cnt component-width">
			<h1 className="text-centered">Вход в систему</h1>
			<div>
				<div className="flex-v mrg-bot">
					{errMsg === '' ? null : (
						<div className="login-err-msg space-between-cent-v">
							{errMsg}
							<Close onClick={() => setErrMsg('')} />
						</div>
					) }
					<label htmlFor="login" className="login-label-area">
						Логин
					</label>
					<input
						id="login"
						className="input-area"
						onBlur={onLoginChange}
						type="text"
						placeholder="Введите ваш логин"
                        spellCheck="false"
                        maxLength={255}
						required
					/>
				</div>

				<div className="flex-v">
					<label htmlFor="password" className="login-label-area">
						Пароль
					</label>
					<input
						id="password"
						className="input-area"
						type="password"
						onBlur={onPasswordChange}
						placeholder="Введите ваш пароль"
                        spellCheck="false"
                        maxLength={255}
                        required
					/>
				</div>

				<button
					className="final-btn input-border-radius pointer"
					onClick={onButtonClick}
				>
					Войти
				</button>
			</div>
		</div>
	)
}

export default Login
