---
layout: default
title: Azure Network Gateway Declaration
---

## Azure Network Gateway

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Network Gateway via the `NetworkGateway` resource
type.

When using declared resources, the following fields need to be provided:

- `networkGatewayName` - The name of the Azure Network Gateway

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworkvirtualnetworkgateways).

Example:

```yaml
name: azure_network_gateway_packages
description: "Average packages on an Azure network gateway"
resourceType: NetworkGateway
azureMetricConfiguration:
  metricName: ExpressRouteGatewayPacketsPerSecond
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- networkGatewayName: promitor-network-gateway-1
- networkGatewayName: promitor-network-gateway-2
resourceDiscoveryGroups:
- name: network-gateway-group
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: network-gateway-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
