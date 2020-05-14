---
title:
subtitle: created on {{ now.Format "2006-01-02" }}
date: 2020-05-14T12:00:00+01:00
removal_date: 2019-07-07
weight:
version:
---

#### Prometheus Legacy Configuration

###### Deprecated as of v1.6 and will be removed in v2.0

Promitor has added support for different metric sinks in v1.6 along with a Prometheus
 Scraping endpoint sink.

This scraping endpoint works the same way as before but has a different way of configuration
 which is more in line with other sinks.

Because of that we are removing support for the "legacy" Prometheus configuration and use
 a unified metric sink approach.

We've added support for Prometheus metric sink while maintaining the legacy approach until
 it's being removed.

**Impact:** Migration is required - Prometheus legacy configuration will no longer be available.

**Alternative:** Use our new [metric sink concept](https://promitor.io/configuration/v1.x/runtime#prometheus-scraping-endpoint).

**Discussion:** [GitHub Issue #1032](https://github.com/tomkerkhove/promitor/issues/1032)
