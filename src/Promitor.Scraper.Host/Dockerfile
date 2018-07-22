FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Promitor.Scraper.Host/* Promitor.Scraper.Host/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
RUN dotnet --info
RUN dotnet publish Promitor.Scraper.Host/Promitor.Scraper.Host.csproj --configuration release -o app

FROM microsoft/dotnet:2.1-aspnetcore-runtime as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /src/Promitor.Scraper.Host/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.Host.dll"]