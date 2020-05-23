FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /source
COPY ["src/Services/Courier/Courier.API/Courier.API.csproj", "src/Services/Courier/Courier.API/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
RUN dotnet restore "src/Services/Courier/Courier.API/Courier.API.csproj"
COPY . .
WORKDIR "/source/src/Services/Courier/Courier.API"
RUN dotnet build "Courier.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Courier.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courier.API.dll"]
