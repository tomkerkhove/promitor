---
layout: default
title: Azure Storage Queue Declaration
---

## Azure Queue
You can declare to scrape an Azure Queue via the `AzureQueue` resource type.

The following fields need to be provided:
- `accountName` - The name of the storage account
- `queueName` - The name of the queue
- `sasToken` - The SAS token used to access the queue/account

Supported metrics:
- Size
- Duration

Duration is the time in seconds of a recent message in the given queue

Example:
```yaml
name: demo_queue_size
description: "Amount of messages in the 'orders' queue"
resourceType: AzureStorageQueue
accountName: promitor
queueName: orders
azureMetricConfiguration:
  metricName: Size
  aggregation: Total
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)
