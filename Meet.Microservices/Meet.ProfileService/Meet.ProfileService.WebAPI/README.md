## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Docker build
- Note: To build with referenced project, go to parent directory.
- `docker build -t meet-profile-service -f .\Meet.Microservices\Meet.ProfileService\Meet.ProfileService.WebAPI\Dockerfile .`

### Docker run
- `docker run -p 5225:80 --network bitirme-calismasi_default --name identit meet-profile-service`

## Todo
- Profile entity modifications (profile photo, blocked users, blockers)
- Clear responses for bad http requests
- Cors