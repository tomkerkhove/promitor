# Resource Discovery Runtime Configuration

This article covers an overview of all the knobs that you can tweak to align the
Resource Discovery runtime with your needs.

Promitor Resource Discovery runtime is configured by mounting the configuration to a volume.

Depending on the operating system, it need to be available on :

- `/config/runtime.yaml` for Linux
- `c:/config/runtime.yaml` for Windows

We provide the capability to override te runtime YAML via [environment variables](#overriding-configuration-with-environment-variables),
if you have the need for it.

Here is a complete example of the runtime YAML:

```yaml
authentication:
  # Options are ServicePrincipal, SystemAssignedManagedIdentity, UserAssignedManagedIdentity.
  mode: ServicePrincipal # Optional. Default: ServicePrincipal.
  identityId: xxxx-xxxx-xxxx # Optional.
server:
  httpPort: 80 # Optional. Default: 80
cache:
  enabled: true # Optional. Default: true
  durationInMinutes: 5 # Optional. Default: 5
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

## Authentication

The Promitor runtime allows you to use various ways to authenticate to Azure:

- `authentication.mode` - Defines authentication mode to use. Options are `ServicePrincipal`,
 `SystemAssignedManagedIdentity`, `UserAssignedManagedIdentity`. _(defaults to service principle)_
- `authentication.identityId` - Id of the Azure AD entity to authenticate with when integrating with Microsoft Azure.
 Required when using `ServicePrincipal` or `UserAssignedManagedIdentity`.

Example:

```yaml
authentication:
  # Options are ServicePrincipal, SystemAssignedManagedIdentity, UserAssignedManagedIdentity.
  mode: ServicePrincipal # Optional. Default: ServicePrincipal.
  identityId: xxxx-xxxx-xxxx # Optional.
```

## Runtime

The Promitor runtime is flexible and allows you to configure it to meet your needs:

- `server.httpPort` - Defines the port to serve HTTP traffic _(default 80)_

Example:

```yaml
server:
  httpPort: 80 # Optional. Default: 80
```

## Cache

The Promitor runtime allows you to cache discovered resources to optimize for performance and avoid hitting Azure throttling.

You can configure how the cache should behave:

- `cache.enabled` - Indication whether or not discovered resources should be cached in-memory. _(default true)_
- `cache.durationInMinutes` - Amount of minutes to cache discovered resources. _(default 5)_

Example:

```yaml
cache:
  enabled: true # Optional. Default: true
  durationInMinutes: 5 # Optional. Default: 5
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
