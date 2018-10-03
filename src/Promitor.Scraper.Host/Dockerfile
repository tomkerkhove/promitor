FROM microsoft/dotnet:2.1.403-sdk AS build
WORKDIR /src
COPY Promitor.Scraper.Host/* Promitor.Scraper.Host/
COPY Promitor.Core.Telemetry/* Promitor.Core.Telemetry/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
RUN dotnet --info
RUN dotnet publish Promitor.Scraper.Host/Promitor.Scraper.Host.csproj --configuration release -o app

FROM microsoft/dotnet:2.1.5-aspnetcore-runtime as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /src/Promitor.Scraper.Host/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.Host.dll"]