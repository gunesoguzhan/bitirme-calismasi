import { RoomModel } from './roomModel'
import { UserModel } from './userModel'

export interface CallModel {
    date: Date
    caller: UserModel
    room: RoomModel
}