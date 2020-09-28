import axios from 'axios'

const httpClient = axios.create({
    // baseURL: process.env.APP_API_BASE_URL,
    // or use window.location.host
    baseURL: 'https://localhost:5001',
    timeout: 2000,
})

export const tokenKeyName = 'token'

let tokenValue = ''
export const setToken = (token: string) => tokenValue = token

httpClient.interceptors.request.use(config => {
	config.headers.Authorization = `Bearer ${tokenValue}`
	return config
})

export default httpClient
