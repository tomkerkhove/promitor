---
layout: default
title: Azure Application Insights Declaration
---

## Azure Application Insights

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.6-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Application Insights via the `ApplicationInsights` resource
type.

When using declared resources, the following fields need to be provided:

- `name` - The name of the Azure Application Insights

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftinsightscomponents).

Example:

```yaml
name: azure_application_insights_exceptions
description: "Average amount of server exceptions in Azure Application Insights"
resourceType: ApplicationInsights
azureMetricConfiguration:
  metricName: exceptions/server
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- name: promitor-application-gateway-1
- name: promitor-application-gateway-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: application-insights-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
