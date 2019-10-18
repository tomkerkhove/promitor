---
layout: default
title: Generic Azure Resource Declaration
---

## Generic Azure Resource - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.2.0-green.svg)

You can declare to scrape a generic Azure resource via the `Generic` resource type.

The following fields need to be provided:

- `resourceUri` - The uri of the Azure resource to scrape.
- `filter` - The filter to use to have fine-grained metrics. Example:
  `EntityName eq 'orders'`

Promitor simplifies defining resource uris by using the subscription & resource
group defined in `azureMetadata` so that your configuration is small & readable.

Example:

```yaml
name: demo_generic_queue_size
description: "Amount of active messages of the 'myqueue' queue (determined with Generic provider)"
resourceType: Generic
# Will scrape subscriptions/<sub>/resourceGroups/<rg>/providers/Microsoft.ServiceBus/namespaces/promitor-messaging
# Where 'sub' & 'rg' are coming from azureMetadata
resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
filter: EntityName eq 'orders'
azureMetricConfiguration:
  metricName: ActiveMessages
  aggregation:
    type: Total
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v0.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
