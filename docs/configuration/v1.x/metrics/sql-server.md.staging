---
layout: default
title: Azure SQL Server Declaration
---

## Azure SQL Server - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)

You can scrape an Azure SQL Server via the `SqlServer` resource type.

The following fields need to be provided:

- `serverName` - The name of the SQL Server instance.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsqlservers).

Example:

```yaml
name: azure_sql_server_dtu_consumption_percent
description: "The DTU consumption percentage used by an Azure SQL Server."
resourceType: SqlServer
azureMetricConfiguration:
  metricName: dtu_consumption_percent
  aggregation:
    type: Average
resources:
- serverName: promitor-sql-server
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
