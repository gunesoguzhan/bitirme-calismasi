import * as jwt from 'jsonwebtoken'
import { Socket } from 'socket.io'
import { logger } from '../logger'

export const authMiddleware = (socket: Socket, next: (err?: Error) => void) => {
    const token = socket.handshake.auth?.token
    if (!token) {
        logger.error('Token is null.')
        next(new Error('Authentication error'))
    }
    try {
        const payload = jwt.verify(token, 'This is the security key for development.', { audience: 'http://localhost', issuer: 'http://localhost' }) as jwt.JwtPayload
        socket.data.userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
        next()
    } catch (err) {
        logger.info('Invalid token. User is not authorized.')
        next(new Error('Authentication error'))
    }
}   