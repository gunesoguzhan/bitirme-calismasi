FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app/Meet.Common/
COPY ./Meet.Common/ ./
RUN dotnet restore

WORKDIR /app/Meet.Microservices/Meet.RoomService/Meet.RoomService.WebAPI/
COPY ./Meet.Microservices/Meet.RoomService/Meet.RoomService.WebAPI/ ./
RUN dotnet restore

WORKDIR /app
RUN dotnet publish -c Release -o out ./Meet.Microservices/Meet.RoomService/Meet.RoomService.WebAPI/Meet.RoomService.WebAPI.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Meet.RoomService.WebAPI.dll"]