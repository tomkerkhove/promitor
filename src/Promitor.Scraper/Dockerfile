FROM microsoft/dotnet:2.0-sdk@sha256:bd00a3b03ae856b7d0994189722d5683857388eff4a2fb8bce77ddc7f86a908f AS build
WORKDIR /build
COPY . /dotnet-sources/.
RUN dotnet publish /dotnet-sources/Promitor.Scraper.csproj --configuration release -o output

FROM microsoft/aspnetcore:2.0@sha256:cd0ce9bddfccdb95773b39034a85f3dd061562ab04547027b09061abc5c91575 as runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /dotnet-sources/output .

ENTRYPOINT ["dotnet", "Promitor.Scraper.dll"]