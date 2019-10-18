---
layout: default
title: Azure Virtual Machine Declaration
---

## Azure Virtual Machine - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)

You can declare to scrape an Azure Virtual Machine via the `VirtualMachine` resource
type.

The following fields need to be provided:

- `virtualMachineName` - The name of the virtual machine

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcomputevirtualmachines).

Example:

```yaml
name: azure_virtual_machine_percentage_cpu
description: "Average percentage cpu usage on an Azure virtual machine"
resourceType: VirtualMachine
azureMetricConfiguration:
  metricName: Percentage CPU
  aggregation:
    type: Average
resources:
- virtualMachineName: promitor-virtual-machine-1
- virtualMachineName: promitor-virtual-machine-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
