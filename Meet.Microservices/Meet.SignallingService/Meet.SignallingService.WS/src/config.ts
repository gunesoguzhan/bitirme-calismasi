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
        audience: 'docker',
        issuer: 'docker',
        securityKey: 'This is the security key for production.'
    },
    rabbitMQ: {
        host: 'rabbitmq',
        port: 5672,
        username: 'admin',
        password: '123456',
        queues: [
            'Meet-MessageReceived'
        ]
    },
    redis: {
        url: 'redis://redis:6379'
    },
    winston: {
        level: 'debug',
        label: 'Meet.SignallingService.WS.Production'
    }
}