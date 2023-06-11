import { useEffect, useState, useContext } from 'react'
import { UserModel } from '../../types/UserModel'
import axiosInstance from '../../axiosInstance'
import { FriendshipItem } from './FriendshipItem'
import { SocketContext } from '../../contexts/SocketContext'
import { AuthContext } from '../../contexts/AuthContext'
import { PeerRoomModel } from '../../types/PeerRoomModel'
import { useNavigate } from 'react-router-dom'

export function FriendshipPanel(props: FriendshipPanelProps) {
    const [activePanel, setActivePanel] = useState('friends')
    const [friends, setFriends] = useState<UserModel[]>()
    const [sentFriendshipRequests, setSentFriendshipRequests] = useState<UserModel[]>()
    const [receivedFriendshipRequests, setReceivedFriendshipRequests] = useState<UserModel[]>()
    const socket = useContext(SocketContext)
    const authContext = useContext(AuthContext)
    const navigate = useNavigate()

    const acceptFriendshipRequest = (user: UserModel) => {
        setReceivedFriendshipRequests(receivedFriendshipRequests?.filter(x => x.id != user.id))
        socket?.emit('friendship:acceptRequest', authContext?.user, user)
    }

    const rejectFriendshipRequest = (user: UserModel) => {
        setReceivedFriendshipRequests(receivedFriendshipRequests?.filter(x => x.id != user.id))
        socket?.emit('friendship:rejectRequest', authContext?.user, user)
    }

    const cancelFriendshipRequest = (user: UserModel) => {
        setSentFriendshipRequests(sentFriendshipRequests?.filter(x => x.id != user.id))
        socket?.emit('friendship:cancelRequest', authContext?.user, user)
    }

    // const removeFriendship = (user: UserModel) => {
    //     setFriends(friends?.filter(x => x.id != user.id))
    //     socket?.emit('friendship:remove', authContext?.user, user)
    // }

    const findOrCreateRoom = async (user: UserModel) => {
        const response = await axiosInstance
            .post('/api/rooms/findOrCreatePeerRoom', { creator: authContext?.user, friend: user })
        if (response.status != 200) return
        const peerRoom = response.data as PeerRoomModel
        if (peerRoom.created)
            socket?.emit('room:peerCreated', peerRoom.id, authContext?.user, user)
        return peerRoom
    }

    useEffect(() => {
        axiosInstance.get("/api/friendship/getFriends")
            .then(response => setFriends(response.data))
        axiosInstance.get("/api/friendship/getSentFriendshipRequests")
            .then(response => setSentFriendshipRequests(response.data))
        axiosInstance.get("/api/friendship/getReceivedFriendshipRequests")
            .then(response => setReceivedFriendshipRequests(response.data))
    }, [])
    return (
        <div className={'flex flex-col overflow-hidden h-full pb-[10px] md:basis-[500px] bg-[#0a0c14]' + (props.isActive ? '' : ' hidden md:flex')}>
            <div className='h-[60px] text-center p-3'>
                <button
                    type='button'
                    className={`bg-[#190d30] px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150 rounded-l-lg ${activePanel === 'friends' ? 'bg-slate-600 hover:bg-slate-600' : ''}`}
                    onClick={() => setActivePanel('friends')}>
                    <span className='py-4'>Friends</span>
                </button>
                <button
                    type='button'
                    className={`bg-[#190d30] px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150 ${activePanel === 'received' ? 'bg-slate-600 hover:bg-slate-600' : ''}`}
                    onClick={() => setActivePanel('received')}>
                    <span className='py-4'>Received</span>
                </button>
                <button
                    type='button'
                    className={`bg-[#190d30] px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150 rounded-r-lg ${activePanel === 'sent' ? 'bg-slate-600 hover:bg-slate-600' : ''}`}
                    onClick={() => setActivePanel('sent')}>
                    <span className='py-4'>Sent</span>
                </button>
                <button
                    type='button'
                    className='md:hidden bg-[#190d30] rounded-lg px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150 float-right'
                    onClick={props.showSearchPanel}>
                    <span
                        className='bg-contain bg-center bg-no-repeat p-2 mr-2'
                        style={{ backgroundImage: 'url(/icons/search-light.png)' }} />
                    <span className='py-4'>Search</span>
                </button>

            </div>
            <ul className='basis-full overflow-y-auto'>
                {activePanel === 'friends' &&
                    friends?.map(x => {
                        return (
                            <li key={x.id} className='mt-2'>
                                <FriendshipItem user={x} >
                                    <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2 mr-3'
                                        onClick={async () => {
                                            const room = await findOrCreateRoom(x)
                                            navigate(`/messages/${room?.id}`)
                                        }}>
                                        <span
                                            className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                            style={{ backgroundImage: `url(/icons/messages-light.png)` }}>
                                        </span>
                                    </button>
                                    <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2'
                                        onClick={async () => {
                                            const room = await findOrCreateRoom(x)
                                            console.log(authContext?.user)
                                            socket?.emit('call:called', { date: new Date(), caller: authContext?.user, room: room })
                                            navigate(`/call?meetingId=${room?.id}&title=${room?.title}`)
                                        }}>
                                        <span
                                            className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                            style={{ backgroundImage: `url(/icons/camera-light.png)` }}>
                                        </span>
                                    </button>
                                </FriendshipItem>
                            </li>
                        )
                    })
                }
                {activePanel === 'sent' &&
                    sentFriendshipRequests?.map(x => {
                        return (
                            <li key={x.id} className='mt-2'>
                                <FriendshipItem user={x} >
                                    <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2'
                                        onClick={() => cancelFriendshipRequest(x)} >
                                        <span
                                            className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                            style={{ backgroundImage: `url(/icons/cross-light.png)` }}>
                                        </span>
                                    </button>
                                </FriendshipItem>
                            </li>
                        )
                    })
                }
                {activePanel === 'received' &&
                    receivedFriendshipRequests?.map(x => {
                        return (
                            <li key={x.id} className='mt-2'>
                                <FriendshipItem user={x} >
                                    <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2 mr-3'
                                        onClick={() => acceptFriendshipRequest(x)} >
                                        <span
                                            className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                            style={{ backgroundImage: `url(/icons/check-light.png)` }}>
                                        </span>
                                    </button>
                                    <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2'
                                        onClick={() => rejectFriendshipRequest(x)} >
                                        <span
                                            className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                            style={{ backgroundImage: `url(/icons/cross-light.png)` }}>
                                        </span>
                                    </button>
                                </FriendshipItem>
                            </li>
                        )
                    })
                }
            </ul>
        </div >
    )
}

type FriendshipPanelProps = {
    isActive: boolean
    showSearchPanel: React.MouseEventHandler<HTMLButtonElement>
}
