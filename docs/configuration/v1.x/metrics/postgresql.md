---
layout: default
title: Azure Database for PostgreSQL
---

## Azure Database for PostgreSQL - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)
You can declare to scrape an Azure Database for PostgreSQL server via the `PostgreSql` resource type.

The following fields need to be provided:
- `serverName` - The name of the PostgreSQL server

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdbforpostgresqlservers).

Example:
```yaml
name: azure_postgre_sql_cpu_percent
description: "The CPU percentage on the server"
resourceType: PostgreSql
scraping:
  schedule: "0 */2 * ? * *"
azureMetricConfiguration:
  metricName: cpu_percent
  aggregation:
    type: Average
    interval: 00:01:00
resources:
- serverName: Promitor-1
- serverName: Promitor-2
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)