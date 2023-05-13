import { createContext, useEffect, useState } from 'react'
import { Socket, io } from 'socket.io-client'

const URL = 'http://localhost:3000'

export const SocketContext = createContext<null | Socket>(null)

export const SocketProvider = ({ children }: { children: React.ReactNode }) => {
    const [socket] = useState(io(URL, { autoConnect: false }))

    useEffect(() => {
        socket.connect()
        //getAllRooms
        //rooms.foreach(room:join)
        return () => { socket.disconnect() }
    }, [socket])

    return (
        <SocketContext.Provider value={socket}>
            {children}
        </SocketContext.Provider>
    )
}