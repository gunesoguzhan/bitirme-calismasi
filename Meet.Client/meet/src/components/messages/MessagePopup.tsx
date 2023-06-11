import { MessageModel } from '../../types/MessageModel'

export function MessagePopup(props: MessagePopupPropTypes) {
    return (
        <div className="fixed bottom-0 right-0 mb-16 mr-4 md:mb-4">
            {props.message && (
                <div className="bg-[#190d30] text-white py-2 px-4 shadow-lg rounded-lg">
                    <div className="text-lg">{props.message.room.title.trim().length === 0 ? `${props.message.sender.firstName} ${props.message.sender.lastName}` : props.message.room.title}</div>
                    <div className="text-sm">{props.message.room.title.trim().length === 0 ? `${props.message.messageText}` : `${props.message.sender.firstName} ${props.message.sender.lastName}: ${props.message.messageText}`}</div>
                </div>
            )}
        </div>

    )
}

type MessagePopupPropTypes = {
    message?: MessageModel
}