FROM microsoft/dotnet:2.0-sdk@sha256:bd00a3b03ae856b7d0994189722d5683857388eff4a2fb8bce77ddc7f86a908f AS build
WORKDIR /build
COPY . /dotnet-sources/.
RUN dotnet publish /dotnet-sources/Promitor.Scraper.csproj --configuration release -o output

FROM microsoft/aspnetcore:2.0@sha256:87ad2237a0e82a6630a331f6c8d5de199ded473f3a8284481eca9a6a560da3e8 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /dotnet-sources/output .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]