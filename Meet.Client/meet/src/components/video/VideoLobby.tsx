import { useEffect, useRef, useState } from 'react'
import { VideoButton } from './VideoButton'
import { useNavigate } from 'react-router-dom'

export function VideoLobby(props: VideoLobbyPropsTypes) {
    const localVideoRef = useRef<HTMLVideoElement>(null)
    const [localStream, setLocalStream] = useState<MediaStream>()
    const [cam, setCam] = useState(true)
    const [mic, setMic] = useState(true)
    const navigate = useNavigate()

    useEffect(() => {
        const startLocalVideo = async () => {
            try {
                const stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true })
                setLocalStream(stream)
                if (localVideoRef.current) {
                    localVideoRef.current.srcObject = stream
                }
            } catch (error) {
                console.error('Error accessing media devices.', error)
            }
        }
        startLocalVideo()
    }, [])

    useEffect(() => {
        localStream?.getVideoTracks().forEach(t => t.enabled = cam)
    }, [cam, localStream])

    useEffect(() => {
        localStream?.getAudioTracks().forEach(t => t.enabled = mic)
    }, [mic, localStream])

    return (
        <div className='h-screen w-screen bg-[#0a0c14] md:p-12 lg:px-36'>
            <div className='w-full h-full flex flex-col'>
                <div className='basis-full overflow-hidden rounded-lg'>
                    <video ref={localVideoRef} autoPlay muted className='w-full h-full object-cover object-center bg-[#030408]'></video>
                </div>
                <div className='w-full h-[96px] flex justify-center py-4'>
                    <VideoButton
                        backgroundImage={`/icons/${cam === true ? 'camera-light' : 'camera-closed-light'}.png`}
                        onClick={() => setCam(!cam)}
                    />
                    <VideoButton
                        backgroundImage={`/icons/${mic === true ? 'microphone-light' : 'microphone-closed-light'}.png`}
                        onClick={() => setMic(!mic)}
                    />
                    <VideoButton backgroundImage="/icons/check-light.png" onClick={() => {
                        localStream?.getTracks().forEach(t => t.stop())
                        props.toggleMic(mic)
                        props.toggleCam(cam)
                    }} />
                    <VideoButton backgroundImage="/icons/cross-light.png" onClick={() => {
                        localStream?.getTracks().forEach(t => t.stop())
                        navigate('/')
                    }} />
                </div>
            </div>
        </div>
    )
}

type VideoLobbyPropsTypes = {
    toggleMic: React.Dispatch<React.SetStateAction<boolean | null | undefined>>
    toggleCam: React.Dispatch<React.SetStateAction<boolean | null | undefined>>
}