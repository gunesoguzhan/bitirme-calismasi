import { useState } from 'react'
import { VideoLobby } from '../components/video/VideoLobby'
import { VideoRoom } from '../components/video/VideoRoom'

export function VideoPage() {
    const [cam, setCam] = useState<boolean | null>()
    const [mic, setMic] = useState<boolean | null>()

    return (
        mic != undefined && cam != undefined ? <VideoRoom mic={mic} cam={cam} /> : <VideoLobby toggleMic={setMic} toggleCam={setCam} />
    )
}