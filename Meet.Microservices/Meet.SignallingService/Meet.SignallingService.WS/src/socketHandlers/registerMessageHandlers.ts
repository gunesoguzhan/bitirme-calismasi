import { publishMessage } from '../amqp'
import { MessageModel } from '../models/messageModel'
import { Server, Socket } from 'socket.io'

export const registerMessageHandlers = (io: Server, socket: Socket) => {
    const sendMessage = async (message: MessageModel) => {
        socket.to(message.room.id).emit('message:received', message)
        publishMessage('Meet-MessageReceived', message)
    }

    socket.on('message:send', sendMessage)
}