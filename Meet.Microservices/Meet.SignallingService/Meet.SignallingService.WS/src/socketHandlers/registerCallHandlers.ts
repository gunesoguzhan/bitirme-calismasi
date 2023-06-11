import { CallModel } from '../models/callModel'
import { logger } from '../logger'
import { Server, Socket } from 'socket.io'
import { publishMessage } from '../amqp'
import * as redis from '../redisHandler'

export const registerCallHandlers = (io: Server, socket: Socket) => {
    const called = (call: CallModel) => {
        logger.info(`Call received. CallerId: ${call.caller.id} SocketId: ${socket.id} RoomId: ${call.room.id}}`)
        socket.to(call.room.id).emit('call:received', call)
        logger.info(`Call sended to room. CallerId: ${call.caller.id} SocketId: ${socket.id} RoomId: ${call.room.id}}`)
        const queueName = 'Meet-CallReceived'
        publishMessage(queueName, { date: call.date, callerId: call.caller.id, roomId: call.room.id })
        logger.info(`Message published to queue. CallerId: ${call.caller.id} SocketId: ${socket.id} RoomId: ${call.room.id}} QueueName: ${queueName}`)
    }

    const callCancelled = (callId: string) => {
        logger.info(`Call cancelled. SocketId: ${socket.id} RoomId: ${callId}}`)
        socket.to(callId).emit('call:cancelled')
    }

    const callRejected = async (call: CallModel) => {
        logger.info(`Call rejected. CallerId: ${call.caller.id} SocketId: ${socket.id} RoomId: ${call.room.id}}`)
        socket.to(await redis.get(call.caller.id)).emit('call:rejected')
    }

    const callAccepted = async (call: CallModel) => {
        logger.info(`Call accepted. CallerId: ${call.caller.id} SocketId: ${socket.id} RoomId: ${call.room.id}}`)
        socket.to(await redis.get(call.caller.id)).emit('call:accepted')
    }

    socket.on('call:called', called)
    socket.on('call:cancelled', callCancelled)
    socket.on('call:rejected', callRejected)
    socket.on('call:accepted', callAccepted)
}