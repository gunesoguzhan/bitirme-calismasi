## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Docker build
- Note: To build with referenced project, go to parent directory.
- `docker build -t meet-room-service -f .\Meet.Microservices\Meet.RoomService\Meet.RoomService.WebAPI\Dockerfile .`

### Docker run
- `docker run -p 5132:80 --network bitirme-calismasi_default --name meet-room-service meet-room-service`

## Todo
- User Updated
- User Deleted
- Clear responses for bad http requests
- Cors