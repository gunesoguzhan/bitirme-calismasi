## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Publish

## Todo
- RabbitMQ publish register events to the Profile Service
- Centralized logging
- Docker hosting
- User entity modifications (refresh token)
- Clear responses for bad http requests
- Cors
- Async communication with other microservices