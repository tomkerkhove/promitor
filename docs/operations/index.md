---
layout: default
title: Operating Promitor
---

Here is an overview of how you can operate Promitor.

- [Health](#health)
  - [Consuming the health endpoint](#consuming-the-health-endpoint)
- [Performance](#performance)
  - [Scraping Prometheus endpoint](#scraping-prometheus-endpoint)
  - [Scraping Azure Monitor](#scraping-azure-monitor)
- [System](#system)
  - [Consuming the System endpoint](#consuming-the-system-endpoint)
  - [Exploring our REST APIs](#exploring-our-rest-apis)
- [Integrations](#integrations)
  - [Azure Resource Manager API - Consumption & Throttling](#azure-resource-manager-api---consumption--throttling)
  - [Azure Monitor](#azure-monitor)

## Health

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

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

## Performance

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

You can easily monitor the performance of Promitor through the following Prometheus metrics:

- `promitor_runtime_dotnet_collection_count_total` - Provides information related to garbage collection count per generation
- `promitor_runtime_dotnet_totalmemory` - Provides information related to total known allocated memory
- `promitor_runtime_process_cpu_seconds_total` - Provides information related to total user & system CPU time spent in seconds
- `promitor_runtime_process_virtual_bytes` - Provides information related to virtual memory size
- `promitor_runtime_process_working_set` - Provides information related to process working set
- `promitor_runtime_process_private_bytes` - Provides information related to process private memory size
- `promitor_runtime_process_num_threads` - Provides information related to total number of threads
- `promitor_runtime_process_processid` - Provides information related to process ID
- `promitor_runtime_process_start_time_seconds` - Provides information related to the start time of the process since
 unix epoch in seconds
- `promitor_runtime_http_request_duration_seconds` - Provides information related to the performance of HTTP routes and outcomes

```text
# HELP promitor_runtime_http_request_duration_seconds duration histogram of http responses labeled with: status_code, method, path
# TYPE promitor_runtime_http_request_duration_seconds histogram
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.005"} 30
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.01"} 31
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.025"} 31
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.05"} 32
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.075"} 33
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.1"} 33
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.25"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="0.75"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="1"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="2.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="7.5"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="10"} 34
promitor_runtime_http_request_duration_seconds_bucket{status_code="200",method="GET",path="/scrape",le="+Inf"} 34
promitor_runtime_http_request_duration_seconds_sum{status_code="200",method="GET",path="/scrape"} 0.27116070000000003
promitor_runtime_http_request_duration_seconds_count{status_code="200",method="GET",path="/scrape"} 34
```

### Scraping Prometheus endpoint

Every Promitor agent supports exposing Prometheus metrics:

- **Resource Discovery agent** - Exposed on `/metrics` endpoint
- **Scraper agent** - Exposed through Prometheus metric sink ([docs](/configuration/v2.x/runtime/scraper#prometheus-scraping-endpoint))

### Scraping Azure Monitor

You can easily monitor the performance of Promitor Scraper agent integrating with Azure Monitor through the following Prometheus metrics:

- `promitor_scrape_error` - Provides indication of all configured metrics that were unable to be scraped in Azure Monitor

```prom
# HELP promitor_scrape_error Provides an indication that the scraping of the resource has failed
# TYPE promitor_scrape_error gauge
promitor_scrape_error{metric_name="promitor_demo_app_insights_dependency_duration_200_OK",resource_group="docker-hub-metrics",resource_name="Microsoft.Insights/Components/docker-hub-metrics",resource_type="Generic",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="c8819874-9e56-4e3f-b1a8-1c0325138f27"} 1 1623691623231
```

- `promitor_scrape_success` - Provides indication of all configured metrics that were successfully scraped and reported in
the configured metric sinks

```text
# HELP promitor_scrape_success Provides an indication that the scraping of the resource was successful
# TYPE promitor_scrape_success gauge
promitor_scrape_success{metric_name="promitor_demo_automation_update_deployment_machine_runs",resource_group="promitor-sources",resource_name="promitor-sandbox",resource_type="AutomationAccount",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="c8819874-9e56-4e3f-b1a8-1c0325138f27"} 1 1623691626335
```

## System

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

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

## Integrations

### Azure Resource Manager API - Consumption & Throttling

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

Promitor exposes runtime metrics to provide insights on the API consumption of
Azure Resource Manager API:

- `promitor_ratelimit_arm` - Indication how many calls are still available before
  Azure Resource Manager is going to throttle us. Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `subscription_id` - _Id of the subscription that is being interacted with_
  - `app_id` - _Id of the application that is being used to interact with API_

You can read more about the Azure Resource Manager limitations on [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits).

### Azure Monitor

Promitor interacts with Azure Monitor API to scrape all the required metrics.

During troubleshooting it can be interesting to gain insights on what the API returns, for which you can opt-in.

You can opt-in for it by configuring the [runtime telemetry](/configuration/v2.x/runtime/scraper#azure-monitor).

[&larr; back](/)
