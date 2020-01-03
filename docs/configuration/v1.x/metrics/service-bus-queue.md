---
layout: default
title: Azure Service Bus Queue Declaration
---

## Azure Service Bus Queue - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1-green.svg)

You can declare to scrape an Azure Service Bus Queue via the `ServiceBusQueue`
resource type.

The following fields need to be provided:

- `namespace` - The name of the Service Bus namespace
- `queueName` - The name of the queue

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftservicebusnamespaces).

The following scraper-specific metric label will be added:

- `entity_name` - Name of the queue

Notes:

- We currently do not support `EntityPath` as a dimension due to internal limitations.

Example:

```yaml
name: azure_service_bus_queue_active_messages
description: "The number of active messages on a service bus queue"
resourceType: ServiceBusQueue
azureMetricConfiguration:
  metricName: ActiveMessages
  aggregation:
    type: Total
resources:
- namespace: promitor-messaging
  queueName: orders
- namespace: promitor-messaging
  queueName: items
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
