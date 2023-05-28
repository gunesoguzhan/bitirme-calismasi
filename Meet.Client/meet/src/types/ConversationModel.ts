import { MessageModel } from './MessageModel'
import { RoomModel } from './RoomModel'

export interface ConversationModel {
    id: string
    room: RoomModel
    lastMessage?: MessageModel
}