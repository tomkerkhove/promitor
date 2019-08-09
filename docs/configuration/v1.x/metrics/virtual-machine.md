---
layout: default
title: Azure Virtual Machine Declaration
---

## Azure Virtual Machine - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)
You can declare to scrape an Azure Virtual Machine via the `VirtualMachine` resource type.

The following fields need to be provided:
- `virtualMachineName` - The name of the virtual machine

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcomputevirtualmachines).

Example:
```yaml
name: demo_virtualmachine_percentage_cpu
description: "Average percentage cpu usage of our 'promitor-virtual-machine' virtual machine"
resourceType: VirtualMachine
virtualMachineName: promitor-virtual-machine
azureMetricConfiguration:
  metricName: Percentage CPU
  aggregation:
    type: Average
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)