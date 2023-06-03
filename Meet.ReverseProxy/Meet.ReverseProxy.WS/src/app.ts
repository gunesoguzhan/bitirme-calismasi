import httpProxy from 'http-proxy'

httpProxy.createProxyServer({
    target: 'http://localhost:3258',
    ws: true,
}).listen(3000)