import { useContext, useEffect } from 'react'
import VideoGrid from '../components/VideoGrid'
import { SocketContext } from '../contexts/SocketContext'

function VideoRoom() {
    const socket = useContext(SocketContext)

    useEffect(() => {
        socket?.emit('room:join', {roomId: 'room1'})
        socket?.on('user:joined', () => console.log('user joined.'))
        return () => {
            socket?.emit('room:leave', {roomId: 'room1'})
        }
    }, [socket])

    return (
        <div>
            <VideoGrid/>
        </div>
    )
}

export default VideoRoom