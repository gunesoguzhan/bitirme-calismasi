import { Server, Socket } from 'socket.io'
import { logger } from '@logger'

export const registerRoomHandlers = (io: Server, socket: Socket) => {
    const joinRoom = ({ roomId }: { roomId: string }) => {
        socket.join(roomId)
        socket.to(roomId).emit('user:joined')
        logger.info(`User joined to the room. userId: ${socket.data.userId} roomId: ${roomId}`)
    }

    const leaveRoom = ({ roomId }: { roomId: string }) => {
        socket.leave(roomId)
        socket.to(roomId).emit('user:leaved')
        logger.info(`User leaved from the room. userId: ${socket.data.userId} roomId: ${roomId}`)
    }

    socket.on('room:join', joinRoom)
    socket.on('room:leave', leaveRoom)
}