## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Docker build
- Note: To build with referenced project, go to parent directory.
- `docker build -t meet-identity-service -f .\Meet.Microservices\Meet.IdentityService\Meet.IdentityService.WebAPI\Dockerfile .`

### Docker run
- `docker run -p 5181:80 --network bitirme-calismasi_default --name identit meet-identity-service`

## Todo
- User entity modifications (refresh token)
- Clear responses for bad http requests
- Cors