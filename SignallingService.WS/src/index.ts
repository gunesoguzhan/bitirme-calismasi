import { Server, ServerOptions } from 'socket.io'
import * as config from './configuration.json'
import { authMiddleware } from './middlewares/authMiddleware'
import { registerRoomHandlers } from './socketHandlers/registerRoomHandlers'

const io = new Server(config.socketServer as Partial<ServerOptions>)

io.use(authMiddleware)

io.on('connect', socket => {
    console.log(`user connected ${socket.data.userId}`)
    registerRoomHandlers(io, socket)
    socket.on('disconnect', () => console.log('user disconnected.'))
})

io.listen(config.server.port)