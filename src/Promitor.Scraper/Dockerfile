FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Promitor.Scraper/* Promitor.Scraper/
COPY Promitor.Integrations.AzureMonitor/* Promitor.Integrations.AzureMonitor/
RUN dotnet --info
RUN dotnet publish Promitor.Scraper/Promitor.Scraper.csproj --configuration release -o app

FROM microsoft/dotnet:2.1-aspnetcore-runtime as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /src/Promitor.Scraper/app .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]