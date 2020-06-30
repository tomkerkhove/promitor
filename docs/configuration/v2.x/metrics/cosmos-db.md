---
layout: default
title: Azure Cosmos Db Declaration
---

## Azure Cosmos Db

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)

You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

The following fields need to be provided:

- `dbName`- The name of the Cosmos Db to be scraped

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdocumentdbdatabaseaccounts).

Example:

```yaml
name: azure_cosmos_db_total_requests
description: "Demo cosmos query"
resourceType: CosmosDb
azureMetricConfiguration:
  metricName: TotalRequests
  aggregation:
    type: Count
resources:
- dbName: cognitiveanalytics-1
- dbName: cognitiveanalytics-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
