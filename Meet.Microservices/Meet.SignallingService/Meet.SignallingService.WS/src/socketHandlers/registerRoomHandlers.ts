import { Server, Socket } from 'socket.io'
import { logger } from '../logger'
import { UserModel } from '../models/userModel'

export const registerRoomHandlers = (io: Server, socket: Socket) => {
    const joinRoom = ({ roomId, user }: { roomId: string, user: UserModel }) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            socket.to(user.id).emit('room:error')
            return
        }
        socket.join(roomId)
        socket.to(roomId).emit('room:joined', user)
        logger.info(`User joined to the room. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    const leaveRoom = ({ roomId, user }: { roomId: string, user: UserModel }) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            socket.to(user.id).emit('room:error')
            return
        }
        socket.leave(roomId)
        socket.to(roomId).emit('room:leaved', user)
        logger.info(`User leaved from the room. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    socket.on('room:join', joinRoom)
    socket.on('room:leave', leaveRoom)
}