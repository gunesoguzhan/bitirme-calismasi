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
        audience: 'http://localhost',
        issuer: 'http://localhost',
        securityKey: 'This is the security key for development.'
    },
    winston: {
        level: 'info',
        label: 'SignallingService.WS.Development'
    }
}