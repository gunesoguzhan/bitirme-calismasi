import { Server, Socket } from 'socket.io'

export const registerRoomHandlers = (io: Server, socket: Socket) => {
    const joinRoom = ({ roomId }: { roomId: string }) => {
        socket.join(roomId)
        socket.to(roomId).emit('user:joined')
        console.log(`user joined to the ${roomId}.`)
    }

    const leaveRoom = ({ roomId }: { roomId: string }) => {
        socket.leave(roomId)
        socket.to(roomId).emit('user:leaved')
        console.log(`user leaved from the ${roomId}`)
    }

    socket.on('room:join', joinRoom)
    socket.on('room:leave', leaveRoom)
}