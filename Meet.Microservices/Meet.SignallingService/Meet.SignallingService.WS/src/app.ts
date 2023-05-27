import { Server, ServerOptions } from 'socket.io'
import { config } from './config'
import { authMiddleware } from './middlewares/authMiddleware'
import { registerRoomHandlers } from './socketHandlers/registerRoomHandlers'
import { logger } from './logger'
import { registerFriendshipHandlers } from './socketHandlers/registerFriendshipHandlers'
import * as redis from './redisHandler'
import { registerMessageHandlers } from './socketHandlers/registerMessageHandlers'
import { initializeRabbitMQ } from './amqp'

const io = new Server(config.socketServer as Partial<ServerOptions>)

initializeRabbitMQ()
redis.initializeRedis()

io.use(authMiddleware)

io.on('connect', async (socket) => {
    await redis.set(socket.data.userId, socket.id)
    logger.info(`User connected. UserId: ${socket.data.userId} SocketId: ${socket.id}`)
    registerFriendshipHandlers(io, socket)
    registerRoomHandlers(io, socket)
    registerMessageHandlers(io, socket)
    socket.on('disconnect', async () => {
        await redis.del(socket.data.userId)
        logger.info(`User disconnected. UserId: ${socket.data.userId} SocketId: ${socket.id}`)
    })
})

io.listen(config.server.port)

logger.info(`Application running on port: ${config.server.port}`)