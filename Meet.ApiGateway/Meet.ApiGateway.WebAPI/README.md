## Tips

### Set Environment Variable
- powershell: `$env:ASPNETCORE_ENVIRONMENT = "<environment name>"`

### Migration
- `dotnet ef migrations add <migration name> -o ./Database/Migrations`

### Run
- `dotnet run --launch-profile <profile name>`

### Docker build
- Note: To build with referenced project, go to parent directory.
- `docker build -t meet-api-gateway -f .\Meet.ApiGateway\Meet.ApiGateway.WebAPI\Dockerfile .`

### Docker run
- `docker run -p 4000:80 --network bitirme-calismasi_default --name api-gateway meet-api-gateway`

## Todo
- Cors