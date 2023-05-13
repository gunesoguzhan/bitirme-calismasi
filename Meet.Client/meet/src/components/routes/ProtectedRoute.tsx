import { useContext } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import { Navigate, Outlet } from 'react-router-dom'

export function ProtectedRoute() {
    const { userId } = useContext(AuthContext)

    return userId ? <Outlet /> : <Navigate to={'/login'} />
}