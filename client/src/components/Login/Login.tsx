import React, { useState } from 'react'

const Login = () => {
	const [inputValues, setInputValues] = useState({
		login: '',
		password: '',
	})

	const onLoginChange = (event: React.FormEvent<HTMLInputElement>) => {
		const login = event.currentTarget.value
		setInputValues({ ...inputValues, login: login })
	}
	const onPasswordChange = (event: React.FormEvent<HTMLInputElement>) => {
		const password = event.currentTarget.value
		setInputValues({ ...inputValues, password: password })
	}

	return (
		<div className="component-cnt component-width">
            <h1 className="text-centered">Вход</h1>
            <div>
                <div className="flex-v mrg-bot">
                    <label htmlFor="login" className="label-area">Логин</label>
                    <input id="login" className="input-area" onBlur={onLoginChange} type="text" placeholder="Введите ваш логин" spellCheck="false" required />
                </div>

                <div className="flex-v">
                    <label htmlFor="password" className="label-area">Пароль</label>
                    <input id="password" className="input-area" type="password" onBlur={onPasswordChange} placeholder="Введите ваш пароль" spellCheck="false" required />
                </div>

                <button className="final-btn input-border-radius pointer">
                    Войти
                </button>
            </div>
        </div>
	)
}

export default Login
