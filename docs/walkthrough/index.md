---
layout: default
title: Walkthroughs & Tutorials
---

## Create a Resource Group

Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) if you don't already have it.

```bash
az group create --name PromitorRG --location eastus
```

Output: 
```	json
{ 
  "id": "/subscriptions/<guid-subscription-id>/resourceGroups/PromitorRG",
  ...
}
```

## Create a Service Principal

Use the resource group creation output to add a scope to your service principal:

```bash
az ad sp create-for-rbac --role="Monitoring Reader" \
  --scopes="/subscriptions/<guid-subscription-id>/resourceGroups/PromitorRG"
```

Which should output something similar to

```json
{
  "appId": "<guid-sp-app-id>",
  "displayName": "azure-cli-2019-03-29-19-21-58",
  "name": "http://azure-cli-2019-03-29-19-21-58",
  "password": "<guid-sp-generated-password>",
  "tenant": "<guid-tenant-id>"
}
```

Save this output - the app ID, tenant ID, and password will be used later.

## Create a Service Bus Namespace and Queue

Before we install Promitor on AKS to start getting the metrics you want out of Azure Monitor, we need to set up the Azure Resource you want to monitor and have it ready to get metrics for (i.e Service Bus, Azure Functions, VM etc.). In this walkthrough we are going to set up a Service Bus as an example. (ref : https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal)

1. Sign in to the Azure portal (https://portal.azure.com)
2. In the left navigation pane of the portal, select + Create a resource, select Integration, and then select Service Bus.

![Alt text](https://docs.microsoft.com/en-us/azure/includes/media/service-bus-create-namespace-portal/create-resource-service-bus-menu.png "Service Bus via Portal")

## Create an AKS Cluster

(From the [AKS Cluster Quickstart](https://docs.microsoft.com/en-us/azure/aks/kubernetes-walkthrough#create-aks-cluster))

```bash
az aks create \
  --name PromitorCluster \
  --resource-group PromitorRG \
  --node-count 1 \
  --generate-ssh-keys
```	

If the Kubernetes command line tool `kubectl` isn't already installed, you can install it with 

```bash
az aks install-cli
```

Then get your cluster's credentials with

```bash
az aks get-credentials \
  --name PromitorCluster \
  --resource-group PromitorRG
```

Verify your credentials and check that your cluster is up and running with `kubectl get nodes`.


## Install Helm and Tiller

[Install Helm](https://helm.sh/docs/using_helm/#installing-helm) on your local machine if you don't already have it.

You'll use Helm to install Tiller, the server-side component of Helm. For clusters with RBAC (Role-Based Access Control), you'll need to set up a service account for Tiller:
- Create a file called `helm-rbac.yaml` with the following:

```YAML
apiVersion: v1
kind: ServiceAccount
metadata:
  name: tiller
  namespace: kube-system
---
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: tiller
roleRef:
  apiGroup: rbac.authorization.k8s.io
  kind: ClusterRole
  name: cluster-admin
subjects:
  - kind: ServiceAccount
    name: tiller
    namespace: kube-system
```

- Create the service account and role binding specified in the above file with the `kubectl apply` command:

```bash
kubectl apply -f helm-rbac.yaml
```

- To deploy Tiller into your AKS cluster, use the `helm init` command:

```bash
helm init --service-account tiller
```

## Deploy Promitor to your cluster using Helm

In addition to the helm chart, you'll need a values file with secrets & a metric declaration file (these can also be the same file for ease of use). The yaml file below will scrape one metric, queue length, from the queue created above.

```yaml
azureAuthentication:
  appId: <guid-sp-app-id>
  appKey: <guid-sp-generated-password>

azureMetadata:
  tenantId: <guid-tenant-id>
  subscriptionId: <guid-subscription-id>
  resourceGroupName: PromitorRG
metricDefaults:
  aggregation:
    interval: 00:05:00
  scraping:
    schedule: "* * * * *"
metrics:
  - name: demo_queue_size
    description: "Amount of active messages of the 'demo_queue' queue"
    resourceType: ServiceBusQueue
    namespace: <service-bus-namespace>
    queueName: demo_queue
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
```

With this file created, you should be able to deploy Promitor with `helm install`. You'll need to be in the top level directory of the Promitor repository for this command to run, or you'll need to edit the chart location.

```bash
helm install ./charts/promitor-agent-scraper \
  --name promitor-agent-scraper \
  --values your/path/to/metric-declaration.yaml
```

## Prometheus install

Running the deployment command from the previous section should give you an output that includes a script similar to this one:

```bash
cat > promitor-scrape-config.yaml <<EOF
extraScrapeConfigs: |
  - job_name: promitor-agent-scraper
    metrics_path: /metrics
    static_configs:
      - targets:
        - promitor-agent-scraper.default.svc.cluster.local:80
EOF
helm install stable/prometheus -f promitor-scrape-config.yaml
```

(You can see this output again at any time by running `helm status promitor-agent-scraper`.)

Running these commands will create a Prometheus scraping configuration file in your current directory and deploy Prometheus to your cluster with that scraping configuration in addition to the default.

## Add load to the Queue

- service bus queue explorer

## See promitor scraping via port-forwarding

From there, run `kubectl port-forward svc/<prometheus-release-name>-prometheus-server 9090:80`. This will allow you to view the Prometheus server at http://localhost:9090. There, you should be able to query `demo_queue_size` and see a result (once all pods are up and Promitor has scraped metrics at least once - run `kubectl get pods` to see the status of your pods). 

## Prometheus alert?

## Grafana install

## Grafana dashboard

[&larr; back](/)