---
layout: default
title: Azure SQL Database Declaration
---

## Azure SQL Database - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1.0-green.svg)

You can scrape an Azure SQL Database via the `SqlDatabase` resource type.

The following fields need to be provided:

- `serverName` - The name of the SQL Server instance.
- `databaseName` - The name of the database.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsqlserversdatabases).

The following scraper-specific metric labels will be added:

- `server` - The name of the SQL Server instance.
- `database` - The name of the database.

Example:

```yaml
name: azure_sql_database_dtu_consumption_percent
description: "The DTU consumption percentage used by an Azure SQL Database."
resourceType: SqlDatabase
azureMetricConfiguration:
  metricName: dtu_consumption_percent
  aggregation:
    type: Average
resources:
- serverName: promitor-sql-server
  databaseName: promitor-db
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
