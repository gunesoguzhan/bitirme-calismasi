import { createLogger, transports, format } from 'winston'

const myFormat = format.printf(({ level, message, label, timestamp }) => {
    return `${timestamp} [${label}] ${level}: ${message}`
})

export const logger = createLogger({
    format: format.combine(
        format.label({ label: 'SignallingService.WS.Development' }),
        format.timestamp(),
        format.colorize({ level: true }),
        myFormat
    ),
    transports: [new transports.Console()]
})