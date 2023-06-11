import { logger } from '../logger'
import { Server, Socket } from 'socket.io'

export const registerVideoHandlers = (io: Server, socket: Socket) => {
    const userJoined = (roomId: string) => {
        logger.info(`User joined to meeting, RoomId: ${roomId} SocketId: ${socket.id}`)
        socket.to(roomId).emit('video:userJoined', socket.id)
    }

    const createAnswer = (remoteId: string, offer: RTCSessionDescription) => {
        logger.info(`Offer created, SocketId: ${socket.id} RemoteId: ${remoteId}`)
        socket.to(remoteId).emit('video:createAnswer', socket.id, offer)
    }

    const setAnswer = (remoteId: string, answer: RTCSessionDescription) => {
        logger.info(`Answer created, SocketId: ${socket.id} RemoteId: ${remoteId}`)
        socket.to(remoteId).emit('video:setAnswer', socket.id, answer)
    }

    const userLeft = () => {
        logger.info(`User left from meeting. SocketId: ${socket.id}`)
        socket.broadcast.emit('video:userLeft', socket.id)
    }

    socket.on('video:userJoined', userJoined)
    socket.on('video:userLeft', userLeft)
    socket.on('video:offerCreated', createAnswer)
    socket.on('video:answerCreated', setAnswer)
}