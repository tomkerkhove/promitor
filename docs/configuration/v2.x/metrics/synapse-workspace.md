---
layout: default
title: Azure Synapse (Workspace) Declaration
---

## Azure Synapse (Workspace)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure Synapse workspace via the `SynapseWorkspace` resource type.

The following fields need to be provided:

- `workspaceName` - The name of the Azure Synapse workspace.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsynapseworkspaces).

The following scraper-specific metric labels will be added:

- `workspace_name` - The name of the Azure Synapse workspace.

Example:

```yaml
- name: promitor_demo_synapse_workspace_builtin_sql_processed_bytes
  description: "Amount of bytes processed in Azure Synapse workspace"
  resourceType: SynapseWorkspace
  azureMetricConfiguration:
    metricName: BuiltinSqlPoolDataProcessedBytes
    aggregation:
      type: Total
  resources:
  - workspaceName: promitor-synapse
    resourceGroupName: promitor-sources
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: synapse-apache-spark-pools
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
