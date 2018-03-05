FROM microsoft/dotnet:2.0-sdk AS build

#  Build setup
COPY Promitor.Scraper/. .
RUN dotnet restore Promitor.Scraper.csproj
RUN dotnet publish Promitor.Scraper.csproj --configuration release -o dotnet-output

FROM microsoft/aspnetcore:2.0 as runtime
# Copy default configuration
WORKDIR config
COPY --from=build /dotnet-output/. .

# Copy application
WORKDIR /app
EXPOSE 80
COPY --from=build /app/dotnet-output/. .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]
