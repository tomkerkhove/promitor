---
layout: default
title: Azure Monitor Autoscale Declaration
---

## Azure Monitor Autoscale

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.3-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Monitor Autoscale via the `MonitorAutoscale`
resource type.

When using declared resources, the following fields need to be provided:

- `autoscaleSettingsName` - The name of the Azure Monitor Autoscale settings

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftinsightsautoscalesettings).

Example:

```yaml
- name: promitor_demo_appplan_autoscale_observed_capacity
  description: "Average amount of current instances for an Azure App Plan with Azure Monitor Autoscale"
  resourceType: MonitorAutoscale
  labels:
    app: promitor
  azureMetricConfiguration:
    metricName: ObservedCapacity
    aggregation:
      type: Average
  resources: # Optional, required when no resource discovery is configured
  - autoscaleSettingsName: app-service-autoscaling-rules
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: autoscaling-rules
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
