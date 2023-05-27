import { logger } from '../logger'
import { UserModel } from '../models/userModel'
import { publishMessage } from '../amqp'
import { Server, Socket } from 'socket.io'
import * as redis from '../redisHandler'

export const registerFriendshipHandlers = (io: Server, socket: Socket) => {
    const sendFriendshipRequest = async ({ sender, receiver }: { sender: UserModel, receiver: UserModel }) => {
        if (socket.data.userId !== sender.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${sender.id}`)
            socket.emit('friendship:error')
            return
        }
        socket.to(await redis.get(receiver.id)).emit('friendship:requestReceived', sender)
        publishMessage('friendshipRequestSent', { senderId: sender.id, receiverId: receiver.id })
        logger.info(`Friendship request sended. SenderId: ${sender.id} ReceiverId: ${receiver.id}`)
    }

    const acceptFriendshipRequest = async ({ accepter, sender }: { accepter: UserModel, sender: UserModel }) => {
        if (socket.data.userId !== accepter.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${accepter.id}`)
            socket.emit('friendship:error')
            return
        }
        socket.to(await redis.get(sender.id)).emit('friendship:requestAccepted', accepter)
        publishMessage('friendshipRequestAccepted', { accepterId: accepter.id, senderId: sender.id })
        logger.info(`Friendship request accepted. SenderId: ${sender.id} AccepterId: ${accepter.id}`)
    }

    socket.on('friendship:sendRequest', sendFriendshipRequest)
    socket.on('friendship:acceptRequest', acceptFriendshipRequest)
}