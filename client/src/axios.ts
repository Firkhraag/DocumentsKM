import axios from 'axios'

const httpClient = axios.create({
	baseURL: `${window.location.protocol}//${window.location.host}/api`,
	withCredentials: true,
})

httpClient.interceptors.request.use((config) => {
    const tokenValue = localStorage.getItem('token')
	config.headers.Authorization = `Bearer ${tokenValue}`
	return config
})

httpClient.interceptors.response.use(
	(response) => {
		const originalRequest = response.config
		if (originalRequest.url === '/users/login' && response.status === 200) {
            localStorage.setItem('token', response.data.token)
		}
		return response
	},
	async (error) => {
		if (error.response == null || error.response.status === 500) {
			return Promise.reject(new Error('Ошибка сети'))
		}
        if (error.response.status === 401) {
			localStorage.removeItem('token')
		}
		return Promise.reject(error)
	}
)

export default httpClient
