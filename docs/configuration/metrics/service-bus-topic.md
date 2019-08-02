---
layout: default
title: Azure Service Bus Topic Declaration
---

## Azure Service Bus Topic - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0.0-green.svg)
You can declare to scrape an Azure Service Bus Topic via the `ServiceBusTopic` resource type.

The following fields need to be provided:
- `namespace` - The name of the Service Bus namespace
- `topicName` - The name of the topic
- `subscriptionName` - The name of the subscription

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftservicebusnamespaces).

Example:
```yaml
name: demo_topic_size
description: "Amount of active messages of the 'mytopic' topic"
resourceType: ServiceBusTopic
namespace: promitor-messaging
topicName: orders
subscriptionName: sello-vendor
azureMetricConfiguration:
  metricName: ActiveMessages
  aggregation:
    type: Total
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)