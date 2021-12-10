---
layout: default
title: Azure Event Hubs Declaration
---

## Azure Event Hubs

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Event Hubs Queue via the `EventHubs`
resource type.

When using declared resources, the following fields need to be provided:

- `namespace` - The name of the Azure Event Hubs namespace.
- `topicName` - The name of the topic. *(optional)*

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsofteventhubnamespaces).

The following scraper-specific metric label will be added:

- `entity_name` - Name of the topic

> :warning: **As of today, it is not supported to combine `topicName` with `EntityPath` as a dimension.**

Example:

```yaml
name: azure_event_hubs_incoming_messages
description: "The number of incoming messages on an Azure Event Hubs topic"
resourceType: EventHubs
azureMetricConfiguration:
  metricName: IncomingMessages
  aggregation:
    type: Total
resources: # Optional, required when no resource discovery is configured
- namespace: promitor-streaming
  topicName: orders
- namespace: promitor-messaging
  topicName: sales
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: event-hubs-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
