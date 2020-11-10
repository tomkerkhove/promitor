---
layout: default
title: Azure Service Bus Queue Declaration
---

## Azure Service Bus Queue

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Service Bus Queue via the `ServiceBusQueue`
resource type.

The following fields need to be provided:

- `namespace` - The name of the Service Bus namespace
- `queueName` - The name of the queue (optional)

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftservicebusnamespaces).

The following scraper-specific metric label will be added:

- `entity_name` - Name of the queue

> :warning: **As of today, `EntityPath` as a dimension is not supported.**

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
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: service-bus-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
