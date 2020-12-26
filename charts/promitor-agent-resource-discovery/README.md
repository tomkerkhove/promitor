# Promitor

[Promitor](https://promitor.io/) is an Azure Monitor scraper which makes
 the metrics available for metric systems such as Atlassian Statuspage,
  Prometheus and StatsD.

## TL;DR

```console
helm repo add promitor https://promitor.azurecr.io/helm/v1/repo
helm repo update
helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery
```

## Introduction

This chart bootstraps a **Promitor Scraper Agent** deployment on a Kubernetes cluster
using the Helm package manager. It will provide the scraper agent with a Kubernetes
Service so that other Pods can consume it.

## Prerequisites

- Kubernetes v1.9 or above
- Azure Subscription
- Azure AD Application to authenticate with ([docs](https://promitor.io/configuration/#authentication-with-azure-monitor))

## Installing the Chart

To install the chart with the release name `promitor-agent-resource-discovery`:

```console
$ helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --values /path/to/metric-declaration.yaml
```

The command deploys Prometheus on the Kubernetes cluster with the specified metrics
declaration, for more information see [our documentation](https://promitor.io/deployment/#using-our-helm-chart).

## Uninstalling the Chart

To uninstall/delete the `promitor-agent-resource-discovery` deployment:

```console
helm uninstall promitor-agent-resource-discovery
```

The command removes all the Kubernetes components associated with the chart and
deletes the release.

## Configuration

The following table lists the configurable parameters of the Promitor chart and
their default values.

| Parameter                  | Description              | Default              |
|:---------------------------|:-------------------------|:---------------------|
| `image.repository`  | Repository which provides the image | `tomkerkhove/promitor-agent-resource-discovery` |
| `image.tag`  | Tag of image to use | None, chart app version is used by default            |
| `image.pullPolicy`  | Policy to pull image | `Always`            |
| `azureLandscape.cloud`  | Azure Cloud to discover resources in. Options are `Global` (default), `China`, `UsGov` & `Germany` | `Global`            |
| `azureLandscape.tenantId`  | Id of Azure tenant to discover resources in |             |
| `azureLandscape.subscriptionIds`  | List of Azure subscription ids to discover resources in | `[]`            |
| `resourceDiscoveryGroups`  | List of resource discovery groups to configured following the [resource discovery declaration docs](https://promitor.io/configuration/v2.x/resource-discovery) |        |
| `azureAuthentication.appId`  | Id of the Azure AD entity to authenticate with |             |
| `azureAuthentication.appKey`  | Secret of the Azure AD entity to authenticate with |             |
| `cache.enabled`  | Indication whether or not discovered resources should be cached in-memory to avoid Azure throttling | `true`            |
| `cache.durationInMinutes`  | Amount of minutes to cache discovered resources | `5`            |
| `telemetry.applicationInsights.enabled`  | Indication whether or not to send telemetry to Azure Application Insights | `false`            |
| `telemetry.applicationInsights.logLevel`  | Minimum level of logging for Azure Application Insights |             |
| `telemetry.applicationInsights.key`  | Application Insights instrumentation key |             |
| `telemetry.containerLogs.enabled`  | Indication whether or not to send telemetry to container logs | `true`            |
| `telemetry.containerLogs.logLevel`  | Minimum level of logging for container logs |  |
| `telemetry.defaultLogLevel`  | Minimum level of logging for all telemetry sinks, unless specified otherwise | `Error`            |
| `rbac.create` | If true, create & use RBAC resources | `true` |
| `rbac.podSecurityPolicyEnabled` | Create pod security policy resources | `false` |
| `rbac.serviceAccount.create` | Create service account resource | `true` |
| `rbac.serviceAccount.name` | Service account name to use if create is false. If create is true, a name is generated using the fullname template | `promitor-resource-discovery` |
| `rbac.serviceAccount.annotations` | Service account annotations| `{}` |
| `health.readiness.enabled`  | Indication if readiness probes should be used | `true`            |   |
| `health.readiness.verifyDependencies`  | Indication if readiness probes should verify if Promitor can interat with external dependencies. Do note that this will contact all dependencies which can have performance impact, cause throttling or cascading failures when consumed very often. | `false`            |   |
| `health.readiness.delay`  | Amount of seconds to wait before probing the container to verify if it's ready | `5`            |   |
| `health.readiness.interval`  | Amount of seconds to wait before probing the container again to verify if it's ready after the last attempt | `5`            |   |
| `health.liveness.enabled`  | Indication if liveness probes should be used | `true`            |   |
| `health.liveness.verifyDependencies`  | Indication if liveness probes should verify if Promitor can interat with external dependencies. Do note that this will contact all dependencies which can have performance impact, cause throttling or cascading failures when consumed very often. | `false`            |   |
| `health.liveness.delay`  | Amount of seconds to wait before probing the container to verify if it's still alive | `5`            |   |
| `health.liveness.interval`  | Amount of seconds to wait before probing the container again to verify if it's still alive after the last attempt | `30`            |   |
| `resources`  | Pod resource requests & limits |    `{}`    |
| `secrets.createSecret`  | Indication if you want to bring your own secret level of logging | `true`            |   |
| `secrets.appKeySecret`  | Name of the secret for Azure AD identity secret | `azure-app-key`            |
| `service.loadbalancer.enabled`  | Indication whether or not to expose service externally through a load balancer | `false`            |
| `service.loadbalancer.azure.dnsPrefix`  | Prefix for DNS name to expose the service on using `<name>.<location>.cloudapp.azure.com` format. This setting is specific to Azure Kubernetes Service ([docs](https://docs.microsoft.com/en-us/azure/aks/static-ip#apply-a-dns-label-to-the-service)) | ``            |
| `service.loadbalancer.azure.exposeInternally`  | To restrict access to Promitor by exposing it through an internal load balancer. This setting is specific to Azure Kubernetes Service ([docs](https://docs.microsoft.com/en-us/azure/aks/internal-lb)) | `false`            |
| `service.port`  | Port on service for other pods to talk to | `8889`            |
| `service.targetPort`  | Port on container to serve traffic | `88`            |

Specify each parameter using the `--set key=value[,key=value]` argument to
`helm install`. For example:

```console
$ helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --set azureLandscape.tenantId='<azure-tenant-id>' \
               --set azureLandscape.subscriptionId='<azure-subscription-id>' \
               --values C:\Promitor\metric-declaration.yaml
```

Alternatively, a YAML file that specifies the values for the above parameters can
be provided while installing the chart. For example,

```console
helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery -f values.yaml
```
