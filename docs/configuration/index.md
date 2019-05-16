---
layout: default
title: General Configuration
---

Here is an overview of how you can configure Promitor.

# Runtime
The Promitor runtime is flexible and allows you to configure it to meet your needs:
- **PROMITOR_HTTP_PORT** - Defines the port to serve HTTP traffic _(default 80)_

# Scraping
Promitor automatically scrapes Azure Monitor and makes the information available based on the metrics configuration.

The behavior of this can be configured with the following environment variables:
- **PROMITOR_CONFIGURATION_PATH** - Defines the location of the YAML file that declares what Azure Monitor metrics to scrape. If nothing is specified, `/config/metrics-declaration.yaml` will be used.
- **PROMITOR_SCRAPE_BASEPATH** - Controls the path where the scraping endpoint for Prometheus is being exposed. If nothing is specified, `/metrics` will be used.

We're also providing feature flags to opt-out of certain features:
- **PROMITOR_FEATURE_METRICSTIMESTAMP** - Defines whether or not a timestamp should be included when the value was scraped on Azure Monitor. Supported values are `True` to opt-in & `False` to opt-out, if nothing is configured this will be turned on.

# Authentication with Azure Monitor
Authentication with Azure Monitor is fully integrated with Azure AD. In order to use Promitor, you'll need to [create an Azure AD Application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#create-an-azure-active-directory-application), that will be used for integrating with the Azure Monitor API.

The following environment variables need to be provided:
- **PROMITOR_AUTH_APPID** - Id of the Azure AD entity to authenticate with
- **PROMITOR_AUTH_APPKEY** - Secret of the Azure AD entity to authenticate with

This information can be found on the newly created AD Application as documented [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal#get-application-id-and-authentication-key).

The entity in the Azure AD needs to have `Monitoring Reader` permission on the resource group that will be queried. More information can be found [here](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-roles-permissions-security).

# Logging & Telemetry
We provide insights in how our runtime is doing and is written to `stdout`.

This can be controlled via the following environment variables:
- **PROMITOR_LOGGING_MINIMUMLOGLEVEL** - Defines the minimum log level that should be logged. If none is configured, `Warning` will be used. Allowed values are `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`, `None` ordered from most to least verbose.

## External Telemetry
Promitor can send telemetry to Azure Application Insights when there is a need to.

It currently supports:
- Exceptions during scraping

In order to enable this feature, the following environment variables need to be provided:
- **PROMITOR_TELEMETRY_INSTRUMENTATIONKEY** - Defines the instrumentation key to use when sending telemetry to Azure Application Insights

[&larr; back](/)
