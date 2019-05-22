FROM mcr.microsoft.com/dotnet/core/sdk:2.2.300 AS build
WORKDIR /src
COPY Promitor.Core/* Promitor.Core/
COPY Promitor.Core.Scraping/* Promitor.Core.Scraping/
COPY Promitor.Core.Telemetry/* Promitor.Core.Telemetry/
COPY Promitor.Core.Telemetry.Metrics/* Promitor.Core.Telemetry.Metrics/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
COPY Promitor.Integrations.AzureStorage/* Promitor.Integrations.AzureStorage/
COPY Promitor.Scraper.Host/* Promitor.Scraper.Host/
RUN dotnet --info
RUN dotnet publish Promitor.Scraper.Host/Promitor.Scraper.Host.csproj --configuration release -o app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.5 as runtime
WORKDIR /app
COPY --from=build /src/Promitor.Scraper.Host/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.Host.dll"]
