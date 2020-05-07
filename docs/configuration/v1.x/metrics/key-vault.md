---
layout: default
title: Azure Key Vault Declaration
---

## Azure Key Vault - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.6-green.svg)

You can declare to scrape an Azure Key Vault
via the `KeyVault` resource type.

The following fields need to be provided:

- `vaultName` - The name of the Azure Key Vault

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftkeyvaultvaults).

Example:

```yaml
name: azure_key_vault_api_latency
description: "The overall latency of service api requests"
resourceType: KeyVault
azureMetricConfiguration:
  metricName: ServiceApiLatency
  aggregation:
    type: Average
resources:
- vaultName: promitor-1
- vaultName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
