## Tips

### Docker build
- `docker build -t meet-signalling-service .`

### Docker run
- `docker run -p 3000:80 --network bitirme-calismasi_default --name signal meet-signalling-service`

## Todo
- centralized logging
- cors
- docker hosting
- seperation of config for development and production environment