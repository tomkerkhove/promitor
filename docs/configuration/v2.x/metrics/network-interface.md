---
layout: default
title: Azure Network Interface Declaration
---

## Azure Network Interface

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an [Azure Network Interface](https://docs.microsoft.com/en-us/azure/virtual-network/virtual-network-network-interface)
via the `NetworkInterface` resource type.

When using declared resources, the following fields need to be provided:

- `networkInterfaceName` - The name of the network interface

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworknetworkinterfaces).

Example:

```yaml
name: azure_network_interface_bytes_received_rate
description: "Number of bytes the Network Interface sent"
resourceType: NetworkInterface
azureMetricConfiguration:
  metricName: BytesReceivedRate
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- networkInterfaceName: promitor-network-interface-1
- networkInterfaceName: promitor-network-interface-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: network-interfaces-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
