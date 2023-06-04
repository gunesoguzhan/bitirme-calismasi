import { useContext, useEffect, useState } from 'react'
import { MessageItem } from './MessageItem'
import { MessageModel } from '../../types/MessageModel'
import { MainLayoutContext } from '../../contexts/MainLayoutContext'
import { Link } from 'react-router-dom'
import axiosInstance from '../../axiosInstance'
import { SubmitHandler, useForm } from 'react-hook-form'
import { AuthContext } from '../../contexts/AuthContext'
import { UserModel } from '../../types/UserModel'
import { SocketContext } from '../../contexts/SocketContext'

export function MessagePanel(props: MessagePanelProps) {
    const [messages, setMessages] = useState<MessageModel[]>([])
    const { hideNavbar, showNavbar } = useContext(MainLayoutContext)
    const authContext = useContext(AuthContext)
    const socket = useContext(SocketContext)

    useEffect(() => {
        axiosInstance.get(`/api/messages?roomId=${props.conversationId}`)
            .then(response => setMessages(response.data))
    }, [props.conversationId])

    useEffect(() => {
        hideNavbar()
        return () => { showNavbar() }
    }, [hideNavbar, showNavbar])

    const { register, handleSubmit, reset } = useForm<MessageModel>()
    const onSubmit: SubmitHandler<MessageModel> = async (data) => {
        data.date = new Date()
        data.sender = authContext?.user as UserModel
        data.room = { id: props.conversationId as string, title: props.conversationTitle as string }
        setMessages([...messages, data])
        socket?.emit('message:send', data)
        reset()
    }

    const messageReceived = (message: MessageModel) => {
        console.log(message)
        setMessages([...messages, message])
    }

    socket?.on("message:received", messageReceived)

    return (
        <div className='flex flex-col basis-full h-full overflow-hidden'>
            <div className='m-[15px] text-2xl'>
                <Link to='/messages' className='outline-none md:hidden rounded-lg hover:bg-slate-800 focus:bg-slate-600 transition-all ease-out duration-150 p-2' >
                    <span
                        className='pb-[3px] px-[20px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                        style={{ backgroundImage: `url(/icons/back-light.png)` }}>
                    </span>
                    <span className='pr-4'>{props.conversationTitle}</span>
                </Link>
                <div className='hidden md:block ml-[20px]'>{props.conversationTitle}</div>
            </div>
            <ul className='basis-full overflow-y-auto'>
                {messages?.map(x => {
                    return (
                        <li key={x.id} className="px-4 py-2">
                            <MessageItem message={x} />
                        </li>
                    )
                })}
            </ul>
            <form className="flex p-4" onSubmit={handleSubmit(onSubmit)}>
                <input
                    type="text"
                    className="flex-1 rounded-lg mr-4 px-4 py-2 outline-none bg-slate-800 focus:bg-slate-600 transition-all ease-out duration-150"
                    placeholder="Type your message here..."
                    autoComplete='off'
                    {...register('messageText', {
                        required: true,
                        minLength: 1
                    })} />
                <button
                    type="submit"
                    className="bg-[#190d30] hover:bg-slate-800 rounded-lg outline-none active:bg-slate-600 transition-all ease-out duration-150" >
                    <div className='px-8 py-3 bg-no-repeat bg-center bg-contain' style={{ backgroundImage: "url('/icons/send-light.png')" }}></div>
                </button>
            </form>
        </div>
    )
}

type MessagePanelProps = {
    conversationId?: string
    conversationTitle?: string
}