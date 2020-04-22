FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201 AS build
WORKDIR /src
COPY Promitor.Agents.ResourceDiscovery/* Promitor.Agents.ResourceDiscovery/
COPY Promitor.Agents.Core/* Promitor.Agents.Core/
COPY Promitor.Core/* Promitor.Core/
RUN dotnet restore Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj
COPY . .
RUN dotnet build Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Promitor.Agents.ResourceDiscovery.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promitor.Agents.ResourceDiscovery.dll"]