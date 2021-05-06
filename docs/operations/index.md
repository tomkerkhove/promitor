---
layout: default
title: Operating Promitor
---

Here is an overview of how you can operate Promitor.

## Health

Promitor provides a basic health endpoint that indicates the state of the scraper.

Health endpoints can be useful for monitoring the scraper, running sanity tests
after deployments or use it for sending liveness / health probes.

### Consuming the health endpoint

You can check the status with a simple `GET`:

```shell
❯ curl -i -X GET "http://<uri>/api/v1/health"
```

Health is currently indicated via the HTTP response status:

- `200 OK` - The scraper is healthy
- `503 Service Unavailable` - The scraper is unhealthy

The endpoint provides more details on integration with following dependencies:

- **Promitor Resource Discovery** (when configured)

## Azure Resource Manager API - Consumption & Throttling

Promitor exposes runtime metrics to provide insights on the API consumption of
Azure Resource Manager API:

- `promitor_ratelimit_arm` - Indication how many calls are still available before
  Azure Resource Manager is going to throttle us. Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `subscription_id` - _Id of the subscription that is being interacted with_
  - `app_id` - _Id of the application that is being used to interact with API_

You can read more about the Azure Resource Manager limitations on [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits).

## Azure Monitor Integration

Promitor interacts with Azure Monitor API to scrape all the required metrics.

During troubleshooting it can be interesting to gain insights on what the API returns, for which you can opt-in.

You can opt-in for it by configuring the [runtime telemetry](/configuration/v2.x/runtime/scraper#azure-monitor).

## Configuration REST APIs

In order to run Promitor certain aspects have to be configured. Once up & running,
you typically do not touch or open the configuration anymore and just intereact
with Promitor.

For some scenarios it can be useful to know what was configured:

- Metrics are not showing up in the scraping endpoint, was it configured correctly?
- We don't see telemetry in our sink, was it turned on?
- ...

Therefor we provide the following REST APIs:

- **Get Metrics Declaration** - Provides a list of metrics that are being scraped
- **Get Runtime Configuration** - Provides an overview of how the runtime is configured

For security reasons, some sections of the configuration might be sanitized in
the response to avoid leaking secrets.

## System

Promitor provides a basic system endpoint that provides information about itself such as its version.

### Consuming the System endpoint

You can check the status with a simple `GET`:

```shell
❯ curl -i -X GET "http://<uri>/api/v1/system"
```

### Exploring our REST APIs

We provide API documentation to make it easier for you to consume our REST APIs them:

- **OpenAPI 3.0 format** ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1-green.svg)
  - You can explore it with OpenAPI UI on `/api/docs`
  - You can find the raw documentation on `/api/v1/docs.json`
- **Swagger 2.0 format** [![Deprecation Badge](https://img.shields.io/badge/Deprecated%20as%20of-v1.1-red)](http://changelog.promitor.io/)
  - You can explore it with Swagger UI on `/swagger`
  - You can find the raw documentation on `/swagger/v1/swagger.json`

[&larr; back](/)
