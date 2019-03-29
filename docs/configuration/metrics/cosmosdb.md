---
layout: default
title: Cosmos Db Declaration
---

## Cosmos Db
You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

The following fields need to be provided:
- `dbName`- The name of the Cosmos Db to be scraped

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcontainerinstancecontainergroups).

Example:
```yaml
    name: demo_cosmos_totalrequests
    description: "Demo cosmos query"
    resourceType: CosmosDb
    dbName: cognitiveanalytics
    azureMetricConfiguration:
      metricName: TotalRequests
      aggregation:
        type: Count
```

[&larr; back to metrics declarations](/configuration/metrics)
[&larr; back to introduction](/)