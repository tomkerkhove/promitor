---
layout: default
title: Runtime Configuration
---

This article covers an overview of all the knobs that you can tweak to align the runtime with your needs.

Promitor runtime is configured by mounting a volume to `/config/runtime.yaml`.

We provide the capability to override te runtime YAML via [environment variables](#overriding-configuration-with-environment-variables), if you have the need for it.

Here is a complete example of the runtime YAML:

```yaml
server:
  httpPort: 80 # Optional. Default: 80
prometheus:
  metricUnavailableValue: NaN # Optional. Default: NaN
  enableMetricTimestamps: false # Optional. Default: true
  scrapeEndpoint:
    baseUriPath: /metrics # Optional. Default: /metrics
metricsConfiguration:
  absolutePath: /config/metrics-declaration.yaml # Optional. Default: /config/metrics-declaration.yaml
telemetry:
  applicationInsights:
    instrumentationKey: ABC # Optional. Note: Required to be specified when turned on
    isEnabled: false # Optional. Default: false
    verbosity: trace # Optional. Default: N/A
  containerLogs:
    isEnabled: true # Optional. Default: true
    verbosity: trace # Optional. Default: N/A
  defaultVerbosity: error # Optional. Default: error
```

_Note: Using Promitor v0.x? [Use environment variables](./../v0.x/) to configure the runtime._

# Runtime
The Promitor runtime is flexible and allows you to configure it to meet your needs:
- `server.httpPort` - Defines the port to serve HTTP traffic _(default 80)_

Example:
```yaml
server:
  httpPort: 80 # Optional. Default: 80
```

# Prometheus Scraping Endpoint
Promitor automatically scrapes Azure Monitor and makes the information available based on the metrics configuration.

The behavior of this can be configured to fit your needs:
- `prometheus.metricUnavailableValue` - Defines the value that will be reported if a metric is unavailable. (Default: `NaN`) 
- `prometheus.enableMetricTimestamps` - Defines whether or not a timestamp should be included when the value was scraped on Azure Monitor. Supported values are `True` to opt-in & `False` to opt-out. (Default: `true`) 
- `prometheus.scrapeEndpoint.baseUriPath` - Controls the path where the scraping endpoint for Prometheus is being exposed.  (Default: `/metrics`)

Example:
```yaml
prometheus:
  metricUnavailableValue: NaN # Optional. Default: NaN
  enableMetricTimestamps: false # Optional. Default: true
  scrapeEndpoint:
    baseUriPath: /metrics # Optional. Default: /metrics
```

# Metric Configuration
Promitor will scrape the Azure Monitor metrics that are configured via a metric declaration YAML.

The behavior of this is configurable:

- `metricsConfiguration.absolutePath` - Defines the location of the YAML file that declares what Azure Monitor metrics to scrape. (Default: `/config/metrics-declaration.yaml`)

Example:
```yaml
metricsConfiguration:
  absolutePath: /config/metrics-declaration.yaml # Optional. Default: /config/metrics-declaration.yaml
```

# Telemetry
We provide insights in how our runtime is doing and is written to one or more sinks.

You can determine what telemetry sinks you want and what the default verbosity should be via the runtime YAML.

General telemetry information can be configured:
- `telemetry.defaultVerbosity`- Defines the default minimum log level that should be logged if a sink does not provide one. Allowed values are `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`, `None` ordered from most to least verbose. (Default: `Error`)

To learn more about the configured sinks and their configuration, see [**"Telemetry Sinks"**](#telemetry-sinks).

Example:

```yaml
telemetry:
  applicationInsights:
    # [...]
  containerLogs:
    # [...]
  defaultVerbosity: error # Optional. Default: error
```

## Telemetry Sinks
Promitor provides the telemetry, but it's up to you to choose where you want to send it to.

We currently support the following sinks:
- [**Container Logs** (stdout/stderr)](#container-logs)
- [**Azure Application Insights**](#azure-application-insights)

### Container Logs
Promitor can send telemetry to `stdout`/`stderr`.

In order to enable use this sink, the following configuration needs to be provided:
- `telemetry.containerLogs.isEnabled` - Determines if the sink is used or not. (Default: `true`)
- `telemetry.containerLogs.verbosity` - Verbosity to use for this sink, if not specified then the `telemetry.defaultVerbosity` will be used. (Optional)

Example:

```yaml
telemetry:
  containerLogs:
    isEnabled: true # Optional. Default: true
    verbosity: trace # Optional. Default: N/A
  defaultVerbosity: error # Optional. Default: error
```

### Azure Application Insights
Promitor can send telemetry to Azure Application Insights when there is a need to.

It currently supports:
- Exceptions during scraping

In order to enable use this sink, the following configuration needs to be provided:

- `telemetry.applicationInsights.isEnabled` - Determines if the sink is used or not. (Default: `true`)
- `telemetry.applicationInsights.verbosity` - Verbosity to use for this sink, if not specified then the `telemetry.defaultVerbosity` will be used. (Optional)
- `telemetry.applicationInsights.instrumentationKey` - Defines the instrumentation key to use when sending telemetry to Azure Application Insights

Example:

```yaml
telemetry:
  applicationInsights:
    instrumentationKey: ABC # Optional. Note: Required to be specified when turned on
    isEnabled: false # Optional. Default: false
    verbosity: trace # Optional. Default: N/A
  containerLogs:
    isEnabled: true # Optional. Default: true
    verbosity: trace # Optional. Default: N/A
  defaultVerbosity: error # Optional. Default: error
```

# Overriding configuration with environment variables

In certain scenarios you'd like to override what was configured in the runtime YAML. Therefor we provide the capability to override them via environment variables.

Every environment variable should be prefixed with `PROMITOR_YAML_OVERRIDE_` followed by the YAML hierarchy where every level is replaced with `__` rather than a tab. Environment variables are not case sensitive.

Our runtime configuration API endpoint allows you to verify if it was overriden and returns what will be used to run Promitor.

> :warning: Depending on the configuration that is changed it may be required to restart Promitor, for example changing the HTTP port.

## Example

Let's say we want to override the following HTTP port:
```yaml
server:
  httpPort: 80
```

An environment variable called `PROMITOR_YAML_OVERRIDE_server__httpPort` can be provided which specifies the new port.

[&larr; back](/)
