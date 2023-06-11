import { useContext, useEffect, useState, useRef } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { SocketContext } from '../../contexts/SocketContext'
import { PeerConnection } from '../../types/PeerConnection'
import { VideoButton } from './VideoButton'
import { MessagePanel } from '../messages/MessagePanel'

//multiple user problem
//with room id

export const VideoRoom = (props: VideoRoomPropsTypes) => {
    const [isChatOpen, setIsChatOpen] = useState(false)
    const [peers, setPeers] = useState<PeerConnection[]>([])
    const [cam, setCam] = useState(props.cam)
    const [mic, setMic] = useState(props.mic)
    const navigate = useNavigate()
    const socket = useContext(SocketContext)
    const localVideoRef = useRef<HTMLVideoElement>(null)
    const [localStream, setLocalStream] = useState<MediaStream>()

    const setRemoteVideoStyle = () => {
        return {
            1: 'col-span-12 row-span-1 min-[560px]:col-span-6 min-[560px]:row-span-2',
        }[peers.length] || 'col-span-6 row-span-1'
    }

    const setLocalVideoStyle = () => {
        return {
            0: 'col-span-12 row-span-2',
            1: 'col-span-12 row-span-1 min-[560px]:col-span-6 min-[560px]:row-span-2',
            2: 'col-span-6 row-span-1 col-start-4',
        }[peers.length] || 'col-span-6 row-span-1'
    }

    useEffect(() => {
        const interval = setInterval(waitForLocalStream, 1000)
        function waitForLocalStream() {
            if (!localStream) return
            clearInterval(interval)
            socket?.emit('video:userJoined', '123')
        }
        return () => {
            socket?.emit('video:userLeft', '123')
        }
    }, [localStream, socket])

    useEffect(() => {
        const createPeerConnection = (id: string) => {
            const pc = new RTCPeerConnection()
            const peer: PeerConnection = { remoteId: id, peerConnection: pc }
            setPeers([...peers, peer])
            let flag = false

            pc.ontrack = (e) => {
                console.log(`on track!!!! ${id}`)
                if (flag) return
                flag = true
                const video = document.querySelector(`#peer-${peer.remoteId} > video`) as HTMLVideoElement
                video.srcObject = e.streams[0]
            }

            localStream?.getTracks().forEach((t) => {
                pc.addTrack(t, localStream)
            })
            return pc
        }

        const createOffer = async (remoteId: string) => {
            const pc = createPeerConnection(remoteId)
            console.log(peers)
            const dataChannel = pc.createDataChannel(`channel-${remoteId}`)
            dataChannel.onopen = () => console.log(`Connection opened. ${remoteId}`)
            let flag = false

            pc.onicecandidate = () => {
                if (flag) return
                socket?.emit('video:offerCreated', remoteId, pc.localDescription)
                flag = true
            }

            const offer = await pc.createOffer()
            await pc.setLocalDescription(offer)
            console.log(`Offer created. remoteId: ${remoteId}`)
            return offer
        }

        const createAnswer = async (remoteId: string, offer: RTCSessionDescription) => {
            const pc = createPeerConnection(remoteId)

            pc.ondatachannel = (e) => {
                const dataChannel = e.channel
                dataChannel.onopen = () => console.log(`Connection opened. ${remoteId}`)
            }

            let flag = false

            pc.onicecandidate = () => {
                if (flag) return
                socket?.emit('video:answerCreated', remoteId, pc.localDescription)
                flag = true
            }

            await pc.setRemoteDescription(offer)
            const answer = await pc.createAnswer()
            await pc.setLocalDescription(answer)
            console.log(`Answer created. remoteId ${remoteId}`)
            return answer
        }

        const setAnswer = async (remoteId: string, answer: RTCSessionDescriptionInit) => {
            const pc = peers.find((x) => x.remoteId === remoteId)?.peerConnection
            await pc?.setRemoteDescription(answer)
            console.log(`Answer set. remoteId: ${remoteId}`)
        }

        const userLeft = (remoteId: string) => {
            console.log(`User left the meeting. remoteId: ${remoteId}`)
            setPeers(peers.filter(x => x.remoteId != remoteId))
        }

        socket?.on('video:createAnswer', createAnswer)
        socket?.on('video:setAnswer', setAnswer)
        socket?.on('video:userJoined', createOffer)
        socket?.on('video:userLeft', userLeft)

        return () => {
            socket?.off('video:createAnswer', createAnswer)
            socket?.off('video:setAnswer', setAnswer)
            socket?.off('video:userJoined', createOffer)
            socket?.off('video:userLeft', userLeft)
        }
    }, [localStream, peers, socket])

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
    }, [navigate])

    useEffect(() => {
        localStream?.getAudioTracks().forEach(x => x.enabled = mic)
    }, [localStream, mic])

    useEffect(() => {
        localStream?.getVideoTracks().forEach(x => x.enabled = cam)
    }, [cam, localStream])

    return (
        <div className="w-screen h-screen flex">
            <div className={`flex flex-col ${!isChatOpen ? 'basis-full' : 'hidden'}`}>
                <div className={`basis-full grid grid-cols-12 grid-rows-2 overflow-hidden`}>
                    {peers.map((peer) => (
                        <div id={`peer-${peer.remoteId}`} key={`peer-${peer.remoteId}`} className={setRemoteVideoStyle()}>
                            <video ref={peer.videoRef} className="object-cover w-full h-full" autoPlay />
                        </div>
                    ))}
                    <div className={setLocalVideoStyle()}>
                        <video ref={localVideoRef} id="localVideo" className="object-cover w-full h-full" autoPlay muted />
                    </div>
                </div>
                <div className="min-h-[64px] flex p-2 bg-[#0a0c14] px-16">
                    <div className="basis-full flex">
                        <VideoButton
                            backgroundImage={`/icons/${cam === true ? 'camera-light' : 'camera-closed-light'}.png`}
                            onClick={() => setCam(!cam)}
                        />
                        <VideoButton
                            backgroundImage={`/icons/${mic === true ? 'microphone-light' : 'microphone-closed-light'}.png`}
                            onClick={() => setMic(!mic)}
                        />
                    </div>
                    <div className="basis-full flex justify-end">
                        <VideoButton
                            backgroundImage="/icons/messages-light.png"
                            onClick={() => setIsChatOpen(true)}
                            style="lg:hidden"
                        />
                        <VideoButton backgroundImage="/icons/leave-light.png" onClick={() => {
                            localStream?.getTracks().forEach(t => t.stop())
                            setPeers([])
                            navigate('/')
                        }} />
                    </div>
                </div>
            </div>
            <MessagePanel
                conversationId="123"
                conversationTitle="Public Chat"
                style={`lg:flex lg:min-w-[320px] lg:max-w-[320px] #0a0c14] ${isChatOpen === true ? 'basis-full' : 'hidden'}`}
                onClose={() => setIsChatOpen(false)}
            />
        </div>
    )
}


type VideoRoomPropsTypes = {
    mic: boolean
    cam: boolean
}