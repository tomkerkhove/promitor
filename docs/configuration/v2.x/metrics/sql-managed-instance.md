---
layout: default
title: Azure SQL Managed Instance Declaration
---

## Azure SQL Managed Instance

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure SQL Managed Instance via the `SqlManagedInstance`
 resource type.

When using declared resources, the following fields need to be provided:

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
resources: # Optional, required when no resource discovery is configured
- instanceName: promitor-sql-managed-instance
resourceDiscoveryGroups:
- name: sql-managed-instances
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: sql-managed-instances-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
