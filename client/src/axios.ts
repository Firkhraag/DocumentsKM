import axios from 'axios'

const httpClient = axios.create({
	baseURL: 'https://localhost:5001',
    // baseURL: process.env.APP_API_BASE_URL,
    // or use window.location.host
})

export const tokenKeyName = 'token'

httpClient.interceptors.request.use(config => {
    const token = localStorage.getItem(tokenKeyName)
    console.log(token)
	config.headers.Authorization = token ? `Bearer ${token}` : ''
	return config
})

export default httpClient
