---
layout: default
title: Azure Container Registry Declaration
---

## Azure Container Registry
You can declare to scrape an Azure Container Registry via the `ContainerRegistry` resource type.

The following fields need to be provided:
- `containerGroup` - The name of the container group

All supported metrics are not documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported) documentation yet, but it's being added via [this PR](https://github.com/MicrosoftDocs/azure-docs/pull/27991).

Example:
```yaml
name: demo_container_registry_total_pull_count
description: "Amount of images that were pulled from the container registry"
resourceType: ContainerRegistry
registryName: promitor
azureMetricConfiguration:
  metricName: TotalPullCount
  aggregation:
    type: Average
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)