import { createContext, useState, useEffect } from 'react'
import { LoginUserModel } from '../types/LoginUserModel'
import jwtDecode from 'jwt-decode'
import { useNavigate } from 'react-router-dom'
import { UserModel } from '../types/UserModel'
import axiosInstance from '../axiosInstance'
import { Loading } from '../components/Loading/Loading'

export const AuthContext = createContext<AuthContextType>()

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const navigate = useNavigate()
    const [user, setUser] = useState<UserModel | undefined>()
    const [loading, setLoading] = useState(true)

    const login = async (loginUser: LoginUserModel) => {
        const response = await axiosInstance.post('/api/auth/login', loginUser)
        if (response.status !== 200) return
        localStorage.setItem('token', response.data)
        await initUser(response.data)
        navigate('/')
    }

    const logout = () => {
        setUser(undefined)
        localStorage.removeItem('token')
    }

    const initUser = async (token: string) => {
        const payload = jwtDecode<TokenType>(token)
        if (payload.exp < Date.now() / 1000) {
            setUser(undefined)
            setLoading(false)
            return
        }
        var response = await axiosInstance.get('/api/profiles/')
        if (response.status !== 200) return
        setUser(response.data)
        setLoading(false)
    }

    useEffect(() => {
        const token = localStorage.getItem('token')
        if (!token) {
            setUser(undefined)
            setLoading(false)
            return
        }
        initUser(token)
    }, [])

    if (loading) {
        return <div>Loading...</div> // Render a loading indicator while user data is being fetched
    }

    return (
        loading ? <Loading /> :
            <AuthContext.Provider value={{ user, login, logout }}>
                {children}
            </AuthContext.Provider>
    )
}

type AuthContextType = {
    user?: UserModel
    login: (loginUser: LoginUserModel) => Promise<void>
    logout: () => void
}

type TokenType = {
    userId: string
    exp: number
}
