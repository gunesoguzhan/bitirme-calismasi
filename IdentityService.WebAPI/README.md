## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Publish

## Todo
- User entity modifications (refresh token, profile photo url, blocked users, blockers etc.)
- Clear responses for bad http requests
- Blocking, unblocking users
- Search, modify, delete endpoints for users
- Centralized logging
- Cors
- Async communication with other microservices
- Docker hosting