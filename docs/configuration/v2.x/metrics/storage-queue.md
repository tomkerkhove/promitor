---
layout: default
title: Azure Storage Queue Declaration
---

## Azure Storage Queue

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)

You can declare to scrape an Azure Queue via the `StorageQueue` resource type.

When using declared resources, the following fields need to be provided:

- `accountName` - The name of the storage account
- `queueName` - The name of the queue
- `sasToken` - The SAS token used to access the queue/account
  - `sasToken.environmentVariable` - Defines the environment variable which contains
    the SAS token to authenticate with
  - `sasToken.rawValue` - Contains the raw hardcoded SAS token _(less secure)_

Supported metrics:

- `TimeSpentInQueue` - Time in seconds that the oldest message has been waiting
  in the queue to be processed.
- `MessageCount`

The following scraper-specific metric label will be added:

- `queue_name` - Name of the queue

Example:

```yaml
name: azure_storage_queue_message_count
description: "The number of messages on an Azure storage queue"
resourceType: StorageQueue
azureMetricConfiguration:
  metricName: MessageCount
  aggregation:
    type: Total
resources:
- accountName: promitor
  queueName: orders
  sasToken:
    environmentVariable: "SECRETS_STORAGEQUEUE_PROMITOR_SASTOKEN"
- accountName: promitor
  queueName: items
  sasToken:
    environmentVariable: "SECRETS_STORAGEQUEUE_PROMITOR_SASTOKEN"
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
