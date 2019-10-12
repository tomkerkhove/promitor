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

In the future, the endpoint will be more advanced by giving detailed status on
dependencies as well.

## Azure Resource Manager API - Consumption & Throttling

Promitor exposes runtime metrics to provide insights on the API consumption of
Azure Resource Manager API:

- `promitor_ratelimit_arm` - Indication how many calls are still available before
  Azure Resource Manager is going to throttle us. Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `subscription_id` - _Id of the subscription that is being interacted with_
  - `app_id` - _Id of the application that is being used to interact with API_

You can read more about the Azure Resource Manager limitations on [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits).

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

[&larr; back](/)
