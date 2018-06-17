FROM microsoft/dotnet:2.0-sdk@sha256:8fc6dd50b2736c638f3dcdba2604ff9a36a7b6d85b385484c8014ff7ba2344de AS build
WORKDIR /build
COPY . /dotnet-sources/.
RUN dotnet publish /dotnet-sources/Promitor.Scraper.csproj --configuration release -o output

FROM microsoft/aspnetcore:2.0@sha256:5f964756fae50873c496915ad952b0f15df8ef985e4ac031d00b7ac0786162d0 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /dotnet-sources/output .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]