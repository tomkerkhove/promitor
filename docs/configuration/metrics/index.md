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
metrics: 
  - name: demo_queue_size
    description: "Amount of active messages of the 'myqueue' queue"
    resourceType: ServiceBusQueue
    namespace: promitor-messaging
    queueName: orders
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation: Total
```

# General Declaration
## Azure

- `azureMetadata.tenantId` - The id of the Azure tenant that will be queried.
- `azureMetadata.subscriptionId` - The id of the default subscription to query.
- `azureMetadata.resourceGroupName` - The name of the default resource group to query.

## Metrics
Every metric that is being declared needs to define the following fields:
- `name` - Name of the metric that will be exposed in the scrape endpoint for Prometheus
- `description` - Description for the metric that will be exposed in the scrape endpoint for Prometheus
- `resourceType` - Defines what type of resource needs to be queried.
- `azureMetricConfiguration.metricName` - The name of the metric in Azure Monitor to query
- `azureMetricConfiguration.aggregation` - The aggregation that needs to be used when querying Azure Monitor

# Supported Azure Services

- [Azure Service Bus Queue](service-bus-queue)

Can't find the Azure service that you want? You can still use the [Generic Azure Resource](generic-azure-resource).

[&larr; back](/)
