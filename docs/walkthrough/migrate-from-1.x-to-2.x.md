---
layout: default
title: Migrate from Promitor Scraper 1.x to 2.x
---

Here is a migration guide from Promitor Scraper v1.x to v2.x.

For a complete overview of our changelog, we recommend going to [changelog.promitor.io](https://changelog.promitor.io).

## Migrate to new metric sink concept

As of Promitor Scraper v1.6 we have introduced the concept of [metric sinks](https://promitor.io/configuration/v2.x/runtime#metric-sinks)
 allowing you to emit scraped Azure Monitor metrics to multiple systems.

With Promitor v2.0, we are removing support for our legacy Prometheus configuration.

When using the following configuration:

```yaml
prometheus:
  metricUnavailableValue: NaN
  enableMetricTimestamps: false
  scrapeEndpoint:
    baseUriPath: /metrics
```

You can easily migrate it to our Prometheus Scraping endpoint sink as following:

```yaml
metricSinks:
  prometheusScrapingEndpoint:
    metricUnavailableValue: NaN
    enableMetricTimestamps: false
    baseUriPath: /metrics
```

For more information, we recommend reading our
 [documentation](https://promitor.io/configuration/v2.x/runtime#prometheus-scraping-endpoint) concerning our Prometheus
  Scraping endpoint.

## Migrate to OpenAPI 3.0 & UI

All Promitor APIs have been migrated from Swagger to OpenAPI 3.0 specification.

Before, our Swagger docs were accessible via:

- Swagger UI on `/swagger`
- Raw documentation on `/swagger/v1/swagger.json`

Our OpenAPI 3.0 docs are available on:

- Swagger UI on `/api/docs`
- Raw documentation on `/api/v1/docs.json`

[&larr; back](/)
