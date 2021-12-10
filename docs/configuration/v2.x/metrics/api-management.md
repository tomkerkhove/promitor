---
layout: default
title: Azure API Management Declaration
---

## Azure API Management

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.3-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure API Management via the `ApiManagement`
 resource type.

When using declared resources, the following fields need to be provided:

- `instanceName` - The name of the Azure API Management instance.
- `locationName` - The name of the regional deployment of the gateway. *(optional)*

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftapimanagementservice).

### Multi-region support

Azure API Management instances can be deployed to multiple regions across the world.

Promitor supports different scenarios:

1. Report metrics for metrics for all locations (default)
2. Scope metric to a single region by configuring `locationName`.
3. Report metrics but split it across all regions by using the `Location` dimension.

The following scraper-specific metric label will be added for scenario 2 & 3:

- `location` - Name of the location

Example:

```yaml
name: promitor_demo_azureapimanagement_capacity
description: "The amount of capacity used an Azure API Management instance."
resourceType: ApiManagement
azureMetricConfiguration:
  metricName: Capacity
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- instanceName: promitor-api-gateway
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: api-management-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
