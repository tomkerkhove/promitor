FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201 AS build
WORKDIR /src
COPY Promitor.Core/* Promitor.Core/
COPY Promitor.Agents.Core/* Promitor.Agents.Core/
COPY Promitor.Core.Configuration/* Promitor.Core.Configuration/
COPY Promitor.Core.Scraping/* Promitor.Core.Scraping/
COPY Promitor.Core.Telemetry/* Promitor.Core.Telemetry/
COPY Promitor.Core.Telemetry.Metrics/* Promitor.Core.Telemetry.Metrics/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
COPY Promitor.Integrations.AzureStorage/* Promitor.Integrations.AzureStorage/
COPY Promitor.Integrations.Sinks.Core/* Promitor.Integrations.Sinks.Core/
COPY Promitor.Integrations.Sinks.Statsd/* Promitor.Integrations.Sinks.Statsd/
COPY Promitor.Agents.Scraper/* Promitor.Agents.Scraper/
RUN dotnet publish Promitor.Agents.Scraper/Promitor.Agents.Scraper.csproj --configuration release --output app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3 as runtime
WORKDIR /app
ARG CONFIG_FOLDER="/config/"
ENV PROMITOR_INTERNAL_CONFIG_FOLDER=$CONFIG_FOLDER
COPY --from=build /src/app .

ENTRYPOINT ["dotnet", "Promitor.Agents.Scraper.dll"]
