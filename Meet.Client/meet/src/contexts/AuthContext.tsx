import { createContext, useMemo, useState } from 'react'
import { LoginUserModel } from '../types/LoginUserModel'
import axios from 'axios'
import jwtDecode from 'jwt-decode'

export const AuthContext = createContext<AuthContextType>()

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [userId, setUserId] = useState<string>()

    const signIn = async (loginUser: LoginUserModel) => {
        const response = await axios.post("http://localhost:5132/auth/login", loginUser)
        if (response.status !== 200) return false
        localStorage.setItem('token', response.data)
        setUser(response.data)
        return true
    }

    const signOut = () => {
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
        <AuthContext.Provider value={{ userId, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    )
}

type AuthContextType = {
    userId?: string
    signIn: (loginUser: LoginUserModel) => Promise<boolean>
    signOut: () => void
}

type TokenType = {
    userId: string
    exp: number
}