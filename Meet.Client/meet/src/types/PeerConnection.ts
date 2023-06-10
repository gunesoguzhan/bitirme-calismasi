export interface PeerConnection {
    remoteId: string
    peerConnection: RTCPeerConnection,
    videoRef?: React.RefObject<HTMLVideoElement>
}