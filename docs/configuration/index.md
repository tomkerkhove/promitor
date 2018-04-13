---
layout: default
title: Promitor - General Configuration
---

Here is an overview of how you can configure Promitor.

# Scraping
Promitor automatically scrapes Azure Monitor and makes the information available based on the metrics configuration.

The behavior of this can be configured with the following environment variables:
- **PROMITOR_CONFIGURATION_PATH** - Defines the location of the YAML file that declares what Azure Monitor metrics to scrape. If nothing is specified, `/config/metrics.yaml` will be used.
- **PROMITOR_SCRAPE_BASEPATH** - Controls the path where the scraping endpoint for Prometheus is being exposed. If nothing is specified, `/prometheus/scrape` will be used.
- **PROMITOR_SCRAPE_SCHEDULE** - A cron expression that controls the fequency in which all the configured metrics will be scraped from Azure Monitor. If configured is specified, `*/5 * * * *` will be used.

# Authentication with Azure Monitor
Authentication with Azure Monitor is fully integrated with Azure AD where you will need to create an entity, preferably an Azure AD Application, that will be used for integrating with the Azure Monitor API.

The following environment variables need to be provided:
- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

The entity in the Azure AD needs to have `Monitoring Reader` permission on the resource group that will be queried. More information can be found [here](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-roles-permissions-security).

[&larr; back](/)