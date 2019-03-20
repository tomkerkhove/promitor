---
layout: default
title: Azure Storage Queue Declaration
---

## Azure Storage Queue
You can declare to scrape an Azure Queue via the `StorageQueue` resource type.

The following fields need to be provided:
- `accountName` - The name of the storage account
- `queueName` - The name of the queue
- `sasToken` - The SAS token used to access the queue/account

Supported metrics:
- MessageCount

Example:
```yaml
name: demo_queue_size
description: "Amount of messages in the 'orders' queue"
resourceType: StorageQueue
accountName: promitor
queueName: orders
azureMetricConfiguration:
  metricName: MessageCount
  aggregation: Total
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)
