---
title: "(2018-09-15)"
date: 2018-09-02T20:46:47+02:00
weight: 1
version:
---

- {{% tag removed %}} Support for Prometheus legacy configuration ([deprecation notice](https://changelog.promitor.io/#prometheus-legacy-configuration))
- {{% tag removed %}} Support for Swagger UI 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-ui-2-0))
- {{% tag removed %}} Support for Swagger 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-2-0))
- {{% tag added %}} Support for scraping Azure Logic Apps ([docs](https://promitor.io/configuration/v2.x/metrics/logic-apps)
 | [#372](https://github.com/tomkerkhove/promitor/issues/314))
- {{% tag added %}} New validation rule to ensure at least one resource or resource collection is configured to scrape
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. [#1105](https://github.com/tomkerkhove/promitor/issues/1105).
- {{% tag added %}} Add validation to ensure the scraping schedule is a valid Cron expression. [#1103](https://github.com/tomkerkhove/promitor/issues/1103).
- {{% tag changed %}} Handle validation failures on startup more gracefully. [#1113](https://github.com/tomkerkhove/promitor/issues/1113).
- {{% tag added %}} Provide support for pushing metrics to Atlassian Statuspage
 ([docs](https://promitor.io/configuration/v2.x/runtime#atlassian-statuspage) | [#1152](https://github.com/tomkerkhove/promitor/issues/1152))
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. [#1105](https://github.com/tomkerkhove/promitor/issues/1105).
- {{% tag added %}} New validation rule to ensure the scraping schedule is a valid Cron expression. [#1103](https://github.com/tomkerkhove/promitor/issues/1103).
- {{% tag added %}} New validation rule to ensure declarative or dynamic discovery for metrics to scrape are configured
