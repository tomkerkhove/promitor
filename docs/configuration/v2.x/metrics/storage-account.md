---
layout: default
title: Azure Storage Account Declaration
---

## Azure Storage Account

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Queue via the `StorageAccount` resource type.

When using declared resources, the following fields need to be provided:

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
resources: # Optional, required when no resource discovery is configured
- accountName: promitor-1
- accountName: promitor-2
resourceDiscoveryGroups:
- name: storage-accounts
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: storage-account-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
