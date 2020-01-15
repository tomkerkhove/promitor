FROM mcr.microsoft.com/dotnet/core/sdk:3.1.101-alpine AS build
WORKDIR /src
COPY Promitor.Core/* Promitor.Core/
COPY Promitor.Core.Configuration/* Promitor.Core.Configuration/
COPY Promitor.Core.Scraping/* Promitor.Core.Scraping/
COPY Promitor.Core.Telemetry/* Promitor.Core.Telemetry/
COPY Promitor.Core.Telemetry.Metrics/* Promitor.Core.Telemetry.Metrics/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
COPY Promitor.Integrations.AzureStorage/* Promitor.Integrations.AzureStorage/
COPY Promitor.Scraper.Host/* Promitor.Scraper.Host/
RUN dotnet publish Promitor.Scraper.Host/Promitor.Scraper.Host.csproj --configuration release --output app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.1-alpine as runtime
WORKDIR /app
COPY --from=build /src/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.Host.dll"]
