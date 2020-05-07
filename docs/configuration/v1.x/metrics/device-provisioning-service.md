---
layout: default
title: Azure Device Provisioning Service Declaration
---

## Azure Device Provisioning Service - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.6-green.svg)

You can declare to scrape an Azure Device Provisioning Service via the `DeviceProvisioningService` resource type.

The following fields need to be provided:

- `deviceProvisioningServiceName` - The name of the Azure Device Provisioning Service

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdevicesprovisioningservices).

Example:

```yaml
name: azure_dps_attestation_attempts
description: "The number of device attestations attempted"
resourceType: DeviceProvisioningService
azureMetricConfiguration:
  metricName: AttestationAttempts
  aggregation:
    type: Total
resources:
- deviceProvisioningServiceName: promitor-1
- deviceProvisioningServiceName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
