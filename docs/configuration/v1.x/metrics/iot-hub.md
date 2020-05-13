---
layout: default
title: Azure IoT Hub Declaration
---

## Azure IoT Hub - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.6-green.svg)

You can declare to scrape an Azure IoT Hub via the `IoTHub` resource type.

The following fields need to be provided:

- `ioTHubName` - The name of the Azure IoT Hub

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdevicesiothubs).

Example:

```yaml
name: azure_iot_hub_total_devices
description: "The number of devices registered to your IoT hub"
resourceType: IoTHub
azureMetricConfiguration:
  metricName: devices.totalDevices
  aggregation:
    type: Total
resources:
- ioTHubName: promitor-1
- ioTHubName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
