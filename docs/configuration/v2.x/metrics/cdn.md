---
layout: default
title: Azure Content Delivery Network (CDN) Declaration
---

## Azure Content Delivery Network (CDN)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.6-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure CDN via the `Cdn` resource
type.

When using declared resources, the following fields need to be provided:

- `cdnName` - The name of the Azure CDN resource

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftcdnprofiles).

> 🚨 The availability of metrics depends on the SKU of the Azure CDN resource.

Example:

```yaml
name: azure_cdn_requests
description: "Amount of requests sent to Azure CDN"
resourceType: Cdn
azureMetricConfiguration:
  metricName: RequestCount
  aggregation:
    type: Total
resources: # Optional, required when no resource discovery is configured
- cdnName: promitor-cdn-1
- cdnName: promitor-cdn-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: cdn-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
