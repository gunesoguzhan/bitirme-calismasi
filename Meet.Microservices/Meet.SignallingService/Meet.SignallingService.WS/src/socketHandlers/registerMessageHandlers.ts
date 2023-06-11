import { logger } from '../logger'
import { publishMessage } from '../amqp'
import { MessageModel } from '../models/messageModel'
import { Server, Socket } from 'socket.io'

export const registerMessageHandlers = (io: Server, socket: Socket) => {
    const sendMessage = async (message: MessageModel) => {
        logger.info(`Message received. SenderId: ${message.sender.id} SocketId: ${socket.id} RoomId: ${message.room.id}}`)
        socket.to(message.room.id).emit('message:received', message)
        logger.info(`Message sended to room. SenderId: ${message.sender.id} SocketId: ${socket.id} RoomId: ${message.room.id}}`)
        const queueName = 'Meet-MessageReceived'
        publishMessage(queueName, { messageText: message.messageText, date: message.date, userId: message.sender.id, roomId: message.room.id })
        logger.info(`Message published to queue. SenderId: ${message.sender.id} SocketId: ${socket.id} RoomId: ${message.room.id}} QueueName: ${queueName}`)
    }

    socket.on('message:send', sendMessage)
}