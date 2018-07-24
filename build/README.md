# Build & Release

Collection of all YAML descriptions of our VSTS builds.

## Releasing Promitor Scraper
Releasing Promitor Scraper is done with `release-promitor-scraper.yaml`.

### Additional configuration
**Build Number Format:**

`$(Image.Version)`

**Variables:**

| Variable Name   | Description                                                       | Settable at queue time | Default Value                                       |
|:----------------|:------------------------------------------------------------------|:---------------------:|:----------------------------------------------------|
| `Release.Title` | Title of the GitHub release                                       | :white_check_mark:     | *v$(Build.BuildNumber)*                             |
| `Image.Version` | New image version of the container image                          | :white_check_mark:     | *0.1.0*                                             |
