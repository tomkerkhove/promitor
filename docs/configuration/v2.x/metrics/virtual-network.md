---
layout: default
title: Azure Virtual Network Declaration
---

## Azure Virtual Network

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.6-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Virtual Network via the `VirtualNetwork` resource
type.

When using declared resources, the following fields need to be provided:

- `virtualNetworkName` - The name of the Azure Virtual Network resource

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftnetworkvirtualnetworks).

Example:

```yaml
name: azure_virtual_network_ddos_attack
description: "Indication whether or not there is a DDOS attack on the Azure Virtual Network"
resourceType: VirtualNetwork
azureMetricConfiguration:
  metricName: IfUnderDDoSAttack
  aggregation:
    type: Maximum
resources: # Optional, required when no resource discovery is configured
- virtualNetworkName: promitor-virtual-network-1
- virtualNetworkName: promitor-virtual-network-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: virtual-network-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
