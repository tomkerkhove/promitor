FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Promitor.Scraper/Promitor.Scraper.csproj Promitor.Scraper/
COPY Promitor.Integrations.AzureMonitor/Promitor.Integrations.AzureMonitor.csproj Promitor.Integrations.AzureMonitor/
RUN dotnet --info
RUN dotnet publish Promitor.Scraper/Promitor.Scraper.csproj --configuration release -o app

FROM microsoft/aspnetcore:2.1 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /src/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]