FROM mcr.microsoft.com/dotnet/core/sdk:3.1.202 AS build
WORKDIR /src
COPY Promitor.Agents.ResourceDiscovery/* Promitor.Agents.ResourceDiscovery/
COPY Promitor.Agents.Core/* Promitor.Agents.Core/
COPY Promitor.Core/* Promitor.Core/
RUN dotnet publish Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj --configuration release --output /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.4 AS runtime
WORKDIR /app
ARG CONFIG_FOLDER="/config/"
ENV PROMITOR_INTERNAL_CONFIG_FOLDER=$CONFIG_FOLDER
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Promitor.Agents.ResourceDiscovery.dll"]