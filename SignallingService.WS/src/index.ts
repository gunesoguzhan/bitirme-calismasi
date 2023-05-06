import { Server, ServerOptions } from 'socket.io'
import * as config from './configuration.json'
import { roomHandler } from './socketHandlers/roomHandler'

const io = new Server(config.socketServer as Partial<ServerOptions>)

io.on('connect', socket => {
    console.log('user connected')
    roomHandler(io, socket)
    socket.on('disconnect', () => console.log('user disconnected.'))
})

io.listen(config.server.port)