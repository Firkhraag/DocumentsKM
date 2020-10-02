import axios from 'axios'

const httpClient = axios.create({
    // baseURL: process.env.APP_API_BASE_URL,
    // or use window.location.host
    baseURL: 'https://localhost:5001',
    timeout: 3000,
})

export const tokenKeyName = 'token'

let tokenValue = ''
export const setToken = (token: string) => tokenValue = token

httpClient.interceptors.request.use(config => {
	config.headers.Authorization = `Bearer ${tokenValue}`
	return config
})

httpClient.interceptors.response.use(
    response => response,
    async error => {
        // Timeout error
        if (error.response == null) {
            return Promise.reject(new Error('Ошибка сети'));
        }

        const originalRequest = error.config;
        if (error.response.status === 401 && originalRequest.url !== '/api/users/refresh-token') {
            await httpClient.post('/api/users/refresh-token');
            return httpClient(originalRequest);


            // const access_token = await refreshAccessToken();            
            // axios.defaults.headers.common['Authorization'] = 'Bearer ' + access_token;

            // return httpClient(originalRequest);
        }
        return Promise.reject(error);

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
);

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
