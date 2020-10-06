import axios from 'axios'

const httpClient = axios.create({
	// baseURL: process.env.APP_API_BASE_URL,
	// or use window.location.host
	baseURL: 'https://localhost:5001/api',
	withCredentials: true,
	timeout: 3000,
})

let tokenValue = ''

httpClient.interceptors.request.use((config) => {
	config.headers.Authorization = `Bearer ${tokenValue}`
	return config
})

httpClient.interceptors.response.use(
	(response) => {
		const originalRequest = response.config
		if (
			(originalRequest.url === '/users/refresh-token' ||
				originalRequest.url === '/users/login') &&
			response.status === 200
		) {
			tokenValue = response.data.accessToken
		} else if (
			originalRequest.url === '/users/logout' &&
			response.status === 204
		) {
			tokenValue = ''
		}
		return response
	},
	async (error) => {
		// Timeout error
		if (error.response == null || error.response.status === 500) {
			return Promise.reject(new Error('Ошибка сети'))
		}

		const originalRequest = error.config
		if (
			error.response.status === 401 &&
			originalRequest.url !== '/users/refresh-token'
		) {
			const response = await httpClient.post('/users/refresh-token')
			tokenValue = response.data.accessToken
			return httpClient(originalRequest)
		}
		return Promise.reject(error)
	}
)

export default httpClient
