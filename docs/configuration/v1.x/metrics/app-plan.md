---
layout: default
title: Azure App Plan Declaration
---

## Azure App Plan - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.2-green.svg)

You can declare to scrape an Azure App Plan via the `AppPlan` resource
type.

The following fields need to be provided:

- `appPlanName` - The name of the Azure App Plan

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftwebserverfarms).

Example:

```yaml
name: azure_app_plan_percentage_memory
description: "Average percentage of memory usage on an Azure App Plan"
resourceType: AppPlan
azureMetricConfiguration:
  metricName: MemoryPercentage
  aggregation:
    type: Average
resources:
- appPlanName: promitor-app-plan
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
