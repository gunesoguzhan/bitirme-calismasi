import { config } from './config'
import { logger } from './logger'
import * as redis from 'redis'

const client = redis.createClient({ url: config.redis.url })

client.connect()

client.on('error', error => {
    logger.error(error)
})

export const set = async (key: string, value: string) => {
    try {
        await client.set(key, value)
    } catch (error) {
        logger.error(error)
    }
}

export const get = async (key: string) => {
    try {
        return await client.get(key)
    }
    catch (error) {
        logger.error(error)
    }
}

export const del = async (key: string) => {
    try {
        await client.del(key)
    }
    catch (error) {
        logger.error(error)
    }
}