import { useContext, useEffect, useState } from 'react'
import { MessageItem } from './MessageItem'
import { MessageModel } from '../../types/MessageModel'
import { MainLayoutContext } from '../../contexts/MainLayoutContext'
import { Link } from 'react-router-dom'
import axiosInstance from '../../axiosInstance'

export function MessagePanel(props: MessagePanelProps) {
    const [messages, setMessages] = useState<MessageModel[]>()
    const { hideNavbar, showNavbar } = useContext(MainLayoutContext)

    useEffect(() => {
        axiosInstance.get("").then(response => setMessages(response.data))
    }, [])

    useEffect(() => {
        hideNavbar()
        return () => { showNavbar() }
    }, [hideNavbar, showNavbar])

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
            <form className="flex p-4">
                <input
                    type="text"
                    className="flex-1 rounded-lg mr-4 px-4 py-2 outline-none bg-slate-800 focus:bg-slate-600 transition-all ease-out duration-150"
                    placeholder="Type your message here..." />
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