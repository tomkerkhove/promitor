---
layout: default
title: Generic Azure Resource Declaration
---

## Generic Azure Resource

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.2-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-No-red.svg)

You can declare to scrape a generic Azure resource via the `Generic` resource type.

Promitor simplifies defining resource uris by using the subscription & resource
group defined in `azureMetadata` so that your configuration is small & readable.

Mandatory fields :

- `resourceUri` - The uri of the Azure resource to scrape.

Optional fields :

- `resourceGroupName` - the resource group for this resource. It overrides the one defined in `azureMetadata`.
- `subscriptionId` - the subscription ID for this resource. It overrides the one defined in `azureMetadata`.
- `filter` - The filter to use to have fine-grained metrics. Example: `EntityName eq 'orders'`.
   See [Azure Monitor REST API Filter Syntax](https://docs.microsoft.com/en-us/rest/api/monitor/filter-syntax).

Example:

```yaml
name: azure_service_bus_active_messages
description: "Amount of active messages of the 'myqueue' queue (determined with Generic provider)"
resourceType: Generic
azureMetricConfiguration:
  metricName: ActiveMessages
  aggregation:
    type: Total
resources:
# Will scrape subscriptions/<sub>/resourceGroups/<rg>/providers/Microsoft.ServiceBus/namespaces/my-promitor-messaging
# Where <sub> & <rg> are coming from azureMetadata
- resourceUri: Microsoft.ServiceBus/namespaces/my-promitor-messaging
  filter: EntityName eq 'orders'
# Will scrape subscriptions/<sub>/resourceGroups/<rg>/providers/Microsoft.ServiceBus/namespaces/my-other-promitor-messaging
# Where <sub> & <rg> are coming from the definition of this resource.
- resourceUri: Microsoft.ServiceBus/namespaces/my-other-promitor-messaging
  subscriptionId: example-subscription
  resourceGroupName: example-resource-group
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
