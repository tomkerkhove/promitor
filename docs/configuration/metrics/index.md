---
layout: default
title: Metrics Declaration
---

All the Azure Monitor metrics that needs to be scraped are consolidated in one YAML file.
This configuration defines the Azure metadata and all the metrics.

Here is an example of how you can scrape an Azure Service Bus queue:

```yaml
azureMetadata:
  tenantId: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
  subscriptionId: yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy
  resourceGroupName: promitor
metricDefaults:
  aggregation:
    interval: 00:05:00
metrics: 
  - name: demo_queue_size
    description: "Amount of active messages of the 'myqueue' queue"
    resourceType: ServiceBusQueue
    namespace: promitor-messaging
    queueName: orders
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
        interval: 00:15:00
```

# General Declaration
## Azure

- `azureMetadata.tenantId` - The id of the Azure tenant that will be queried.
- `azureMetadata.subscriptionId` - The id of the default subscription to query.
- `azureMetadata.resourceGroupName` - The name of the default resource group to query.

## Metric Defaults

- `metricDefaults.aggregation.interval` - The default interval which defines over what period measurements of a metric should be aggregated.

## Metrics
Every metric that is being declared needs to define the following fields:
- `name` - Name of the metric that will be exposed in the scrape endpoint for Prometheus
- `description` - Description for the metric that will be exposed in the scrape endpoint for Prometheus
- `resourceType` - Defines what type of resource needs to be queried.
- `azureMetricConfiguration.metricName` - The name of the metric in Azure Monitor to query
- `azureMetricConfiguration.aggregation.type` - The aggregation that needs to be used when querying Azure Monitor
- `azureMetricConfiguration.aggregation.interval` - Overrides the default aggregation interval defined in `metricDefaults.aggregation.interval` with a new interval

# Supported Azure Services
Every Azure service is supported and can be scraped by using the [Generic Azure Resource](generic-azure-resource).

We also provide a simplified way to configure the following Azure resources:
- [Azure Container Instances](container-instances)
- [Azure Service Bus Queue](service-bus-queue)
- [Azure Storage Queue](storage-queue)

Want to help out? Create an issue and [contribute a new scraper](https://github.com/tomkerkhove/promitor/blob/master/adding-a-new-scraper.md).

[&larr; back](/)
