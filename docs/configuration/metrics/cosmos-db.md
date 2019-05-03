---
layout: default
title: Azure Cosmos Db Declaration
---

## Azure Cosmos Db - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)
You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

The following fields need to be provided:
- `dbName`- The name of the Cosmos Db to be scraped

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdocumentdbdatabaseaccounts).

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