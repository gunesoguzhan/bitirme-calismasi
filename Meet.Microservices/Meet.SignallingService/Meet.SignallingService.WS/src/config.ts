export const config = {
    server: {
        port: 3000
    },
    socketServer: {
        cors: {
            origins: [
                'http://localhost:5173'
            ],
            credentials: true
        }
    },
    jwt: {
        audience: 'localhost',
        issuer: 'localhost',
        securityKey: 'This is the security key for development.'
    },
    rabbitMQ: {
        url: 'localhost:5672',
        username: 'admin',
        password: '123456'
    },
    redis: {
        url: 'redis://localhost:6379'
    },
    winston: {
        level: 'debug',
        label: 'SignallingService.WS.Development'
    }
}