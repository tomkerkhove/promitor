---
layout: default
title: Azure Network Interface Declaration
---

## Azure Network Interface
You can declare to scrape an Azure Network Interface via the `NetworkInterface` resource type.

The following fields need to be provided:
- `networkInterfaceName` - The name of the network interface

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworknetworkinterfaces).

Example:
```yaml
  - name: demo_azuresnetworkinterface_bytesreceivedrate
    description: "Number of bytes the Network Interface sent"
    resourceType: NetworkInterface
    networkInterfaceName: promitor-network-interface
    azureMetricConfiguration:
      metricName: BytesReceivedRate
      aggregation:
        type: Average
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)