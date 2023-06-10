import { AuthContext } from '../../contexts/AuthContext'
import { MessageModel } from '../../types/MessageModel'
import { useContext } from 'react'

export function MessageItem(props: MessageProps) {
  const authContext = useContext(AuthContext)

  return (
    <div className={props.message.sender.id === authContext?.user?.id ? 'flex items-end justify-end' : 'flex items-start'}>
      <div className={`rounded-lg px-4 py-2 max-w-[75%] break-all min-w-[128px] ${props.message.sender.id === authContext?.user?.id ? 'bg-slate-800 ' : 'bg-[#190d30]'}`}>
        <div className="text-xs text-gray-400">{props.message.sender.id !== authContext?.user?.id ? `${props.message.sender.firstName} ${props.message.sender.lastName}` : ''}</div>
        <p className="text-sm mt-1">{props.message.messageText}</p>
        <div className="text-xs text-gray-400 min-w-[60px] text-right">{new Date(props.message.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
      </div>
    </div>
  )
}

type MessageProps = {
  message: MessageModel
}