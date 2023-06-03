FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app/Meet.Common/
COPY ./Meet.Common/ ./
RUN dotnet restore

WORKDIR /app/Meet.ApiGateway/Meet.ApiGateway.WebAPI/
COPY ./Meet.ApiGateway/Meet.ApiGateway.WebAPI/ ./
RUN dotnet restore

WORKDIR /app
RUN dotnet publish -c Release -o out ./Meet.ApiGateway/Meet.ApiGateway.WebAPI/Meet.ApiGateway.WebAPI.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Meet.ApiGateway.WebAPI.dll"]