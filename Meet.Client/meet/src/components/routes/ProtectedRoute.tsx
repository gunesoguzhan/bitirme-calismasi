import { useContext } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import { Navigate, Outlet } from 'react-router-dom'
import { SocketProvider } from '../../contexts/SocketContext'

export function ProtectedRoute() {
    const { user } = useContext(AuthContext)
    return user ? <SocketProvider><Outlet /></SocketProvider> : <Navigate to={'/login'} />
}