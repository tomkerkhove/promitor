---
layout: default
title: Azure Web App Declaration
---

## Azure Web App - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.2-green.svg)

You can declare to scrape an Azure Web App via the `WebApp` resource
type.

The following fields need to be provided:

- `webAppName` - The name of the Azure Web App
- `slotName` - The name of the deployment slot *(optional)*

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftwebsites-excluding-functions).

The following scraper-specific metric label will be added:

- `slot_name` - Name of the deployment slot. If none is specified, `production` will be used.

Example:

```yaml
name: azure_web_app_requests
description: "Amount of requests for an Azure Web App"
resourceType: WebApp
azureMetricConfiguration:
  metricName: Requests
  aggregation:
    type: Total
resources:
- webAppName: promitor-web-app
  slot: staging
- webAppName: promitor-web-app
  slot: production
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v1.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
