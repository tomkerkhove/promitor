---
layout: default
title: Azure Storage Account Declaration
---

## Azure Storage Account - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)

You can declare to scrape an Azure Queue via the `StorageAccount` resource type.

The following fields need to be provided:

- `accountName` - The name of the Azure Storage account

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftstoragestorageaccounts).

Example:

```yaml
name: azure_storage_account_capacity
description: "The average capacity used in the storage account"
resourceType: StorageAccount
azureMetricConfiguration:
  metricName: UsedCapacity
  aggregation:
    type: Average
resources:
- accountName: promitor-1
- accountName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
