---
layout: default
title: Azure Synapse (SQL pool) Declaration
---

## Azure Synapse (SQL pool)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure Synapse SQL pool via the `SynapseSqlPool` resource type.

The following fields need to be provided:

- `workspaceName` - The name of the Azure Synapse workspace.
- `poolName` - The name of the SQL pool.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsynapseworkspacessqlpools).

The following scraper-specific metric labels will be added:

- `workspace_name` - The name of the Azure Synapse workspace.
- `pool_name` - The name of the SQL pool.

Example:

```yaml
- name: promitor_demo_synapse_sql_pool_dwu_limit
  description: "Amount of DWUs defined as limit for SQL pool in Azure Synapse"
  resourceType: SynapseSqlPool
  azureMetricConfiguration:
    metricName: DWULimit
    aggregation:
      type: Maximum
  resources:
  - workspaceName: promitor-synapse
    poolName: sqlpool
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: synapse-sql-pools
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
