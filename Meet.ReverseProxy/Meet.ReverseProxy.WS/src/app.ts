import httpProxy from 'http-proxy'

httpProxy.createProxyServer({
    target: process.env.NODE_ENV === 'production'
        ? 'http://meet-signalling-service:3258'
        : 'http://localhost:3258',
    ws: true,
}).listen(3000)