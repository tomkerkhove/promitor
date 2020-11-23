---
layout: default
title: Frequently asked questions (FAQs)
---

Here are a list of questions you may have:

- [Are multi-dimensional metrics supported?](#are-multi-dimensional-metrics-supported)
- [Is scraping multiple subscriptions supported?](#is-scraping-multiple-subscriptions-supported)
- [What Azure clouds are supported?](#what-azure-clouds-are-supported)
- [Why does Azure Blob & File Storage only report account-level information?](#why-does-azure-blob--file-storage-only-report-account-level-information)
- [Why does my multi-dimensional metric report label value `unknown` with Prometheus?](#why-does-my-multi-dimensional-metric-report-label-value-unknown-with-prometheus)
- [What Helm version is supported?](#what-helm-version-is-supported)
- [What operating systems are supported?](#what-operating-systems-are-supported)

## Are multi-dimensional metrics supported?

Yes, every scraper supports scraping multi-dimensional metrics except for
Azure Storage queues.

You can configure the dimension you are interested in via
`azureMetricConfiguration.dimension.Name`, for more information see
our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

However, you can only use it with metrics in Azure Monitor that support this,
for a complete overview we recommend reading the
[official documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported).

## Is scraping multiple subscriptions supported?

No, we do not support scraping multiple subscriptions as of today as we consider that to be a security boundary.

However, you can deploy multiple instances of Promitor that each scrape another subscription.

We have it on [our backlog](https://github.com/tomkerkhove/promitor/issues/761) to see if there is
 enough demand for it, feel free to give a :+1:. If that is the case, we will reconsider this limitation.

## What Azure clouds are supported?

We support `Global` (default), `China`, `UsGov` & `Germany` Azure clouds.

This can be configured in the metric configuration under `azureMetadata`.

For more information see our ['Metric Configuration' page](/configuration/v1.x/metrics/#metrics).

## Why does Azure Blob & File Storage only report account-level information?

Azure Monitor currently only provides account-level metrics which we can serve.

As part of [#450](https://github.com/tomkerkhove/promitor/issues/450) &
 [#446](https://github.com/tomkerkhove/promitor/issues/446) we plan to provide the capability to have more granular information.

## Why does my multi-dimensional metric report label value `unknown` with Prometheus?

When Promitor is unable to find a metric for a multi-dimensional metric, it will report `unknown` for the dimension
 label given it was not able to determine what the dimension value is due to the lack of metrics.

You can read more about it in our [Prometheus sink documentation](/configuration/v2.x/runtime/scraper/#what-happens-when-metrics-are-unavailable-for-multi-dimensional-metrics).

## What Helm version is supported?

Promitor supports deployments with **Helm v3.0** for all versions.

Helm has [deprecated support for v2.0](https://helm.sh/blog/helm-v2-deprecation-timeline/) and is out of support.
 Because of that, Promitor supported to remove support for Helm 2.0 as of Promitor 2.0.

However, while we don't support it out-of-the-box it is possible that it is still compatible with Helm 2.0 but we do
 not provide support for it.

Join the discussion on our Helm v2.0 support deprecation on [GitHub Discussions](https://github.com/tomkerkhove/promitor/discussions/1375).

## What operating systems are supported?

We support running on both Linux & Windows platforms.

[&larr; back](/)
