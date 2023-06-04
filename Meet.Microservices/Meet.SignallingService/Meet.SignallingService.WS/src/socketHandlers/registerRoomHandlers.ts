import { Server, Socket } from 'socket.io'
import { logger } from '../logger'
import { UserModel } from '../models/userModel'
import * as redis from '../redisHandler'

export const registerRoomHandlers = (io: Server, socket: Socket) => {
    const joinRoom = (roomId: string, user: UserModel) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            return
        }
        socket.join(roomId)
        socket.to(roomId).emit('room:joined', user)
        logger.info(`User joined to the room. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    const leaveRoom = (roomId: string, user: UserModel) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            return
        }
        socket.leave(roomId)
        socket.to(roomId).emit('room:leaved', user)
        logger.info(`User leaved from the room. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    const createGroupRoom = (roomId: string, user: UserModel, users: UserModel[]) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            return
        }
        socket.join(roomId)
        users.map(async u => socket.to(await redis.get(u.id)).emit('room:created', roomId))
        logger.info(`Group room created. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    const createPeerRoom = async (roomId: string, user: UserModel, peer: UserModel) => {
        if (socket.data.userId !== user.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${user.id}`)
            return
        }
        socket.join(roomId)
        socket.to(await redis.get(peer.id)).emit('room:created', roomId)
        logger.info(`Peer room created. UserId: ${socket.data.userId} RoomId: ${roomId}`)
    }

    socket.on('room:join', joinRoom)
    socket.on('room:leave', leaveRoom)
    socket.on('room:groupCreated', createGroupRoom)
    socket.on('room:peerCreated', createPeerRoom)
}