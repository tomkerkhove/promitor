---
layout: default
title: Azure Database for MariaDB Declaration
---

## Azure Database for MariaDB

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v2.6-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Database for MariaDB via the `MariaDb` resource
type.

When using declared resources, the following fields need to be provided:

- `serverName` - The name of the Azure Database for MariaDB server

All supported metrics are documented in the official [Azure Monitor documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftdbformariadbservers).

Example:

```yaml
name: azure_db_mariadb_percentage_cpu
description: "Average percentage cpu usage on an Azure Database for MariaDB"
resourceType: MariaDb
azureMetricConfiguration:
  metricName: cpu_percent
  aggregation:
    type: Average
resources: # Optional, required when no resource discovery is configured
- serverName: promitor-maria-db-1
- serverName: promitor-maria-db-2
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: maria-db-landscape
```

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
