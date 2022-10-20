---
date: 2022-11-11T20:46:47+02:00
weight: 1
version:
---

#### Scraper

- {{% tag added %}} Provide Azure Log Analytics scraper ([docs](https://docs.promitor.io/v2.9/scraping/providers/log-analytics/)
  | [#2132](https://github.com/tomkerkhove/promitor/pull/2132))
- {{% tag added %}} Capability to use multiple metrics dimensions with new metrics configuration property
 `metrics[x].azureMetricConfiguration.dimensions` ([#1820](https://github.com/tomkerkhove/promitor/issues/1820))
- {{% tag deprecated %}} Old metrics configuration property for metric dimension `metrics[x].azureMetricConfiguration.dimension`
 ([#1820](https://github.com/tomkerkhove/promitor/issues/1820))

#### Resource Discovery

None.
