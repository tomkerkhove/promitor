---
layout: default
title: Runtime Configuration
---

This article covers an overview of all the knobs that you can tweak to align the
runtime with your needs.

Promitor runtime is configured by mounting the configuration to a volume.

Depending on the operating system, it need to be available on :

- `/config/runtime.yaml` for Linux
- `c:/config/runtime.yaml` for Windows

We provide the capability to override te runtime YAML via [environment variables](#overriding-configuration-with-environment-variables),
if you have the need for it.

Here is a complete example of the runtime YAML:

```yaml
server:
  httpPort: 80 # Optional. Default: 80
metricSinks:
  statsd:
    host: graphite
    port: 8125 # Optional. Default: 8125
    metricPrefix: promitor. # Optional. Default: None
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

_Note: Using Promitor v0.x? [Use environment variables](./../v0.x/) to configure
the runtime._

## Runtime

The Promitor runtime is flexible and allows you to configure it to meet your needs:

- `server.httpPort` - Defines the port to serve HTTP traffic _(default 80)_

Example:

```yaml
server:
  httpPort: 80 # Optional. Default: 80
```

## Metric Sinks

Promitor automatically scrapes Azure Monitor and makes the information available
by providing the metric information to the configured sinks.

As of today, we support the follow sinks:

- **Prometheus Scraping Endpoint**
- **StatsD**

### Prometheus Scraping Endpoint

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.6-green.svg)

In order to expose a Prometheus Scraping endpoint, you'll need to configure the sink:

- `prometheusScrapingEndpoint.metricUnavailableValue` - Defines the value that will be reported
  if a metric is unavailable. (Default: `NaN`)
- `prometheusScrapingEndpoint.enableMetricTimestamps` - Defines whether or not a timestamp should
  be included when the value was scraped on Azure Monitor. Supported values are
  `True` to opt-in & `False` to opt-out. (Default: `true`)
- `prometheusScrapingEndpoint.scrapeEndpoint.baseUriPath` - Controls the path where the scraping
  endpoint for Prometheus is being exposed.  (Default: `/metrics`)

```yaml
metricSinks:
  prometheusScrapingEndpoint:
    metricUnavailableValue: NaN # Optional. Default: NaN
    enableMetricTimestamps: false # Optional. Default: true
    baseUriPath: /metrics # Optional. Default: /metrics
```

### StatsD

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.6-green.svg)

In order to push metrics to a StatsD server, you'll need to configure the sink:

- `metricSinks.statsd.host` - DNS name or IP address of StatsD server.
- `metricSinks.statsd.host` - Port (UDP) address of StatsD server. (Default: `8125`)
- `metricSinks.statsd.metricPrefix` - Prefix that will be added to every metric
 defined in the metric declaration.

```yaml
metricSinks:
  statsd:
    host: graphite
    port: 8125
    metricPrefix: promitor.
```

### Prometheus Scraping Endpoint - Legacy Configuration

![Availability Badge](https://img.shields.io/badge/Will%20Be%20Removed%20In-v2.0-red.svg)

For now, we still support configuring it by using the the old way of configuration:

```yaml
prometheus:
  metricUnavailableValue: NaN # Optional. Default: NaN
  enableMetricTimestamps: false # Optional. Default: true
  scrapeEndpoint:
    baseUriPath: /metrics # Optional. Default: /metrics
```

However, this approach is deprecated and will be removed in 2.0 so we recommend migrating to metric sink approach.

## Metric Configuration

Promitor will scrape the Azure Monitor metrics that are configured via a metric
declaration YAML.

The behavior of this is configurable:

- `metricsConfiguration.absolutePath` - Defines the location of the YAML file that
declares what Azure Monitor metrics to scrape. (Default: `/config/metrics-declaration.yaml`)

Example:

```yaml
metricsConfiguration:
  absolutePath: /config/metrics-declaration.yaml # Optional. Default: /config/metrics-declaration.yaml
```

## Telemetry

We provide insights in how our runtime is doing and is written to one or more sinks.

You can determine what telemetry sinks you want and what the default verbosity
should be via the runtime YAML.

General telemetry information can be configured:

- `telemetry.defaultVerbosity`- Defines the default minimum log level that should
  be logged if a sink does not provide one. Allowed values are `Trace`, `Debug`,
  `Information`, `Warning`, `Error`, `Critical`, `None` ordered from most to least
  verbose. (Default: `Error`)

To learn more about the configured sinks and their configuration, see
[**"Telemetry Sinks"**](#telemetry-sinks).

Example:

```yaml
telemetry:
  applicationInsights:
    # [...]
  containerLogs:
    # [...]
  defaultVerbosity: error # Optional. Default: error
```

### Telemetry Sinks

Promitor provides the telemetry, but it's up to you to choose where you want to
send it to.

We currently support the following sinks:

- [**Container Logs** (stdout/stderr)](#container-logs)
- [**Azure Application Insights**](#azure-application-insights)

#### Container Logs

Promitor can send telemetry to `stdout`/`stderr`.

In order to enable use this sink, the following configuration needs to be provided:

- `telemetry.containerLogs.isEnabled` - Determines if the sink is used or not.
  (Default: `true`)
- `telemetry.containerLogs.verbosity` - Verbosity to use for this sink, if not
  specified then the `telemetry.defaultVerbosity` will be used. (Optional)

Example:

```yaml
telemetry:
  containerLogs:
    isEnabled: true # Optional. Default: true
    verbosity: trace # Optional. Default: N/A
  defaultVerbosity: error # Optional. Default: error
```

#### Azure Application Insights

Promitor can send telemetry to Azure Application Insights when there is a need to.

It currently supports:

- Traces  ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1-green.svg)
- Exceptions

In order to enable use this sink, the following configuration needs to be provided:

- `telemetry.applicationInsights.isEnabled` - Determines if the sink is used or not.
  (Default: `true`)
- `telemetry.applicationInsights.verbosity` - Verbosity to use for this sink, if
  not specified then the `telemetry.defaultVerbosity` will be used. (Optional)
- `telemetry.applicationInsights.instrumentationKey` - Defines the instrumentation
  key to use when sending telemetry to Azure Application Insights

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

## Azure Monitor

Promitor interacts with Azure Monitor API to scrape all the required metrics.

During troubleshooting it can be interesting to gain insights on what the API returns, for which you can opt-in.

The behavior of this can be configured to fit your needs:

- `azureMonitor.logging.informationLevel` - Defines granularity of information that should be reported. Available
 options are `Basic`, `Headers`, `Body` & `BodyAndHeaders`. (Default: `Basic`)
- `azureMonitor.logging.isEnabled` - Defines whether or not information concerning the integration with Azure Monitor
 API. (Default: `false`)

Example:

```yaml
azureMonitor:
  logging:
    informationLevel: Basic # Optional. Default: Basic
    isEnabled: false # Optional. Default: false
```

_Note: All telemetry is emitted as verbose so you have to make sure `telemetry` is configured correctly._

## Overriding configuration with environment variables

In certain scenarios you'd like to override what was configured in the runtime YAML.
Therefore we provide the capability to override them via environment variables.

Every environment variable should be prefixed with `PROMITOR_YAML_OVERRIDE_` followed
by the YAML hierarchy where every level is replaced with `__` rather than a tab.
Environment variables are not case sensitive.

Our runtime configuration API endpoint allows you to verify if it was overriden
and returns what will be used to run Promitor.

> :warning: Depending on the configuration that is changed it may be required to
restart Promitor, for example changing the HTTP port.

### Example

Let's say we want to override the following HTTP port:

```yaml
server:
  httpPort: 80
```

An environment variable called `PROMITOR_YAML_OVERRIDE_server__httpPort` can be
provided which specifies the new port.

[&larr; back](/)
