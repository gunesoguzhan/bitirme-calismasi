import { logger } from '../logger'
import { UserModel } from '../models/userModel'
import { publishMessage } from '../amqp'
import { Server, Socket } from 'socket.io'
import * as redis from '../redisHandler'

export const registerFriendshipHandlers = (io: Server, socket: Socket) => {
    const sendFriendshipRequest = async (sender: UserModel, receiver: UserModel) => {
        if (socket.data.userId !== sender.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${sender.id}`)
            return
        }
        socket.to(await redis.get(receiver.id)).emit('friendship:requestReceived', sender)
        publishMessage('Meet-FriendshipRequestSent', { senderId: sender.id, receiverId: receiver.id })
        logger.info(`Friendship request sended. SenderId: ${sender.id} ReceiverId: ${receiver.id}`)
    }

    const acceptFriendshipRequest = async (accepter: UserModel, sender: UserModel) => {
        if (socket.data.userId !== accepter.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${accepter.id}`)
            return
        }
        socket.to(await redis.get(sender.id)).emit('friendship:requestAccepted', accepter)
        publishMessage('Meet-FriendshipRequestAccepted', { accepterId: accepter.id, senderId: sender.id })
        logger.info(`Friendship request accepted. SenderId: ${sender.id} AccepterId: ${accepter.id}`)
    }

    const rejectFriendshipRequest = async (rejecter: UserModel, sender: UserModel) => {
        if (socket.data.userId !== rejecter.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${rejecter.id}`)
            return
        }
        socket.to(await redis.get(sender.id)).emit('friendship:requestAccepted', rejecter)
        publishMessage('Meet-FriendshipRequestRejected', { rejecterId: rejecter.id, senderId: sender.id })
        logger.info(`Friendship request rejected. SenderId: ${sender.id} RejecterId: ${rejecter.id}`)
    }

    const cancelFriendshipRequest = async (canceller: UserModel, friend: UserModel) => {
        if (socket.data.userId !== canceller.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${canceller.id}`)
            return
        }
        socket.to(await redis.get(friend.id)).emit('friendship:requestAccepted', canceller)
        publishMessage('Meet-FriendshipRequestCancelled', { cancellerId: canceller.id, friendId: friend.id })
        logger.info(`Friendship request cancelled. FriendId: ${friend.id} CancellerId: ${canceller.id}`)
    }

    const friendshipRemoved = async (remover: UserModel, friend: UserModel) => {
        if (socket.data.userId !== remover.id) {
            logger.error(`Socket user and emitter user are not equal. socket.data.userId: ${socket.data.userId} emitter.id: ${remover.id}`)
            return
        }
        socket.to(await redis.get(friend.id)).emit('friendship:requestAccepted', remover)
        publishMessage('Meet-FriendshipRemoved', { removerId: remover.id, friendId: friend.id })
        logger.info(`Friendship ended. FriendId: ${friend.id} RemoverId: ${remover.id}`)
    }

    socket.on('friendship:sendRequest', sendFriendshipRequest)
    socket.on('friendship:acceptRequest', acceptFriendshipRequest)
    socket.on('friendship:rejectRequest', rejectFriendshipRequest)
    socket.on('friendship:cancelRequest', cancelFriendshipRequest)
    socket.on('friendship:removed', friendshipRemoved)
}