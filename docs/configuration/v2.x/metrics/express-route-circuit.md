---
layout: default
title: Azure Express Route Circuit Declaration
---

## Azure Express Route Circuit

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Express Route Circuit (without Peerings) via the `ExpressRouteCircuit` resource
type.

When using declared resources, the following fields need to be provided:

- `expressRouteCircuitName` - The name of the Azure Express Route circuit

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftnetworkexpressroutecircuits).

Example:

```yaml
name: azure_express_route_percentage_arp_availability
description: "Average percentage of arp availability on an Azure express route circuit"
resourceType: ExpressRouteCircuit
azureMetricConfiguration:
  metricName: ArpAvailability
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- expressRouteCircuitName: promitor-express-route-circuit-1
- expressRouteCircuitName: promitor-express-route-circuit-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: express-route-circuit-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
