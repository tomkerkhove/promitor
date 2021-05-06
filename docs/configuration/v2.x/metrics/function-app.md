---
layout: default
title: Azure Function App Declaration
---

## Azure Function App

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.2-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Function App via the `FunctionApp` resource
type.

When using declared resources, the following fields need to be provided:

- `functionAppName` - The name of the Azure Function App
- `slotName` - The name of the deployment slot *(optional)*

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftwebsites-functions).

The following scraper-specific metric label will be added:

- `slot_name` - Name of the deployment slot. If none is specified, `production` will be used.

Example:

```yaml
name: azure_function_requests
description: "Amount of requests for an Azure Function App"
resourceType: FunctionApp
azureMetricConfiguration:
  metricName: Requests
  aggregation:
    type: Total
resources: # Optional, required when no resource discovery is configured
- functionAppName: promitor-function-app
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: function-app-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
