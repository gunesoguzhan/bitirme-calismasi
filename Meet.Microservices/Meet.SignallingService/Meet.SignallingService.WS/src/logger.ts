import { createLogger, transports, format } from 'winston'
import { config } from './configuration/config'
import { SeqTransport } from '@datalust/winston-seq'

const myFormat = format.printf(({ level, message, label, timestamp }) => {
    return `[${timestamp}] ${label} ${level}: ${message}`
})

export const logger = createLogger({
    defaultMeta: { Application: config.winston.application },
    transports: [
        new transports.Console({
            format: format.combine(
                format.label({ label: config.winston.application }),
                format.timestamp(),
                format.colorize({ level: true }),
                myFormat
            )
        }),
        new SeqTransport({
            serverUrl: config.winston.seq.serverUrl,
            onError: (e => { console.error(e) }),
            handleExceptions: true,
            handleRejections: true,
            format: format.combine(
                format.errors({ stack: true }),
                format.json()
            )
        })],
    level: config.winston.level
})