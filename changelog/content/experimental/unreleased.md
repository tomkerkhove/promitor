---
date: 2022-11-22T12:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag added %}} Provide custom formatting for emitting metrics using StatsD sink in Geneva format
- {{% tag added %}} Provide capability to read Azure AD service principal secret from a file
- {{% tag added %}} Provide Azure Log Analytics scraper ([docs](https://docs.promitor.io/v2.9/scraping/providers/log-analytics/)
| [#2132](https://github.com/tomkerkhove/promitor/pull/2132))
- {{% tag added %}} Provide support for Azure Data Explorer Clusters. ([docs](https://docs.promitor.io/v2.9/scraping/providers/data-explorer-clusters.md)
- {{% tag added %}} Provide support for Azure NAT Gateway. ([docs](https://docs.promitor.io/v2.9/scraping/providers/nat-gateway.md)
- {{% tag fixed %}} Fixed a bug where startup throws scheduling exception due to metric misconfiguration
- {{% tag fixed %}} Fixed a bug where resource discovery for Azure Container Instances was not working
- {{% tag fixed %}} Fixed a bug where Promitor was reported as `unknown_service:dotnet` job in OpenTelemetry Collector

#### Resource Discovery

- {{% tag added %}} Provide path to read app secret key from file
- {{% tag added %}} Provide support for Azure Data Explorer Clusters. ([docs](https://docs.promitor.io/v2.9/scraping/providers/data-explorer-clusters.md)
- {{% tag added %}} Provide support for Azure NAT Gateway. ([docs](https://docs.promitor.io/v2.9/scraping/providers/nat-gateway.md)
