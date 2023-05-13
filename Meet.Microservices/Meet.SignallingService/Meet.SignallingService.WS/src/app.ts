import { Server, ServerOptions } from 'socket.io'
import { config } from '@config'
import { authMiddleware } from '@middlewares/authMiddleware'
import { registerRoomHandlers } from '@socketHandlers/registerRoomHandlers'
import { logger } from '@logger'

const io = new Server(config.socketServer as Partial<ServerOptions>)

io.use(authMiddleware)

io.on('connect', socket => {
    logger.info(`user connected ${socket.data.userId}`)
    registerRoomHandlers(io, socket)
    socket.on('disconnect', () => logger.info(`user disconnected ${socket.data.userId}`))
})

io.listen(config.server.port)