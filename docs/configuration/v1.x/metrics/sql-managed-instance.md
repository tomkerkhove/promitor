---
layout: default
title: Azure SQL Managed Instance Declaration
---

## Azure SQL Managed Instance - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1-green.svg)

You can scrape an Azure SQL Managed Instance via the `SqlManagedInstance`
 resource type.

The following fields need to be provided:

- `instanceName` - The name of the SQL Server instance.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsqlmanagedinstances).

Example:

```yaml
name: promitor_demo_azuresqlmanagedinstance_nodes
description: "The amount of nodes for an Azure SQL Managed Instance."
resourceType: SqlManagedInstance
azureMetricConfiguration:
  metricName: virtual_core_count
  aggregation:
    type: Average
resources:
- instanceName: promitor-sql-managed-instance
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
