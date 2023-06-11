import { useContext } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import { Navigate, Outlet } from 'react-router-dom'
import { SocketProvider } from '../../contexts/SocketContext'
import { SocketEvents } from '../socket/SocketEvents'

export function ProtectedRoute() {
    const authContext = useContext(AuthContext)
    return (
        authContext?.user
            ? <SocketProvider>
                <SocketEvents>
                    <Outlet />
                </SocketEvents>
            </SocketProvider>
            : <Navigate to={'/login'} />
    )
    // return <SocketProvider><Outlet /></SocketProvider>
}