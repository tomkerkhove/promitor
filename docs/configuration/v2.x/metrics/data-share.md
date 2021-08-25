---
layout: default
title: Azure Data Share Declaration
---

## Azure Data Share

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.5-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Data Share resource via the `DataShare` resource
type.

When using declared resources, the following fields need to be provided:

- `accountName` - The name of the Azure Data Share account
- `shareName` - The name of the share *(optional)*

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftdatashareaccounts).

The following scraper-specific metric label will be added:

- `share_name` - Name of the share.

Example:

```yaml
- name: promitor_demo_data_share_received
  description: "Amount of shares received from other parties per Azure Data Share account"
  resourceType: DataShare
  azureMetricConfiguration:
    metricName: ShareSubscriptionCount
    aggregation:
      type: Maximum
  resources: # Optional, required when no resource discovery is configured
  - accountName: promitor-data-share
    shareName: Promitor
  resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
  - name: data-share-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
