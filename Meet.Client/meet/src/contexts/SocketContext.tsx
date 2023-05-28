import { createContext, useContext, useEffect, useState } from 'react'
import { Socket, io } from 'socket.io-client'
import { RoomModel } from '../types/RoomModel'
import { AuthContext } from './AuthContext'
import axiosInstance from '../axiosInstance'

const URL = 'http://localhost:3000'

export const SocketContext = createContext<null | Socket>(null)

export const SocketProvider = ({ children }: { children: React.ReactNode }) => {
    const [socket] = useState(io(URL, { autoConnect: false }))
    const { user } = useContext(AuthContext)

    useEffect(() => {
        const token = localStorage.getItem('token')
        socket.auth = {
            token: token
        }
        socket.connect()
        axiosInstance.get("/api/rooms").then(response => {
            if (response.status !== 200)
                return
            const rooms: RoomModel[] = response.data
            rooms.map(room => socket.emit('room:join', room.id, user))
        })

        return () => { socket.disconnect() }
    }, [socket])

    return (
        <SocketContext.Provider value={socket}>
            {children}
        </SocketContext.Provider>
    )
}