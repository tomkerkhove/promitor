---
layout: default
title: Deploying Promitor Resource Discovery
---

Here is an overview of how you can deploy Promitor Resource Discovery on your infrastructure, we support both Linux and Windows.

## Docker

```shell
❯ docker run -d -p 9999:80 --name promitor-agent-resource-discovery   \
                         --env PROMITOR_AUTH_APPID='<azure-ad-app-id>'   \
                         --env-file C:/Promitor/promitor-discovery-auth.creds   \
                         --volume C:/Promitor/resource-discovery-declaration.yaml:/config/resource-discovery-declaration.yaml   \
                         --volume C:/Promitor/resource-discovery-runtime.yaml:/config/runtime.yaml   \
                         tomkerkhove/promitor-agent-resource-discovery:0.1.0-preview-1
```

## Kubernetes

We provide a Helm Chart which deploys all the required infrastructure on your
Kubernetes cluster.

### Getting the Helm Chart

Install the Promitor Chart repository:

```shell
❯ helm repo add promitor https://promitor.azurecr.io/helm/v1/repo
```

Refresh your local Chart repositories:

```shell
❯ helm repo update
```

If all goes well you should be able to list all Promitor charts:

```shell
❯ helm search hub promitor
URL                                                     CHART VERSION   APP VERSION     DESCRIPTION
https://hub.helm.sh/charts/promitor/promitor-ag...      1.6.0           1.6.1           A Helm chart to deploy Promitor, an Azure Monit...
https://hub.helm.sh/charts/promitor/promitor-ag...      0.0.0-PR920     0.0.0-PR920     A Helm chart to deploy Promitor, an Azure Monit...
```

### Using our Helm Chart

To use this, you will need to provide parameters [via `--set` or `--values`](https://helm.sh/docs/using_helm/#customizing-the-chart-before-installing).
Included here are the values that correspond with the local environment variables.
In addition to these, you will need a metric declaration file as described in
[Metric Declaration](/configuration/metrics).

```yaml
azureAuthentication:
  appId: 67882a00-21d3-4ee7-b32a-430ea0768cd3
  appKey: <hidden>
azureLandscape:
  cloud: Global
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionIds:
  - 0329dd2a-59dc-4493-aa54-cb01cb027dc2
  - 0f9d7fea-99e8-4768-8672-06a28514f77e
resourceDiscoveryGroups:
- name: service-bus-landscape
  type: ServiceBusQueue
- name: api-gateways
  type: ApiManagement
- name: app-plan-landscape
  type: AppPlan
- name: container-instances
  type: ContainerInstance
- name: container-registry-landscape
  type: ContainerRegistry
- name: cosmos-accounts
  type: CosmosDb
- name: dps
  type: DeviceProvisioningService
- name: event-hubs-landscape
  type: EventHubs
service:
  loadBalancer:
    dnsPrefix: promitor-resource-discovery
    enabled: true
telemetry:
  defaultLogLevel: information
```

Check the [full values file](https://github.com/tomkerkhove/promitor/blob/master/charts/promitor-agent-resource-discovery/values.yaml)
to see all configurable values.

If you have a `metric-declaration.yaml` file, you can create a basic deployment
with this command:

```shell
❯ helm install promitor-agent-scraper promitor/promitor-agent-scraper \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --values /path/to/helm-configuration.yaml
```

[&larr; back](/)
