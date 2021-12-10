---
layout: default
title: Azure Database for PostgreSQL
---

## Azure Database for PostgreSQL

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.0-green.svg)![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)

You can declare to scrape an Azure Database for PostgreSQL server via the `PostgreSql`
resource type.

When using declared resources, the following fields need to be provided:

- `serverName` - The name of the PostgreSQL server
- `type` - The type of PostgreSQL server. *(optional)*
  - Allowed values are `Simple` (default), `Flexible` & `Hyperscale`.

All supported metrics are documented in the official Azure Monitor documentation:

- [Simple servers](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdbforpostgresqlservers)
- [Flexible servers](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftdbforpostgresqlflexibleservers)
- [Hyperscale servers](https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/metrics-supported#microsoftdbforpostgresqlservergroupsv2)

Example:

```yaml
name: azure_postgre_sql_cpu_percent
description: "The CPU percentage on the server"
resourceType: PostgreSql
scraping:
  schedule: "0 */2 * ? * *"
azureMetricConfiguration:
  metricName: cpu_percent
  aggregation:
    type: Average
    interval: 00:01:00
resources: # Optional, required when no resource discovery is configured
- serverName: Promitor-1
- serverName: Promitor-2
  type: Flexible
resourceDiscoveryGroups: # Optional, requires Promitor Resource Discovery agent (https://promitor.io/concepts/how-it-works#using-resource-discovery)
- name: postgres-database-landscape
```

### Limitations

- Resource discovery will discover all types of servers and thus the used metrics should match all of the types.
  - You can use tags to define which resources to include, if you need filtering capabilities.

<!-- markdownlint-disable MD033 -->
[&larr; back to metrics declarations](/configuration/v2.x/metrics)<br />
[&larr; back to introduction](/)
<!-- markdownlint-enable -->
