---
layout: default
title: Frequently asked questions (FAQs)
---

# Are multi-dimensional metrics supported?

Yes, every scraper supports scraping multi-dimensional metrics except for Azure Storage queues.

You can configure the dimension you are interested in via `azureMetricConfiguration.dimensionName`, for more information see our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

However, you can only use it with metrics in Azure Monitor that support this, for a complete overview we recommend reading the [official documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported).

[&larr; back](/)