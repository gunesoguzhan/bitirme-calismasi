import { RoomModel } from './RoomModel'
import { UserModel } from './UserModel'

export interface MessageModel {
    id: string
    date: Date
    messageText: string,
    room: RoomModel,
    sender: UserModel
}