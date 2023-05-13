import { MessageModel } from './MessageModel'

export interface ConversationModel {
    id: string
    type: string
    title: string
    lastMessage: MessageModel
}