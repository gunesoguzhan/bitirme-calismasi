import { useState, useContext, useEffect } from 'react'
import { VideoLobby } from '../components/video/VideoLobby'
import { VideoRoom } from '../components/video/VideoRoom'
import { useParams } from 'react-router-dom'
import { SocketContext } from '../contexts/SocketContext'

export function VideoPage() {
    const [cam, setCam] = useState<boolean | null>()
    const [mic, setMic] = useState<boolean | null>()
    const params = useParams<{ meetingId?: string }>()
    const socket = useContext(SocketContext)

    useEffect(() => {
        const event = socket?.listeners('call:received')[0]
        socket?.off('call:received')
        return (() => {
            event && socket?.on('call:received', event)
        })
    }, [socket])

    return (
        mic != undefined && cam != undefined
            ? <VideoRoom mic={mic} cam={cam} meetingId={params.meetingId} />
            : <VideoLobby toggleMic={setMic} toggleCam={setCam} />
    )
}