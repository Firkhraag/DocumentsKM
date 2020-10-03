import axios from 'axios'

const httpClient = axios.create({
	// baseURL: process.env.APP_API_BASE_URL,
	// or use window.location.host
	baseURL: 'https://localhost:5001/api',
	withCredentials: true,
	timeout: 3000,
})

let tokenValue = ''
// export const setToken = (token: string) => tokenValue = token
// 1

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

			// const access_token = await refreshAccessToken();
			// axios.defaults.headers.common['Authorization'] = 'Bearer ' + access_token;

			// return httpClient(originalRequest);
		}
		return Promise.reject(error)

		// // Timeout - error.code = ECONNABORTED
		// // Если не 401 отклонить Promise
		// if (error.response == null) {
		//     return Promise.reject(new Error('Ошибка сети'));
		// }
		// if (error.response.status !== 401) {
		//     return Promise.reject(error);
		// }
		// console.log('Test')

		// // Предотвращаем бесконечную рекурсию, убирая данный interceptor
		// httpClient.interceptors.response.eject(interceptor)
		// // Пытаемся обновить access token
		// return axios.post('/api/users/refresh-token').then(response => {
		//     tokenValue = response.data.token
		//     return httpClient(error.response.config)
		// }).catch(error => {
		//     tokenValue = ''
		//     // this.router.push('/login')
		//     return Promise.reject(error)
		// }).finally(createAxiosResponseInterceptor) // Возвращаем interceptor обратно
	}
)

// const createAxiosResponseInterceptor = () => {
//     const interceptor = httpClient.interceptors.response.use(
//         response => response,
//         error => {

//             const originalRequest = error.config;
//             if (error.response.status === 401 && !originalRequest._retry) {
//                 originalRequest._retry = true;
//                 const access_token = await refreshAccessToken();
//                 axios.defaults.headers.common['Authorization'] = 'Bearer ' + access_token;
//                 return axiosApiInstance(originalRequest);
//             }
//             return Promise.reject(error);

//             // // Timeout - error.code = ECONNABORTED
//             // // Если не 401 отклонить Promise
//             // if (error.response == null) {
//             //     return Promise.reject(new Error('Ошибка сети'));
//             // }
//             // if (error.response.status !== 401) {
//             //     return Promise.reject(error);
//             // }
//             // console.log('Test')

//             // // Предотвращаем бесконечную рекурсию, убирая данный interceptor
//             // httpClient.interceptors.response.eject(interceptor)
//             // // Пытаемся обновить access token
//             // return axios.post('/api/users/refresh-token').then(response => {
//             //     tokenValue = response.data.token
//             //     return httpClient(error.response.config)
//             // }).catch(error => {
//             //     tokenValue = ''
//             //     // this.router.push('/login')
//             //     return Promise.reject(error)
//             // }).finally(createAxiosResponseInterceptor) // Возвращаем interceptor обратно
//         }
//     );
// }

// createAxiosResponseInterceptor()

export default httpClient
