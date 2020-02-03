---
layout: default
title: Frequently asked questions (FAQs)
---

Here are a list of questions you may have:

- [What Azure clouds are supported?](#what-azure-clouds-are-supported)
- [What operating systems are supported?](#what-operating-systems-are-supported)
- [Is scraping multiple subscriptions supported?](#is-scraping-multiple-subscriptions-supported)
- [Are multi-dimensional metrics supported?](#are-multi-dimensional-metrics-supported)
- [Why does Azure Blob & File Storage only report account-level information?](#why-does-azure-blob--file-storage-only-report-account-level-information)

## What Azure clouds are supported?

We support `Global` (default), `China`, `UsGov` & `Germany` Azure clouds.

This can be configured in the metric configuration under `azureMetadata`.

For more information see our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

## What operating systems are supported?

We support running on both Linux & Windows platforms.

## Is scraping multiple subscriptions supported?

No, we do not support scraping multiple subscriptions as of today as we consider that to be a security boundary.

However, you can deploy multiple instances of Promitor that each scrape another subscription.

We have it on [our backlog](https://github.com/tomkerkhove/promitor/issues/761) to see if there is
 enough demand for it, feel free to give a :+1:. If that is the case, we will reconsider this limitation.

## Are multi-dimensional metrics supported?

Yes, every scraper supports scraping multi-dimensional metrics except for
Azure Storage queues.

You can configure the dimension you are interested in via
`azureMetricConfiguration.dimension.Name`, for more information see
our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

However, you can only use it with metrics in Azure Monitor that support this,
for a complete overview we recommend reading the
[official documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported).

## Why does Azure Blob & File Storage only report account-level information?

Azure Monitor currently only provides account-level metrics which we can serve.

As part of [#450](https://github.com/tomkerkhove/promitor/issues/450) &
 [#446](https://github.com/tomkerkhove/promitor/issues/446) we plan to provide the capability to have more granular information.

[&larr; back](/)
