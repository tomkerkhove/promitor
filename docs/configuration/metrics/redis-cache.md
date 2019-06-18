---
layout: default
title: Azure Cache for Redis Declaration
---

## Azure Cache for Redis - ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1.0-green.svg)
You can declare to scrape an Azure Cache for Redis via the `RedisCache` resource type.

The following fields need to be provided:
- `cacheName` - The name of the Redis Cache service

The list of supported metrics is available on the Azure Cache for Redis [monitoring documentation page](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-how-to-monitor#available-metrics-and-reporting-intervals).

Example:
```yaml
name: redis_cache_hits
description: "The number of successful key lookups during the specified reporting interval. This maps to keyspace_hits from the Redis INFO command."
resourceType: RedisCache
cacheName: cachejco
scraping:
  schedule: "0 */2 * ? * *"
azureMetricConfiguration:
  metricName: CacheHits
  aggregation:
    type: Average
    interval: 00:01:00
```

[&larr; back to metrics declarations](/configuration/metrics)<br />
[&larr; back to introduction](/)