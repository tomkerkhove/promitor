# Kubernetes

We provide a Helm Chart which deploys all the required infrastructure on your
Kubernetes cluster.

## Getting the Helm Chart

Install the Promitor Chart repository:

```shell
❯ helm repo add promitor https://charts.promitor.io/
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
```

## Using our Helm Chart

You can easily install our Resource Discovery Agent as following:

```shell
❯ helm install promitor-agent-resource-discovery promitor/promitor-agent-resource-discovery \
               --set azureAuthentication.appId='<azure-ad-app-id>' \
               --set azureAuthentication.appKey='<azure-ad-app-key>' \
               --values /path/to/helm-configuration.yaml
```

Next to Azure authentication, a [resource discovery declaration](https://promitor.io/configuration/v2.x/resource-discovery)
 must be provided through `--values`.

Here is an example of resource discovery declaration which you can pass:

```yaml
azureLandscape:
  cloud: Global
  tenantId: e0372f7f-a362-47fb-9631-74a5c4ba8bbf
  subscriptionIds:
  - 0329dd2a-59dc-4493-aa54-cb01cb027dc2
resourceDiscoveryGroups:
- name: api-gateways
  type: ApiManagement
```

Our Helm chart provides a variety of configuration options which you can explore in
 our [full values file](https://github.com/promitor/charts/blob/main/promitor-agent-resource-discovery/values.yaml).
to see all configurable values.

### Sample configuration

Want to get started easily? Here's a sample configuration to spin up the Resource Discovery agent which will be publicly
 exposed outside of the cluster on promitor-resource-discovery-sample.westeurope.cloudapp.azure.com:8889/api/docs/index.html.

```yaml
azureAuthentication:
  appId: 67882a00-21d3-4ee7-b32a-430ea0768cd3
  appKey: <hidden>
azureLandscape:
  cloud: Global
  tenantId: e0372f7f-a362-47fb-9631-74a5c4ba8bbf
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
