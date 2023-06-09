import axios from 'axios'

const axiosInstance = axios.create()



axiosInstance.defaults.baseURL = process.env.NODE_ENV === 'production'
    ? "http://meet-api-gateway:80"
    : "http://localhost:4000"

axiosInstance.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token')
        if (token) {
            config.headers.Authorization = `Bearer ${token}`
        }
        return config
    },
    (error) => {
        return Promise.reject(error)
    }
)

export default axiosInstance
