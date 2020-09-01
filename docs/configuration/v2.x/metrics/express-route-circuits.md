---
layout: default
title: Azure Express Route Circuits Declaration
---

## Azure Express Route Circuits

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Express Route Circuits (without Peerings) via the `ExpressRouteCircuits` resource
type.

The following fields need to be provided:

- `expressRouteCircuitsName` - The name of the express route circuits

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworkexpressroutecircuits).

Example:

```yaml
name: azure_express_route_percentage_arp_availability
description: "Average percentage of arp availability on an Azure express route circuits"
resourceType: ExpressRouteCircuits
azureMetricConfiguration:
  metricName: ArpAvailability
  aggregation:
    type: Average
resources:
- expressRouteCircuitsName: promitor-express-route-circuits-1
- expressRouteCircuitsName: promitor-express-route-circuits-2
resourceDiscoveryGroups:
- name: express-route-circuits-group
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
