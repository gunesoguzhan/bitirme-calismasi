// import { Socket } from 'socket.io'
import * as jwt from 'jsonwebtoken'
import { Socket } from 'socket.io'

export const authMiddleware = (socket: Socket, next: (err?: Error) => void) => {
    const token = socket.handshake.auth?.token
    if (!token)
        next(new Error('Authentication error'))
    try {
        const payload = jwt.verify(token, 'This is the security key for development.', {audience: 'http://localhost', issuer: 'http://localhost'}) as jwt.JwtPayload
        socket.data.userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
        next()
    } catch (err) {
        next(new Error('Authentication error'))
    }
}   