---
layout: default
title: Azure Logic Apps Declaration
---

## Azure Logic Apps

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Logic App via the `LogicApp`
resource type.

When using declared resources, the following fields need to be provided:

- `workflowName` - The name of the Azure Logic App

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftlogicworkflows).

Example:

```yaml
name: azure_logic_apps_failed_run
description: "Total amount of failed runs for Azure Logic Apps"
resourceType: LogicApp
azureMetricConfiguration:
  metricName: RunsFailed
  aggregation:
    type: Total
resources: # Optional, required when no resource discovery is configured
- workflowName: promitor-workflow-1
- workflowName: promitor-workflow-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: logic-apps-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
