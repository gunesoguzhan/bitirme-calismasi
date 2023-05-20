import { createContext, useMemo, useState } from 'react'
import { LoginUserModel } from '../types/LoginUserModel'
import axios from 'axios'
import jwtDecode from 'jwt-decode'
import { useNavigate } from 'react-router-dom'

export const AuthContext = createContext<AuthContextType>()

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const navigate = useNavigate()
    const [userId, setUserId] = useState<string>()

    const login = async (loginUser: LoginUserModel) => {
        const response = await axios.post("http://localhost:4000/api/auth/login", loginUser)
        if (response.status !== 200) return
        localStorage.setItem('token', response.data)
        setUser(response.data)
        // get all rooms
        // join rooms with socket
        navigate('/')
    }

    const logout = () => {
        localStorage.removeItem('token')
    }

    const setUser = (token: string) => {
        const payload = jwtDecode<TokenType>(token)
        if (payload.exp < Date.now() / 1000) {
            setUserId(undefined)
            return
        }
        setUserId(payload.userId)
    }

    useMemo(() => {
        const token = localStorage.getItem('token')
        if (!token) {
            setUserId(undefined)
            return
        }
        setUser(token)
    }, [])

    return (
        <AuthContext.Provider value={{ userId, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

type AuthContextType = {
    userId?: string
    login: (loginUser: LoginUserModel) => Promise<boolean>
    logout: () => void
}

type TokenType = {
    userId: string
    exp: number
}