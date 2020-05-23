FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /source
COPY ["src/Services/Delivery/Delivery.API/Delivery.API.csproj", "src/Services/Delivery/Delivery.API/"]
COPY ["src/Services/Delivery/Delivery.Infrastructure/Delivery.Infrastructure.csproj", "src/Services/Delivery/Delivery.Infrastructure/"]
COPY ["src/Services/Delivery/Delivery.Domain/Delivery.Domain.csproj", "src/Services/Delivery/Delivery.Domain/"]
COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
RUN dotnet restore "src/Services/Delivery/Delivery.API/Delivery.API.csproj"
COPY . .
WORKDIR "/source/src/Services/Delivery/Delivery.API"
RUN dotnet build "Delivery.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Delivery.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delivery.API.dll"]