---
layout: default
title: Azure Automation Account Declaration
---

## Azure Automation account

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.1-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can scrape an Azure Automation account via the `AutomationAccount`
 resource type.

When using declared resources, the following fields need to be provided:

- `accountName` - The name of the Azure Automation account.
- `runbookName` - The name of the runbook. (optional and only supported on limited metrics)

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftautomationautomationaccounts).

The following scraper-specific metric label will be added :

- `runbook_name` - Name of the runbook

Example:

```yaml
name: promitor_demo_automation_job_count
description: "Amount of jobs per Azure Automation account & job"
resourceType: AutomationAccount
azureMetricConfiguration:
  metricName: TotalJob
  aggregation:
    type: Total
resources: # Optional, required when no resource discovery is configured
- resourceGroupName: promitor-sources
  accountName: promitor-sandbox
  runbookName: Example # Optional, currently only supported for 'TotalJob' metric
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: automation-accounts
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
