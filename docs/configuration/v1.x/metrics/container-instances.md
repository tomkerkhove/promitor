---
layout: default
title: Azure Container Instances Declaration
---

## Azure Container Instances - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)

You can declare to scrape an Azure Container Instances via the `ContainerInstance`
resource type.

The following fields need to be provided:

- `containerGroup` - The name of the container group

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcontainerinstancecontainergroups).

Example:

```yaml
name: azure_container_instance_cpu_usage
description: "Average cpu usage of our 'promitor-container-instance' container instance"
resourceType: ContainerInstance
azureMetricConfiguration:
  metricName: CpuUsage
  aggregation:
    type: Average
resources:
- containerGroup: promitor-container-instance-1
- containerGroup: promitor-container-instance-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
