---
layout: default
title: Azure Virtual Machine Scale Set (VMSS) Declaration
---

## Azure Virtual Machine Scale Set (VMSS)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.2-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Virtual Machine Scale Set via the `VirtualMachineScaleSet` resource
type.

The following fields need to be provided:

- `scaleSetName` - The name of the Virtual Machine Scale Set

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcomputevirtualmachinescalesets).

Example:

```yaml
name: azure_virtual_machine_scale_set_percentage_cpu
description: "Average percentage cpu usage on an Azure virtual machine scale set"
resourceType: VirtualMachineScaleSet
azureMetricConfiguration:
  metricName: Percentage CPU
  dimension:
    name: VMName
  aggregation:
    type: Average
resources:
- scaleSetName: promitor-virtual-machine-scale-set-1
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: virtual-machine-scale-sets-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
