import { RoomModel } from './roomModel'
import { UserModel } from './userModel'

export interface MessageModel {
    messageText: string
    sender: UserModel
    date: Date
    room: RoomModel
}