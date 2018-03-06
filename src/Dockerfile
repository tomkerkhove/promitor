FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /build

#  Build setup
COPY Promitor.Scraper/. .
RUN dotnet restore Promitor.Scraper.csproj
RUN dotnet publish Promitor.Scraper.csproj --configuration release -o dotnet-output

FROM microsoft/aspnetcore:2.0 as runtime
# Copy default configuration
WORKDIR /config
COPY --from=build /build/dotnet-output/metrics.yaml .

# Copy application
WORKDIR /app
EXPOSE 80
COPY --from=build /build/dotnet-output/. .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]
