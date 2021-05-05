---
layout: default
title: Azure File Storage Declaration
---

## Azure File Storage

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)

You can declare to scrape an Azure Queue via the `FileStorage` resource type.

The following fields need to be provided:

- `accountName` - The name of the Azure Storage account

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftstoragestorageaccountsfileservices).

Example:

```yaml
name: azure_storage_files_capacity
description: "The average capacity used by files in the storage account"
resourceType: FileStorage
azureMetricConfiguration:
  metricName: FileCapacity
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- accountName: promitor-1
- accountName: promitor-2
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
