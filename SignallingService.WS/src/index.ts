import { Server, ServerOptions } from 'socket.io'
import * as config from './configuration.json'

const io = new Server(config.socketServer as Partial<ServerOptions>)

io.on('connect', socket => {
    console.log('user connected')
    socket.on('disconnect', () => console.log('user disconnected.'))
})

io.listen(config.server.port)