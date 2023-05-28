import { publishMessage } from '../amqp'
import { MessageModel } from '../models/messageModel'
import { Server, Socket } from 'socket.io'

export const registerMessageHandlers = (io: Server, socket: Socket) => {
    const sendMessage = async (message: MessageModel) => {
        socket.to(message.room.id).emit('message:received', message)
        publishMessage('Meet-MessageReceived', { messageText: message.messageText, date: message.date, userId: message.sender.id, roomId: message.room.id })
    }

    socket.on('message:send', sendMessage)
}