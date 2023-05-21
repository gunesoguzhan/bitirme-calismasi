import * as jwt from 'jsonwebtoken'
import { Socket } from 'socket.io'
import { logger } from '../logger'
import { config } from '../config'

export const authMiddleware = (socket: Socket, next: (err?: Error) => void) => {
    const token = socket.handshake.query?.token as string
    if (!token) {
        logger.error('Jwt token is null.')
        next(new Error('Authentication error'))
    }
    try {
        const payload = jwt.verify(token, config.jwt.securityKey, config.jwt) as jwt.JwtPayload
        socket.data.userId = (payload.userId as string).toUpperCase()
        next()
    } catch (err) {
        logger.info('Invalid token. User is not authorized.')
        next(new Error('Authentication error'))
    }
}   