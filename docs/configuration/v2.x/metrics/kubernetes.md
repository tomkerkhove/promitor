---
layout: default
title: Azure Kubernetes Service Declaration
---

## Azure Kubernetes Service

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Kubernetes Service (AKS)
via the `KubernetesService` resource type.

When using declared resources, the following fields need to be provided:

- `clusterName` - The name of the Azure Kubernetes Service

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcontainerservicemanagedclusters).

Example:

```yaml
name: azure_kubernetes_available_cpu_cores
description: "Available CPU cores in cluster"
resourceType: KubernetesService
azureMetricConfiguration:
  metricName: kube_node_status_allocatable_cpu_cores
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- clusterName: promitor-aks
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: kubernetes-service-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
