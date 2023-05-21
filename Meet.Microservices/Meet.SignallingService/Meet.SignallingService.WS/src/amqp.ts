import amqp from 'amqplib'
import { logger } from './logger'
import { config } from './config'

const connectToRabbitMQ = async () => {
    try {
        const amqpUrl = `amqp://${config.rabbitMQ.username}:${config.rabbitMQ.password}@${config.rabbitMQ.url}`
        const connection = await amqp.connect(amqpUrl)
        const channel = await connection.createChannel()
        return { channel, connection }
    } catch (error) {
        logger.error(error)
        return null
    }
}

export const publishMessage = async <T>(queueName: string, message: T) => {
    const { connection, channel } = await connectToRabbitMQ()
    await channel.assertQueue(queueName)
    logger.debug(`AMQP: Queue (${queueName}) asserted.`)
    const result = channel.sendToQueue(queueName, Buffer.from(JSON.stringify(message)))
    logger.debug(`AMQP: Message sended to ${queueName}`)
    connection.close()
    return result
}