import { createLogger, transports, format } from 'winston'
import { config } from '@config'

const myFormat = format.printf(({ level, message, label, timestamp }) => {
    return `[${timestamp}] ${label} ${level}: ${message}`
})

export const logger = createLogger({
    format: format.combine(
        format.label({ label: config.winston.label }),
        format.timestamp(),
        format.colorize({ level: true }),
        myFormat
    ),
    transports: [new transports.Console()],
    level: config.winston.level
})