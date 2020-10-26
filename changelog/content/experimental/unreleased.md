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
 | [#314](https://github.com/tomkerkhove/promitor/issues/314)).
- {{% tag added %}} Support for scraping Azure Express Route circuits ([docs](https://promitor.io/configuration/v2.x/metrics/express-route-circuit) | [#1251](https://github.com/tomkerkhove/promitor/issues/1251) | Contributed by [@bluepixbe](https://github.com/bluepixbe) 🎉).
- {{% tag added %}} Support for scraping Azure Application Gateway ([docs](https://promitor.io/configuration/v2.x/metrics/application-gateway) | [#1251](https://github.com/tomkerkhove/promitor/issues/313) | Contributed by [@bluepixbe](https://github.com/bluepixbe) 🎉).
- {{% tag added %}} Support for scraping Azure Network Gateway ([docs](https://promitor.io/configuration/v2.x/metrics/network-gateway) | [#1264](https://github.com/tomkerkhove/promitor/issues/1264) | Contributed by [@bluepixbe](https://github.com/bluepixbe) 🎉).
- {{% tag added %}} Support for scraping Azure Kubernetes Service ([docs](https://promitor.io/configuration/v2.x/metrics/kubernetes) | [#333](https://github.com/tomkerkhove/promitor/issues/333) | Contributed by [@jkataja](https://github.com/jkataja) 🎉).
- {{% tag added %}} New validation rule to ensure at least one resource or resource collection is configured to scrape
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. ([#1105](https://github.com/tomkerkhove/promitor/issues/1105) | Contributed by [@adamconnelly](https://github.com/adamconnelly) 🎉).
- {{% tag added %}} Add validation to ensure the scraping schedule is a valid Cron expression. ([#1103](https://github.com/tomkerkhove/promitor/issues/1103) | Contributed by [@adamconnelly](https://github.com/adamconnelly) 🎉).
- {{% tag added %}} Provide support for pushing metrics to Atlassian Statuspage
 ([docs](https://promitor.io/configuration/v2.x/runtime#atlassian-statuspage) | [#1152](https://github.com/tomkerkhove/promitor/issues/1152))
- {{% tag added %}} Provide suggestions when unknown fields are found in the metrics config. ([#1105](https://github.com/tomkerkhove/promitor/issues/1105) | Contributed by [@adamconnelly](https://github.com/adamconnelly) 🎉).
- {{% tag added %}} New validation rule to ensure the scraping schedule is a valid Cron expression. ([#1103](https://github.com/tomkerkhove/promitor/issues/1103)).
- {{% tag added %}} New validation rule to ensure declarative or dynamic discovery for metrics to scrape are configured
- {{% tag added %}} New System API endpoint giving runtime information ([docs](https://promitor.io/operations/#system)
 | [#1208](https://github.com/tomkerkhove/promitor/issues/1208))
- {{% tag changed %}} Show Promitor version during startup
- {{% tag changed %}} Provide capability to scrape all queues in Azure Service Bus, instead of having to declare the
 queue name. ([#529](https://github.com/tomkerkhove/promitor/issues/529)).
- {{% tag changed %}} Handle validation failures on startup more gracefully. ([#1113](https://github.com/tomkerkhove/promitor/issues/1113) | Contributed by [@adamconnelly](https://github.com/adamconnelly) 🎉).
- {{% tag changed %}} Improve time series handling to ensure finalized time series are reported
- {{% tag changed %}} `service.exposeExternally` is renamed to `service.loadBalancer.enabled` in Promitor Agent Helm chart
- {{% tag changed %}} Default name for `rbac.serviceAccount.name` is now `promitor-scraper` in Promitor Agent Helm chart
- {{% tag fixed %}} Ensure Prometheus metric sink does write timestamps ([#1217](https://github.com/tomkerkhove/promitor/issues/1217)).
- {{% tag fixed %}} Dimensions with `/` in name are now supported by replacing it with `_` for Prometheus metric sink ([#1248](https://github.com/tomkerkhove/promitor/issues/1248)).
- {{% tag removed %}} Support for Prometheus legacy configuration ([deprecation notice](https://changelog.promitor.io/#prometheus-legacy-configuration)
 | [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))
- {{% tag removed %}} Support for Swagger UI 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-ui-2-0) |
 [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))
- {{% tag removed %}} Support for Swagger 2.0 ([deprecation notice](https://changelog.promitor.io/#swagger-2-0) |
 [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x))

Learn how to migrate to 2.0 with our [migration guide](https://promitor.io/walkthrough/migrate-from-1.x-to-2.x).
