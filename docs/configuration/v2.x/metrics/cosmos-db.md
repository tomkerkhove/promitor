---
layout: default
title: Azure Cosmos Db Declaration
---

## Azure Cosmos Db

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape Cosmos Db via the `CosmosDb` resource type.

When using declared resources, the following fields need to be provided:

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
resources: # Optional, required when no resource discovery is configured
- dbName: cosmos-database-1
- dbName: cosmos-database-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: cosmos-db-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
