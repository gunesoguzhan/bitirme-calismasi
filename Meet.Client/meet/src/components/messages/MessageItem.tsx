import { AuthContext } from '../../contexts/AuthContext'
import { MessageModel } from '../../types/MessageModel'
import { useContext } from 'react'

export function MessageItem(props: MessageProps) {
  const { user } = useContext(AuthContext)

  return (
    <div className={props.message.sender.id === user?.id ? 'flex items-end justify-end' : 'flex items-start'}>
      <div className={props.message.sender.id === user?.id ? 'bg-slate-800 rounded-lg px-4 py-2 max-w-[75%] break-all' : 'bg-[#190d30] rounded-lg px-4 py-2 max-w-[75%] break-all'}>
        <p className="text-sm">{props.message.messageText}</p>
        <div className="flex">
          <div className="text-xs text-gray-400 basis-full">{props.message.sender.id !== user?.id ? `${props.message.sender.firstName} ${props.message.sender.lastName}` : ''}</div>
          <div className="text-xs text-gray-400 basis-full text-right">{new Date(props.message.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
        </div>
      </div>
    </div>
  )
}

type MessageProps = {
  message: MessageModel
}