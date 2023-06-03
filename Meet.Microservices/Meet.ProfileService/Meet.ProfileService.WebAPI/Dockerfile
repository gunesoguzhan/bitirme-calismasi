FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app/Meet.Common/
COPY ./Meet.Common/ ./
RUN dotnet restore

WORKDIR /app/Meet.Microservices/Meet.ProfileService/Meet.ProfileService.WebAPI/
COPY ./Meet.Microservices/Meet.ProfileService/Meet.ProfileService.WebAPI/ ./
RUN dotnet restore

WORKDIR /app
RUN dotnet publish -c Release -o out ./Meet.Microservices/Meet.ProfileService/Meet.ProfileService.WebAPI/Meet.ProfileService.WebAPI.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Meet.ProfileService.WebAPI.dll"]