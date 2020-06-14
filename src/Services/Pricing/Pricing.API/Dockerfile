FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /source

COPY ["Kangaroo.sln", "Kangaroo.sln"]

COPY ["src/Services/Courier/Courier.API/Courier.API.csproj", "src/Services/Courier/Courier.API/"]
COPY ["src/Services/Courier/Courier.UnitTests/Courier.UnitTests.csproj", "src/Services/Courier/Courier.UnitTests/"]
COPY ["src/Services/Courier/Courier.FunctionalTests/Courier.FunctionalTests.csproj", "src/Services/Courier/Courier.FunctionalTests/"]

COPY ["src/Services/Delivery/Delivery.API/Delivery.API.csproj", "src/Services/Delivery/Delivery.API/"]
COPY ["src/Services/Delivery/Delivery.Infrastructure/Delivery.Infrastructure.csproj", "src/Services/Delivery/Delivery.Infrastructure/"]
COPY ["src/Services/Delivery/Delivery.Domain/Delivery.Domain.csproj", "src/Services/Delivery/Delivery.Domain/"]
COPY ["src/Services/Delivery/Delivery.UnitTests/Delivery.UnitTests.csproj", "src/Services/Delivery/Delivery.UnitTests/"]
COPY ["src/Services/Delivery/Delivery.FunctionalTests/Delivery.FunctionalTests.csproj", "src/Services/Delivery/Delivery.FunctionalTests/"]

COPY ["src/Services/Pricing/Pricing.API/Pricing.API.csproj", "src/Services/Pricing/Pricing.API/"]
COPY ["src/Services/Pricing/Pricing.UnitTests/Pricing.UnitTests.csproj", "src/Services/Pricing/Pricing.UnitTests/"]

COPY ["src/BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/EventBus/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/EventBus/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/EventBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj", "src/BuildingBlocks/EventBus/EventBusServiceBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBusRabbitMQ/"]
COPY ["src/BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHostCustomization/WebHost.Customization/"]

COPY ["src/Common/Kangaroo.Common/Kangaroo.Common.csproj", "src/Common/Kangaroo.Common/"]

RUN dotnet restore "Kangaroo.sln"

COPY . .

WORKDIR "/source/src/Services/Pricing/Pricing.API"
RUN dotnet build "Pricing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pricing.API.csproj" -c Release -o /app/publish

# this layer is used for running unit tests in Continuous Integration
FROM build AS unittest
WORKDIR "/source/src/Services/Pricing/Pricing.UnitTests"
# on running this build target it will run the unit tests
CMD ["dotnet", "test", "--logger:trx"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pricing.API.dll"]