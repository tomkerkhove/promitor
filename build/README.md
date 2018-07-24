# Build & Release

Collection of all YAML descriptions of our VSTS builds.

## Releasing Promitor Scraper
Releasing Promitor Scraper is done with `release-promitor-scraper.yaml`.

### Additional configuration
Build Number Format: `$(Image.Version)`

Variables:
| Variable Name | Settable at queue time | Default Value                                       |
|:--------------|:-----------------------|:----------------------------------------------------|
| Release.Title | :x:                    | `v$(Build.BuildNumber)`                             |
| Image.Name    | :x:                    | `tomkerkhove/promitor-scraper:$(Build.BuildNumber)` |
| Image.Version | :x:                    | `0.1.0`                                             |