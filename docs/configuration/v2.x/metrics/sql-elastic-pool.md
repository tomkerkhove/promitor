---
layout: default
title: Azure SQL Elastic Pool Declaration
---

## Azure SQL Elastic Pool

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure SQL Elastic Pool via the `SqlElasticPool` resource type.

When using declared resources, the following fields need to be provided:

- `serverName` - The name of the SQL Server instance.
- `poolName` - The name of the elastic pool.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsqlserverselasticpools).

The following scraper-specific metric labels will be added:

- `server` - The name of the SQL Server instance.
- `elastic_pool` - The name of the elastic pool.

Example:

```yaml
- name: promitor_demo_sql_elastic_pool_cpu
  description: "CPU percentage used for a Azure SQL Elastic Pool"
  resourceType: SqlElasticPool
  labels:
    app: promitor
  azureMetricConfiguration:
    metricName: cpu_percent
    aggregation:
      type: Average
  resources: # Optional, required when no resource discovery is configured
  - serverName: promitor-sql-server
    poolName: promitor-db
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: sql-elastic-pools
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
