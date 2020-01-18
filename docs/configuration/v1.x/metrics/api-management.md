---
layout: default
title: Azure API Management Declaration
---

## Azure API Management - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)

You can scrape an Azure API Management via the `ApiManagement`
 resource type.

The following fields need to be provided:

- `instanceName` - The name of the Azure API Management instance.

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftapimanagementservice).

Example:

```yaml
name: promitor_demo_azureapimanagement_capacity
description: "The amount of capacity used an Azure API Management instance."
resourceType: ApiManagement
azureMetricConfiguration:
  metricName: Capacity
  aggregation:
    type: Average
resources:
- instanceName: promitor-api-gateway
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
