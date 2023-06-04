import { createContext, useContext, useEffect, useState } from 'react'
import { Socket, io } from 'socket.io-client'
import { RoomModel } from '../types/RoomModel'
import { AuthContext } from './AuthContext'
import axiosInstance from '../axiosInstance'

const URL = process.env.NODE_ENV === 'production'
    ? "http://meet-reverse-proxy:3000"
    : 'http://localhost:3000'

export const SocketContext = createContext<null | Socket>(null)

export const SocketProvider = ({ children }: { children: React.ReactNode }) => {
    const [socket] = useState(io(URL, { autoConnect: false }))
    const authContext = useContext(AuthContext)

    useEffect(() => {
        const token = localStorage.getItem('token')
        socket.auth = {
            token: token
        }
        socket.connect()
        var abortController = new AbortController()
        axiosInstance.get("/api/rooms", { signal: abortController.signal })
            .then(response => {
                if (response.status !== 200)
                    return
                const rooms: RoomModel[] = response.data
                rooms.map(room => socket.emit('room:join', room.id, authContext?.user))
            })

        return () => {
            abortController.abort()
            socket.disconnect()
        }
    }, [socket])

    return (
        <SocketContext.Provider value={socket}>
            {children}
        </SocketContext.Provider>
    )
}