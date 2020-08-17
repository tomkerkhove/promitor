---
title: "(2018-09-15)"
date: 2018-09-02T20:46:47+02:00
weight: 1
version:
---

- {{% tag added %}} Support for resource discovery ([docs](https://promitor.io/configuration/v2.x/resource-discovery) |
 [configuration](https://promitor.io/configuration/v2.x/resource-discovery) |
 [deployment](https://promitor.io/deployment/resource-discovery/deployment))
- {{% tag added %}} Support for scraping Azure Event Hubs ([docs](https://promitor.io/configuration/v2.x/metrics/event-hubs)
 | [#372](https://github.com/tomkerkhove/promitor/issues/69))
- {{% tag added %}} Support for scraping Azure Logic Apps ([docs](https://promitor.io/configuration/v2.x/metrics/logic-apps)
 | [#314](https://github.com/tomkerkhove/promitor/issues/314))
- {{% tag added %}} New validation rule to ensure at least one resource or resource collection is configured to scrape
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. [#1105](https://github.com/tomkerkhove/promitor/issues/1105).
- {{% tag added %}} Add validation to ensure the scraping schedule is a valid Cron expression. [#1103](https://github.com/tomkerkhove/promitor/issues/1103).
- {{% tag added %}} Provide support for pushing metrics to Atlassian Statuspage
 ([docs](https://promitor.io/configuration/v2.x/runtime#atlassian-statuspage) | [#1152](https://github.com/tomkerkhove/promitor/issues/1152))
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. [#1105](https://github.com/tomkerkhove/promitor/issues/1105).
- {{% tag added %}} New validation rule to ensure the scraping schedule is a valid Cron expression. [#1103](https://github.com/tomkerkhove/promitor/issues/1103).
- {{% tag added %}} New validation rule to ensure declarative or dynamic discovery for metrics to scrape are configured
- {{% tag added %}} New System API endpoint giving runtime information ([docs](https://promitor.io/operations/#system)
 | [#1208](https://github.com/tomkerkhove/promitor/issues/1208))
- {{% tag changed %}} Provide capability to scrape all queues in Azure Service Bus, instead of having to declare the
 queue name. [#529](https://github.com/tomkerkhove/promitor/issues/529).
- {{% tag changed %}} Handle validation failures on startup more gracefully. [#1113](https://github.com/tomkerkhove/promitor/issues/1113).
- {{% tag changed %}} Improve time series handling to ensure finalized time series are reported
- {{% tag fixed %}} Ensure Prometheus metric sink does write timestamps [#1217](https://github.com/tomkerkhove/promitor/issues/1217).
- {{% tag removed %}} Support for Prometheus legacy configuration ([deprecation notice](https://changelog.promitor.io/#prometheus-legacy-configuration)
 | [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))
- {{% tag removed %}} Support for Swagger UI 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-ui-2-0) |
 [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))
- {{% tag removed %}} Support for Swagger 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-2-0) |
 [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))

Learn how to migrate to 2.0 with our [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x).
