---
layout: default
title: Metrics Declaration
---

All the Azure Monitor metrics that needs to be scraped are consolidated in one
YAML file. This configuration defines the Azure metadata and all the metrics.

## General Declaration

As Promitor evolves we need to change the structure of our metrics declaration.

`version: {version}` - **[REQUIRED]** Version of declaration that is used. Allowed
values are `v1`.

### Azure

- `azureMetadata.tenantId` - The id of the Azure tenant that will be queried.
- `azureMetadata.subscriptionId` - The id of the default subscription to query.
- `azureMetadata.resourceGroupName` - The name of the default resource group to query.

### Metric Defaults

- `metricDefaults.scraping.schedule` - **[REQUIRED]** A cron expression that controls
  the fequency in which all the configured metrics will be scraped from Azure Monitor.
  You can use [crontab-generator.org](https://crontab-generator.org/) to generate
  a cron that fits your needs.
- `metricDefaults.aggregation.interval` - The default interval which defines over
  what period measurements of a metric should be aggregated.

### Metrics

Every metric that is being declared needs to define the following fields:

- `name` - Name of the metric that will be exposed in the scrape endpoint for Prometheus.
- `description` - Description for the metric that will be exposed in the scrape
  endpoint for Prometheus.
- `resourceType` - Defines what type of resource needs to be queried.
- `labels` - Defines a set of custom labels to included for a given metric.
- `azureMetricConfiguration.metricName` - The name of the metric in Azure Monitor
  to query.
- `azureMetricConfiguration.aggregation.type` - The aggregation that needs to be
  used when querying Azure Monitor
- `azureMetricConfiguration.aggregation.interval` - Overrides the default
  aggregation interval defined in `metricDefaults.aggregation.interval` with a
  new interval.

Additionally, the following fields are optional:

- `resourceGroupName` - The resource group to scrape for this metric. This
  allows you to specify a different resource group from the one configured in
  `azureMetadata`, enabling you to scrape multiple resource groups with one
  single configuration.
- `scraping.schedule` - A scraping schedule for the individual metric; overrides
  the the one specified in `metricDefaults`.

### Example

Here is an example of how you can scrape two Azure Service Bus queues in different
resource groups, one in the `promitor` resource group and one on the `promitor-dev`
resource group:

```yaml
version: v1
azureMetadata:
  tenantId: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
  subscriptionId: yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy
  resourceGroupName: promitor
metricDefaults:
  aggregation:
    interval: 00:05:00
  scraping:
    # Every minute
    schedule: "0 * * ? * *"
metrics:
  - name: demo_queue_size
    description: "Amount of active messages of the 'myqueue' queue"
    resourceType: ServiceBusQueue
    namespace: promitor-messaging
    queueName: orders
    scraping:
      # Every 2 minutes
      schedule: "0 */2 * ? * *"
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
        interval: 00:15:00
  - name: demo_queue_dev_size
    description: "Amount of active messages of the 'myqueue-dev' queue"
    resourceType: ServiceBusQueue
    namespace: promitor-messaging-dev
    queueName: orders
    resourceGroupName: promitor-dev
    labels:
      app: promitor
      stage: dev
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
        interval: 00:15:00
```

## Supported Azure Services

[Generic Azure Resource](generic-azure-resource) allows you to scrape every Azure
service supported by Azure Monitor.

We also provide a simplified way to scrape the following Azure resources:

- [Azure Service Bus Queue](service-bus-queue)
- [Azure Storage Queue](storage-queue)

Want to help out? Create an issue and [contribute a new scraper](https://github.com/tomkerkhove/promitor/blob/master/adding-a-new-scraper.md).

[&larr; back](/)
