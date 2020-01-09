---
layout: default
title: Metrics Declaration
---

All the Azure Monitor metrics that need to be scraped are consolidated in one YAML
file. This configuration defines the Azure metadata and all the metrics.

## General Declaration

As Promitor evolves we need to change the structure of our metrics declaration.

`version: {version}` - **[REQUIRED]** Version of declaration that is used. Allowed
values are `v1`.

### Azure

- `azureMetadata.tenantId` - The id of the Azure tenant that will be queried.
- `azureMetadata.subscriptionId` - The id of the default subscription to query.
- `azureMetadata.resourceGroupName` - The name of the default resource group to query.
- `azureMetadata.cloud` - The name of the Azure cloud to use. Options are `Global`
 (default), `China`, `UsGov` & `Germany`.

### Metric Defaults

- `metricDefaults.scraping.schedule` - **[REQUIRED]** A cron expression that controls
  the frequency of which all the configured metrics will be scraped from Azure Monitor.
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
- `labels` - Defines a set of custom labels to include for a given metric.
- `azureMetricConfiguration.metricName` - The name of the metric in Azure Monitor
  to query
- `azureMetricConfiguration.aggregation.type` - The aggregation that needs to be
  used when querying Azure Monitor
- `azureMetricConfiguration.aggregation.interval` - Overrides the default aggregation
  interval defined in `metricDefaults.aggregation.interval` with a new interval
- `resources` - An array of one or more resources to get metrics for. The fields
  required vary depending on the `resourceType` being created, and are documented
  for each resource. All resources support an optional `resourceGroupName` to allow
  the global resource group to be overridden.

Additionally, the following fields are optional:

- `azureMetricConfiguration.dimension.Name` - The name of the dimension that should
   be used to scrape a multi-dimensional metric in Azure Monitor.
  - ☝ *Promitor simply acts as a proxy and will not validate if it's supported or
     not, we recommend verifying that the dimension is supported in the
     [official documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported)*
- `scraping.schedule` - A scraping schedule for the individual metric; overrides
  the the one specified in `metricDefaults`

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
  cloud: China
metricDefaults:
  aggregation:
    interval: 00:05:00
  scraping:
    # Every minute
    schedule: "0 * * ? * *"
metrics:
  - name: azure_service_bus_active_messages
    description: "The number of active messages on a service bus queue."
    resourceType: ServiceBusQueue
    labels:
      app: promitor
      tier: messaging
    scraping:
      # Every 2 minutes
      schedule: "0 */2 * ? * *"
    azureMetricConfiguration:
      metricName: ActiveMessages
      dimension:
        name: <dimension-name>
      aggregation:
        type: Total
        interval: 00:15:00
    resources:
      - namespace: promitor-messaging
        queueName: orders
      - namespace: promitor-messaging-dev
        queueName: orders
        resourceGroupName: promitor-dev
```

## Supported Azure Services

[Generic Azure Resource](generic-azure-resource) allows you to scrape every Azure
service supported by Azure Monitor.

We also provide a simplified way to scrape the following Azure resources:

- [Azure App Plan](app-plan)
- [Azure Cache for Redis](redis-cache)
- [Azure Container Instances](container-instances)
- [Azure Container Registry](container-registry)
- [Azure Cosmos DB](cosmos-db)
- [Azure Database for PostgreSQL](postgresql)
- [Azure Function App](function-app)
- [Azure Network Interface](network-interface)
- [Azure Service Bus Queue](service-bus-queue)
- [Azure SQL Database](sql-database)
- [Azure SQL Managed Instance](sql-managed-instance)
- [Azure Storage Queue](storage-queue)
- [Azure Virtual Machine](virtual-machine)
- [Azure Virtual Machine Scale Set (VMSS)](virtual-machine-scale-set)
- [Azure Web App](web-app)

Want to help out? Create an issue and [contribute a new scraper](https://github.com/tomkerkhove/promitor/blob/master/adding-a-new-scraper.md).

[&larr; back](/)
