import { RoomModel } from './roomModel'
import { UserModel } from './userModel'

export interface MessageModel {
    messageText: string
    sender: UserModel
    dateTime: Date
    room: RoomModel
}