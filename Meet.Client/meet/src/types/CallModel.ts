import { RoomModel } from './RoomModel'
import { UserModel } from './UserModel'

export interface CallModel {
    id: string
    date: Date
    caller: UserModel
    room: RoomModel
}