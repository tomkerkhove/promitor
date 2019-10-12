---
layout: default
title: Azure Service Bus Queue Declaration
---

## Azure Service Bus Queue - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1.0-green.svg)

You can declare to scrape an Azure Service Bus Queue via the `ServiceBusQueue`
resource type.

The following fields need to be provided:

- `namespace` - The name of the Service Bus namespace
- `queueName` - The name of the queue

Supported metrics:

- `IncomingMessages`
- `IncomingRequests`
- `ActiveMessages`
- `Messages`
- `Size`

Example:

```yaml
name: demo_queue_size
description: "Amount of active messages of the 'myqueue' queue"
resourceType: ServiceBusQueue
namespace: promitor-messaging
queueName: orders
azureMetricConfiguration:
  metricName: ActiveMessages
  aggregation:
    type: Total
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
