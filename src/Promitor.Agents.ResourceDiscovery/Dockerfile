FROM mcr.microsoft.com/dotnet/core/sdk:3.1.302 AS build
WORKDIR /src
COPY . .
RUN dotnet publish Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj --configuration release --output /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.6 AS runtime
WORKDIR /app
ARG CONFIG_FOLDER="/config/"
ENV PROMITOR_INTERNAL_CONFIG_FOLDER=$CONFIG_FOLDER
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Promitor.Agents.ResourceDiscovery.dll"]