FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj", "Promitor.Agents.ResourceDiscovery/"]
RUN dotnet restore "Promitor.Agents.ResourceDiscovery/Promitor.Agents.ResourceDiscovery.csproj"
COPY . .
WORKDIR "/src/Promitor.Agents.ResourceDiscovery"
RUN dotnet build "Promitor.Agents.ResourceDiscovery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Promitor.Agents.ResourceDiscovery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Promitor.Agents.ResourceDiscovery.dll"]