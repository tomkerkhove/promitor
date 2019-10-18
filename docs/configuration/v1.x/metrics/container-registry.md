---
layout: default
title: Azure Container Registry Declaration
---

## Azure Container Registry - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)

You can declare to scrape an Azure Container Registry via the `ContainerRegistry`
resource type.

The following fields need to be provided:

- `registryName` - The name of the registry

All supported metrics are not documented in the official
[Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported)
documentation yet, but it's being added via [this PR](https://github.com/MicrosoftDocs/azure-docs/pull/27991).

Example:

```yaml
name: azure_container_registry_total_pull_count
description: "Amount of images that were pulled from the container registry"
resourceType: ContainerRegistry
azureMetricConfiguration:
  metricName: TotalPullCount
  aggregation:
    type: Average
resources:
- registryName: promitor-1
- registryName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
