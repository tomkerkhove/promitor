---
date: 2021-01-01T20:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag added %}} Provide support for pushing metrics to an OpenTelemetry Collector ([docs](https://docs.promitor.io/latest/scraping/runtime-configuration/#opentelemetry)
 | [#1824](https://github.com/tomkerkhove/promitor/issues/1824))
- {{% tag added %}} Provide capability to define metric window starting point to query in Azure Monitor ([docs](https://docs.promitor.io/latest/scraping/runtime-configuration/#azure-monitor)
 | [#2023](https://github.com/tomkerkhove/promitor/issues/2023))
- {{% tag fixed %}} Avoid querying too much data from Azure Monitor by using 6 days of metric data ([#2023](https://github.com/tomkerkhove/promitor/issues/2023))
- {{% tag fixed %}} Agent no longer fails to create background jobs due to CultureNotFoundException ([#2089](https://github.com/tomkerkhove/promitor/issues/2089))
- {{% tag fixed %}} Ensure resource discovery is optional and does not block startup ([#2104](https://github.com/tomkerkhove/promitor/issues/2104))

#### Resource Discovery

None.
