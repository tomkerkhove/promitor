---
layout: default
title: Deploying Promitor Resource Discovery
---

Here is an overview of how you can deploy Promitor Resource Discovery on your infrastructure, we support both Linux and Windows.

You can learn more about our Helm chart on [artifacthub.io](https://artifacthub.io/packages/helm/promitor/promitor-agent-resource-discovery).

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
URL                                                     CHART VERSION           APP VERSION     DESCRIPTION
https://hub.helm.sh/charts/promitor/promitor-ag...      2.0.0-preview-3         2.0.0-preview-3 Promitor, bringing Azure Monitor metrics where ...
https://hub.helm.sh/charts/promitor/promitor-ag...      0.0.0-pr997             0.0.0-pr997     A Helm chart to deploy Promitor, an Azure Monit...
https://hub.helm.sh/charts/promitor/promitor-ag...      0.0.0-0.0.0-pr1326      0.0.0-pr1326    Promitor, bringing Azure Monitor metrics where ...
```

### Using our Helm Chart

You can easily install our Resource Discovery Agent as following:

```shell
❯ helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --values /path/to/helm-configuration.yaml
```

Next to Azure authentication, a [resource discovery declaration](http://localhost:4000/configuration/v2.x/resource-discovery)
 must be provided through `--values`.

Here is an example of resource discovery declaration which you can pass:

```yaml
azureLandscape:
  cloud: Global
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionIds:
  - 0329dd2a-59dc-4493-aa54-cb01cb027dc2
resourceDiscoveryGroups:
- name: api-gateways
  type: ApiManagement
```

Our Helm chart provides a variety of configuration options which you can explore in
 our [full values file](https://github.com/tomkerkhove/promitor/blob/master/charts/promitor-agent-resource-discovery/values.yaml).
to see all configurable values.

#### Sample configuration

Want to get started easily? Here's a sample configuration to spin up the Resource Discovery agent which will be publicly
 exposed outside of the cluster on promitor-resource-discovery-sample.westeurope.cloudapp.azure.com:8888/api/docs/index.html.

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
    dnsPrefix: promitor-resource-discovery-sample
    enabled: true
telemetry:
  defaultLogLevel: information
```

You can easily deploy it by passing the file through `--values` during installation.

[&larr; back](/)
