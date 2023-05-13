import { MessageModel } from '../../types/MessageModel'

export function MessageItem(props: MessageProps) {
  return (
    <div className={props.message.sender === 'me' ? 'flex items-end justify-end' : 'flex items-start'}>
      <div className={props.message.sender === 'me' ? 'bg-slate-800 rounded-lg px-4 py-2 max-w-[75%] break-all' : 'bg-[#190d30] rounded-lg px-4 py-2 max-w-[75%] break-all'}>
        <p className="text-sm">{props.message.messageText}</p>
        <div className="flex">
          <div className="text-xs text-gray-400 basis-full">{props.message.sender !== 'me' ? props.message.sender : ''}</div>
          <div className="text-xs text-gray-400 basis-full text-right">{props.message.date}</div>
        </div>
      </div>
    </div>
  )
}

type MessageProps = {
  message: MessageModel
}