# Integrations

## Azure Resource Manager API - Consumption & Throttling

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

## Azure Resource Graph

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

## Azure Monitor

Promitor interacts with Azure Monitor API to scrape all the required metrics.

During troubleshooting it can be interesting to gain insights on what the API returns, for which you can opt-in.

You can opt-in for it by configuring the [runtime telemetry](/configuration/v2.x/runtime/scraper#azure-monitor).

[&larr; back](/)
