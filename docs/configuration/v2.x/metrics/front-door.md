---
layout: default
title: Azure Front Door Declaration
---

## Azure Front Door

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Front Door via the `FrontDoor` resource
type.

The following fields need to be provided:

- `name` - The name of the Azure Front Door

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworkfrontdoors).

Example:

```yaml
name: azure_express_route_percentage_arp_availability
description: "Average percentage of arp availability on an Azure express route circuit"
resourceType: FrontDoor
azureMetricConfiguration:
  metricName: ArpAvailability
  aggregation:
    type: Average
resources:
- expressRouteCircuitName: promitor-express-route-circuit-1
- expressRouteCircuitName: promitor-express-route-circuit-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: express-route-circuit-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
