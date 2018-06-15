FROM microsoft/dotnet:2.1-sdk@sha256:5ce14e07bd69096f15f4792a85baf46983e655f6535fc87b7235373f3cec99a3 AS build
WORKDIR /build
COPY . /dotnet-sources/.
RUN dotnet publish /dotnet-sources/Promitor.Scraper.csproj --configuration release -o output

FROM microsoft/aspnetcore:2.0@sha256:5f964756fae50873c496915ad952b0f15df8ef985e4ac031d00b7ac0786162d0 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /dotnet-sources/output .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]