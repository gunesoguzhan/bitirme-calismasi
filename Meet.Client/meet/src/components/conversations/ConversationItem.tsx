import { NavLink } from 'react-router-dom'
import { ConversationModel } from '../../types/ConversationModel'

export function ConversationItem(props: ConversationProps) {
    return (
        <NavLink
            type='button' to={props.href}
            className={({ isActive }) => {
                let style = "flex w-full text-left px-2 outline-none hover:border-r-4 transition-all ease-out duration-150"
                if (isActive) style += ' border-r-2'
                return style
            }}>
            <div className={"bg-slate-800 flex items-center justify-center h-12 w-12 rounded-lg text-white text-xl"}>
                {props.conversation.room.title.trim().length === 0 ? `${props.conversation.lastMessage?.sender.firstName.charAt(0)}${props.conversation.lastMessage?.sender.lastName.charAt(0)}`
                    : props.conversation.room.title.charAt(0).toLocaleUpperCase()}
            </div>
            <div className="flex-1 overflow-hidden flex flex-col pl-3">
                <div className="text-lg truncate">{props.conversation.room.title.trim().length === 0 ? `${props.conversation.lastMessage?.sender.firstName} ${props.conversation.lastMessage?.sender.lastName}`
                    : props.conversation.room.title.charAt(0).toLocaleUpperCase()}</div>
                <div className="text-sm text-gray-500 truncate">
                    {props.conversation.room.title.trim().length === 0 ? props.conversation.lastMessage?.messageText : `${props.conversation.lastMessage?.sender.firstName} ${props.conversation.lastMessage?.sender.lastName}: ${props.conversation.lastMessage?.messageText}`}
                </div>
            </div>
            <div className="flex flex-col justify-center text-sm text-right pr-2">
                <div className="text-gray-500">{new Date(props.conversation.lastMessage?.date as Date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
            </div>
        </NavLink >
    )
}

type ConversationProps = {
    conversation: ConversationModel
    href: string
}