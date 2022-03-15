## Kubernetes

We provide a Helm Chart which deploys all the required infrastructure on your
Kubernetes cluster.

### Getting the Helm Chart

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
URL                                                     CHART VERSION   APP VERSION     DESCRIPTION
https://hub.helm.sh/charts/promitor/promitor-ag...      1.6.0           1.6.1           A Helm chart to deploy Promitor, an Azure Monit...
```

### Using our Helm Chart

To use this, you will need to provide parameters [via `--set` or `--values`](https://helm.sh/docs/using_helm/#customizing-the-chart-before-installing).
Included here are the values that correspond with the local environment variables.
In addition to these, you will need a metric declaration file as described in
[Metric Declaration](/configuration/metrics).

```yaml
azureMetadata:
  tenantId: "<azure-tenant-id>"
  subscriptionId: "<azure-subscription-id>"

runtime:
  metricSinks:
    atlassianStatuspage:
      enabled: true
      pageId: "ABC"
      systemMetricMapping:
      - id: nfkgnrwpn545
        promitorMetricName: promitor_demo_appplan_percentage_cpu
    prometheusScrapingEndpoint:
      enabled: true
      baseUriPath: /metrics
      enableMetricTimestamps: True
    statsd:
      enabled: true
      host: graphite
      port: 8125
      metricPrefix: poc.promitor.
  telemetry:
    applicationInsights:
      enabled: True
      key: "<azure-app-insights-key>"

metrics:
  - name: promitor_demo_servicebusqueue_queue_size
    description: "Amount of active messages of the 'orders' queue (determined with ServiceBusQueue provider)"
    resourceType: ServiceBusQueue
    namespace: promitor-messaging
    queueName: orders
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Average
```

Check the [full values file](https://github.com/promitor/charts/blob/main/promitor-agent-scraper/values.yaml)
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
