---
layout: default
title: Azure Storage Queue Declaration
---

## Azure Storage Queue - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)
You can declare to scrape an Azure Queue via the `StorageQueue` resource type.

The following fields need to be provided:
- `accountName` - The name of the storage account
- `queueName` - The name of the queue
- `sasToken` - The SAS token used to access the queue/account
  - `sasToken.environmentVariable` - Defines the environment variable which contains the SAS token to authenticate with
  - `sasToken.rawValue` - Contains the raw hardcoded SAS token _(less secure)_

Supported metrics:
- `TimeSpentInQueue` - Time in seconds that the oldest message has been waiting in the queue to be processed.
- `MessageCount`

Example:
```yaml
name: demo_queue_size
description: "Amount of messages in the 'orders' queue"
resourceType: StorageQueue
accountName: promitor
queueName: orders
sasToken:
  environmentVariable: "SECRETS_STORAGEQUEUE_PROMITOR_SASTOKEN"
azureMetricConfiguration:
  metricName: MessageCount
  aggregation:
    type: Total
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)
