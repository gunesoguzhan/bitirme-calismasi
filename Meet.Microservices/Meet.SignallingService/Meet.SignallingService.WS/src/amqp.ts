import amqp from 'amqplib'
import { logger } from './logger'
import { config } from './configuration/config'


let connection: amqp.Connection
let channel: amqp.Channel

export const initializeRabbitMQ = async () => {
    try {
        const amqpUrl = `amqp://${config.rabbitMQ.username}:${config.rabbitMQ.password}@${config.rabbitMQ.host}:${config.rabbitMQ.port}`
        connection = await amqp.connect(amqpUrl)
        logger.debug(`AMQP: connected to ${amqpUrl}`)
        channel = await connection.createChannel()
        logger.debug('AMQP: channel created.')
        config.rabbitMQ.queues.map(async queue => {
            await channel.assertQueue(queue, { autoDelete: false, durable: true })
            logger.debug(`AMQP: queue asserted. Queue: ${queue}`)
        })
    } catch (error) {
        logger.error(`AMQP error: ${error}`)
    }
}

export const publishMessage = async <T>(queue: string, message: T) => {
    try {
        const result = channel.sendToQueue(queue, Buffer.from(JSON.stringify(message)))
        logger.debug(`AMQP: Message sended to ${queue}`)
        return result
    } catch (error) {
        logger.error(`AMQP error: ${error}`)
        return false
    }
}