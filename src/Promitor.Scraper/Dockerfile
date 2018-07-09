FROM microsoft/dotnet:2.0-sdk@sha256:bd00a3b03ae856b7d0994189722d5683857388eff4a2fb8bce77ddc7f86a908f AS build
WORKDIR /build
COPY . /dotnet-sources/.
RUN dotnet publish /dotnet-sources/Promitor.Scraper.csproj --configuration release -o output

FROM microsoft/aspnetcore:2.0@sha256:5f964756fae50873c496915ad952b0f15df8ef985e4ac031d00b7ac0786162d0 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /dotnet-sources/output .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]