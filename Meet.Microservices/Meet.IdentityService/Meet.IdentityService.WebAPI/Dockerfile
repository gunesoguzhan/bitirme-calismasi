FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app/Meet.Common/
COPY ./Meet.Common/ ./
RUN dotnet restore

WORKDIR /app/Meet.Microservices/Meet.IdentityService/Meet.IdentityService.WebAPI/
COPY ./Meet.Microservices/Meet.IdentityService/Meet.IdentityService.WebAPI/ ./
RUN dotnet restore

WORKDIR /app
RUN dotnet publish -c Release -o out ./Meet.Microservices/Meet.IdentityService/Meet.IdentityService.WebAPI/Meet.IdentityService.WebAPI.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Meet.IdentityService.WebAPI.dll"]