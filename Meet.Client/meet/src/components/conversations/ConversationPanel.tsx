import { useEffect, useState } from 'react'
import { ConversationItem } from './ConversationItem'
import { ConversationModel } from '../../types/ConversationModel'
import axios from 'axios'

export function ConversationPanel(props: ConversationPanelProps) {
    const [conversations, setConversations] = useState<ConversationModel[]>()

    useEffect(() => {
        console.log(axios.defaults.headers.common)
        axios.defaults.headers.common.Authorization = `bearer ${localStorage.getItem('token')}`
        axios.get("/api/conversations").then(response => setConversations(response.data))
    }, [])

    return (
        <div className={'flex flex-col overflow-hidden h-full pb-[10px] md:basis-[500px] bg-[#0a0c14]' + (props.isActive ? '' : ' hidden md:flex')}>
            <div className='h-[60px] text-right p-3'>
                <button type='button' className='bg-[#190d30] rounded-lg px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150'>
                    <span className='bg-contain bg-center bg-no-repeat p-3 pt-[10px]' style={{ backgroundImage: 'url(/icons/plus-light.png)' }}></span>
                    <span className='py-4'>New</span>
                </button>
            </div>
            <ul className='basis-full overflow-y-auto'>
                {conversations?.map(x => {
                    return (
                        <li key={x.id} className='mt-2'>
                            <ConversationItem href={`/messages/${x.id}`} conversation={x} />
                        </li>
                    )
                })}
            </ul>
        </div>

    )
}

type ConversationPanelProps = {
    isActive: boolean
}
