import { config } from './config'
import { logger } from './logger'
import * as redis from 'redis'

let client: any

export const initializeRedis = async () => {
    client = redis.createClient({ url: config.redis.url })
    logger.debug('Redis: client created.')
    await client.connect()
    logger.debug('Redis: client connected.')
    client.on('error', error => {
        logger.error(`Redis error: ${error}`)
    })
}

export const set = async (key: string, value: string) => {
    try {
        await client.set(key, value)
        logger.debug(`Redis: set succeeded. Key: ${key} Value: ${value}`)
    } catch (error) {
        logger.error(`Redis error: ${error}`)
    }
}

export const get = async (key: string) => {
    try {
        const value = await client.get(key)
        logger.debug(`Redis: get succeeded. Key: ${key} Value: ${value}`)
        return value
    }
    catch (error) {
        logger.error(`Redis error: ${error}`)
        return null
    }
}

export const del = async (key: string) => {
    try {
        await client.del(key)
        logger.debug(`Redis: del succeeded. Key ${key}`)
    }
    catch (error) {
        logger.error(`Redis error: ${error}`)
    }
}