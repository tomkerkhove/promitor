---
layout: default
title: Frequently asked questions (FAQs)
---

## What Azure clouds are supported?

We support `Global` (default), `China`, `UsGov` & `Germany` Azure clouds.

This can be configured in the metric configuration under `azureMetadata`.

For more information see our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

## Are multi-dimensional metrics supported?

Yes, every scraper supports scraping multi-dimensional metrics except for
Azure Storage queues.

You can configure the dimension you are interested in via
`azureMetricConfiguration.dimension.Name`, for more information see
our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

However, you can only use it with metrics in Azure Monitor that support this,
for a complete overview we recommend reading the
[official documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported).

[&larr; back](/)
