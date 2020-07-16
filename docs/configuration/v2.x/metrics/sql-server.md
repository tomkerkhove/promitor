---
layout: default
title: Azure SQL Server Declaration
---

## Azure SQL Server

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)

You can scrape an Azure SQL Server via the `SqlServer` resource type.

The following fields need to be provided:

- `serverName` - The name of the SQL Server instance.

Supported metrics:

- `dtu_consumption_percent` - Percentage of consumed CPU across all elastic pools.
  - *Requires `dimension.name` to be set to `ElasticPoolResourceId`*
- `storage_used` - Amount of storage data across all elastic pools in bytes.
  - *Requires `dimension.name` to be set to `ElasticPoolResourceId`*
- `dtu_used` - Amount of consumed DTU across all databases.
  - *Requires `dimension.name` to be set to `DatabaseResourceId`*

> The official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsqlservers)
> lists more metrics but these are not surfaced externally. However, you can still give them a try but we don't
> support them for now.

Example:

```yaml
name: azure_sql_server_dtu_consumption_percent
description: "The DTU consumption percentage used by an Azure SQL Server."
resourceType: SqlServer
azureMetricConfiguration:
  metricName: dtu_used
  dimension:
    name: DatabaseResourceId
  aggregation:
    type: Average
resources:
- serverName: promitor
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
