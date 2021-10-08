---
layout: default
title: Operating Promitor
---

Here is an overview of how you can operate Promitor.

- [Health](#health)
  - [Consuming the health endpoint](#consuming-the-health-endpoint)
- [Discovery](#discovery)
  - [Subscription](#subscription)
  - [Resource Groups](#resource-groups)
- [Performance](#performance)
  - [Scraping Prometheus endpoint](#scraping-prometheus-endpoint)
  - [Scraping Azure Monitor](#scraping-azure-monitor)
- [System](#system)
  - [Consuming the System endpoint](#consuming-the-system-endpoint)
  - [Exploring our REST APIs](#exploring-our-rest-apis)
- [Integrations](#integrations)
  - [Azure Resource Manager API - Consumption & Throttling](#azure-resource-manager-api---consumption--throttling)
  - [Azure Resource Graph](#azure-resource-graph)
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

## Discovery

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-No-red.svg)

**Promitor Resource Discovery** provides a way to discover the resources for our Scraper agent to dynamically scrape resources.

Next to that, it provides a variety of system metrics that provides information concerning your Azure landscape.

### Subscription

Our `promitor_azure_landscape_subscription_info` metrics provides an overview of all the Azure subscriptions that
 Promitor is able to discover in your Azure Landscape.

It provides the following tags with more information:

- `tenant_id` - Id of the Azure tenant
- `subscription_name` - Name of the Azure subscription
- `subscription_id` - Id of the Azure subscription
- `state` - Indication of the state of the subscription ([docs](https://docs.microsoft.com/en-us/azure/cost-management-billing/manage/subscription-states))
- `spending_limit` - Indication whether or not there is a spending limit
- `quota_id` - Id of the Azure subscription used to manage quotas
- `authorization` - Type of authorization that is being used

```prometheus
# HELP promitor_azure_landscape_subscription_info Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.
# TYPE promitor_azure_landscape_subscription_info gauge
promitor_azure_landscape_subscription_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_name="Windows Azure MSDN - Visual Studio Ultimate",subscription_id="0329dd2a-59dc-4493-aa54-cb01cb027dc2",state="Enabled",spending_limit="On",quota_id="MSDN_2014-09-01",authorization="RoleBased"} 1 1628779903451
promitor_azure_landscape_subscription_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_name="Visual Studio Enterprise",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",state="Enabled",spending_limit="Off",quota_id="Sponsored_2016-01-01",authorization="RoleBased"} 1 1628779903451
```

### Resource Groups

Our `promitor_azure_landscape_resource_group_info` metrics provides an overview of all the Azure resource groups that
 Promitor is able to discover in your Azure Landscape across all your subscriptions.

It provides the following tags with more information:

- `tenant_id` - Id of the Azure tenant
- `subscription_id` - Id of the Azure subscription
- `resource_group_name` - Name of the Azure resource group
- `region` - Region in which the resource group is located
- `provisioning_state` - State of the resource group
- `managed_by` - Id of the Azure resource managing this resource group, for example an Azure Kubernetes Service cluster.

```prometheus
# HELP promitor_azure_landscape_resource_group_info Provides information concerning the Azure resource groups in the landscape that Promitor has access to.
# TYPE promitor_azure_landscape_resource_group_info gauge
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="NetworkWatcherRG",region="westeurope",provisioning_state="Succeeded",managed_by="n/a"} 1 1628779903423
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="promitor-testing-resource-discovery-eu",region="westeurope",provisioning_state="Succeeded",managed_by="n/a"} 1 1628779903423
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="MC_keda-demos_keda-demos_westeurope",region="westeurope",provisioning_state="Succeeded",managed_by="/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourcegroups/keda-demos/providers/Microsoft.ContainerService/managedClusters/keda-demos"} 1 1628779903423
```

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

You can easily monitor the performance of Promitor Scraper agent integrating with Azure Monitor
 through the following Prometheus metrics:

- `promitor_scrape_error` - Provides indication of all configured metrics that were unable to be scraped in Azure Monitor

```prom
# HELP promitor_scrape_error Provides an indication that the scraping of the resource has failed
# TYPE promitor_scrape_error gauge
promitor_scrape_error{metric_name="promitor_demo_app_insights_dependency_duration_200_OK",resource_group="docker-hub-metrics",resource_name="Microsoft.Insights/Components/docker-hub-metrics",resource_type="Generic",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf"} 1 1623691623231
```

- `promitor_scrape_success` - Provides indication of all configured metrics that were successfully scraped and reported in
the configured metric sinks

```text
# HELP promitor_scrape_success Provides an indication that the scraping of the resource was successful
# TYPE promitor_scrape_success gauge
promitor_scrape_success{metric_name="promitor_demo_automation_update_deployment_machine_runs",resource_group="promitor-sources",resource_name="promitor-sandbox",resource_type="AutomationAccount",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf"} 1 1623691626335
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

- `promitor_ratelimit_arm_throttled` - Indication whether or not we are being throttled by Azure Resource Manager
 (ARM). Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `subscription_id` - _Id of the subscription that is being interacted with_
  - `app_id` - _Id of the application that is being used to interact with API_

```text
# HELP promitor_ratelimit_arm Indication how many calls are still available before Azure Resource Manager (ARM) is going to throttle us.
# TYPE promitor_ratelimit_arm gauge
promitor_ratelimit_arm{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0329dd2a-59dc-4493-aa54-cb01cb027dc2",app_id="ceb249a3-44ce-4c90-8863-6776336f5b7e"} 11995 1629719527020
promitor_ratelimit_arm{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",app_id="ceb249a3-44ce-4c90-8863-6776336f5b7e"} 11989 1629719532626
# HELP promitor_ratelimit_arm_throttled Indication concerning Azure Resource Manager are being throttled. (1 = yes, 0 = no).
# TYPE promitor_ratelimit_arm_throttled gauge
promitor_ratelimit_arm_throttled{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0329dd2a-59dc-4493-aa54-cb01cb027dc2",app_id="ceb249a3-44ce-4c90-8863-6776336f5b7e"} 0 1629719527020
promitor_ratelimit_arm_throttled{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",app_id="ceb249a3-44ce-4c90-8863-6776336f5b7e"} 0 1629719532626
```

You can read more about the Azure Resource Manager limitations on [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits).

### Azure Resource Graph

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-No-red.svg)

Promitor exposes runtime metrics to provide insights on the API consumption of
Azure Resource Graph:

- `promitor_ratelimit_resource_graph_remaining` - Indication how many calls are still available before
  Azure Resource Manager is going to throttle us. Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `cloud` - _Name of the cloud_
  - `auth_mode` - _Authentication mode to authenticate with_
  - `app_id` - _Id of the application that is being used to interact with_

- `promitor_ratelimit_resource_graph_throttled` - Indication whether or not we are being throttled by Azure Resource
 Graph. Metric provides following labels:
  - `tenant_id` - _Id of the tenant that is being interacted with_
  - `cloud` - _Name of the cloud_
  - `auth_mode` - _Authentication mode to authenticate with_
  - `app_id` - _Id of the application that is being used to interact with_

```text
# HELP promitor_ratelimit_resource_graph_remaining Indication how many calls are still available before Azure Resource Graph is going to throttle us.
# TYPE promitor_ratelimit_resource_graph_remaining gauge
promitor_ratelimit_resource_graph_remaining{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",cloud="Global",auth_mode="ServicePrincipal",app_id="67882a00-21d3-4ee7-b32a-430ea0768cd3"} 9 1629719863738
# HELP promitor_ratelimit_resource_graph_throttled Indication concerning Azure Resource Graph are being throttled. (1 = yes, 0 = no).
# TYPE promitor_ratelimit_resource_graph_throttled gauge
promitor_ratelimit_resource_graph_throttled{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",cloud="Global",auth_mode="ServicePrincipal",app_id="67882a00-21d3-4ee7-b32a-430ea0768cd3"} 0 1629719863738
```

You can read more about the Azure Resource Graph throttling on [docs.microsoft.com](https://docs.microsoft.com/en-us/azure/governance/resource-graph/overview#throttling).

### Azure Monitor

Promitor interacts with Azure Monitor API to scrape all the required metrics.

During troubleshooting it can be interesting to gain insights on what the API returns, for which you can opt-in.

You can opt-in for it by configuring the [runtime telemetry](/configuration/v2.x/runtime/scraper#azure-monitor).

[&larr; back](/)
