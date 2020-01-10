---
layout: default
title: Azure Function App Declaration
---

## Azure Function App - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.2-green.svg)

You can declare to scrape an Azure Function App via the `FunctionApp` resource
type.

The following fields need to be provided:

- `functionAppName` - The name of the Azure Function App

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
resources:
- functionAppName: promitor-function-app
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
