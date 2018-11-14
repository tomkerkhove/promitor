dotnet --info
dotnet build src/Promitor.sln --configuration release
dotnet test src/Promitor.Scraper.Tests.Unit/Promitor.Scraper.Tests.Unit.csproj --list-tests
dotnet test src/Promitor.Scraper.Tests.Unit/Promitor.Scraper.Tests.Unit.csproj --no-build
npm install -g snyk
snyk auth ${SNYK_TOKEN}
cd ./src/Promitor.Scraper.Host
snyk test --packageManager=nuget --org=tomkerkhove-github-marketplace
snyk monitor --packageManager=nuget --org=tomkerkhove-github-marketplace