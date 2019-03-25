---
layout: default
title: Azure Container Instances Declaration
---

## Azure Container Instances
You can declare to scrape an Azure Container Instances via the `ContainerInstances` resource type.

The following fields need to be provided:
- `name` - The name of the Azure Container Instance resource

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcontainerinstancecontainergroups).

Example:
```yaml
name: demo_containerinstances_cpu
description: "Average cpu usage of our 'promitor-container-instance' container instance"
resourceType: ContainerInstances
name: promitor-container-instance
azureMetricConfiguration:
  metricName: CpuUsage
  aggregation:
    type: Average
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)