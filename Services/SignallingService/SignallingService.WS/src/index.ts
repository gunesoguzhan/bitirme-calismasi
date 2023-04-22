import { Server } from 'socket.io'

const port = 3000

const io = new Server()

io.on('connection', () => {
    console.log('user connected')
})

io.listen(port)
    