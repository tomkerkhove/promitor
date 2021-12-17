---
layout: default
title: Azure Load Balancer Declaration
---

## Azure Load Balancer

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.6-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Load Balancer via the `LoadBalancer` resource
type.

When using declared resources, the following fields need to be provided:

- `loadBalancerName` - The name of the Azure Load Balancer resource

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftnetworkloadbalancers).

Example:

```yaml
name: azure_load_balancer_traffic_bytes
description: "Average amount of bytes sent through an Azure Load Balancer"
resourceType: LoadBalancer
azureMetricConfiguration:
  metricName: ByteCount
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- loadBalancerName: promitor-load-balancer-1
- loadBalancerName: promitor-load-balancer-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: load-balancer-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
