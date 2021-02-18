---
layout: default
title: Azure Synapse (Apache Spark pool) Declaration
---

## Azure Synapse (Apache Spark pool)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure Synapse Apache Spark pool via the `SynapseApacheSparkPool` resource type.

The following fields need to be provided:

- `workspaceName` - The name of the Azure Synapse workspace.
- `poolName` - The name of the Apache Spark pool.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftsynapseworkspacesbigdatapools).

The following scraper-specific metric labels will be added:

- `workspace_name` - The name of the Azure Synapse workspace.
- `pool_name` - The name of the Apache Spark pool.

Example:

```yaml
- name: promitor_demo_synapse_apache_spark_apps_ended
  description: "Amount of apps ended running on Apache Spark pool in Azure Synapse"
  resourceType: SynapseApacheSparkPool
  azureMetricConfiguration:
    metricName: BigDataPoolApplicationsEnded
    aggregation:
      type: Total
  resources:
  - workspaceName: promitor-synapse
    poolName: sparkpool
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: synapse-apache-spark-pools
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
