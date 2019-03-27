---
layout: default
<<<<<<< HEAD
<<<<<<< HEAD
title: Cosmos Db Declaration
---

## Cosmos Db
You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

The following fields need to be provided:
- `dbName`- The name of the Cosmos Db to be scraped
=======
title: Azure Container Instances Declaration
=======
title: Cosmos Db Declaration
>>>>>>> e7b8c45... Cosmos DB Implementation of Scraper
---

## Cosmos Db
You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

The following fields need to be provided:
<<<<<<< HEAD
- `containerGroup` - The name of the container group
>>>>>>> 94af341... Implementation of Cosmos DB scraper with TotalRequests metric
=======
- `dbName`- The name of the Cosmos Db to be scraped
>>>>>>> e7b8c45... Cosmos DB Implementation of Scraper

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcontainerinstancecontainergroups).

Example:
```yaml
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> e7b8c45... Cosmos DB Implementation of Scraper
	name: demo_cosmos_totalrequests
    description: "Demo cosmos query"
    resourceType: CosmosDb
    dbName: cognitiveanalytics
    azureMetricConfiguration:
      metricName: TotalRequests
      aggregation:
        type: Count
<<<<<<< HEAD
=======
name: demo_containerinstances_cpu
description: "Average cpu usage of our 'promitor-container-instance' container instance"
resourceType: ContainerInstance
containerGroup: promitor-container-instance
azureMetricConfiguration:
  metricName: CpuUsage
  aggregation:
    type: Average
>>>>>>> 94af341... Implementation of Cosmos DB scraper with TotalRequests metric
=======
>>>>>>> e7b8c45... Cosmos DB Implementation of Scraper
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)