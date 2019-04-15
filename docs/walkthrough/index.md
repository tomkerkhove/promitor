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
  "id": "/subscriptions/<guid>/resourceGroups/PromitorRG",
  ...
}
```

## Create a Service Principal

Use the resource group creation output to add a scope to your service principal:

```bash
az ad sp create-for-rbac --role="Monitoring Reader" \
  --scopes="/subscriptions/<guid>/resourceGroups/PromitorRG"
```

Which should output something similar to

```json
{
  "appId": "<guid-app-id>",
  "displayName": "azure-cli-2019-03-29-19-21-58",
  "name": "http://azure-cli-2019-03-29-19-21-58",
  "password": "<guid-generated-password>",
  "tenant": "<guid-tenant-id>"
}
```

Save this output - the app ID, tenant ID, and password will be used later.

## Create a Service Bus Namespace and Queue

Before we install Promitor on AKS to start getting the metrics you want out of Azure Monitor, we need to set up the Azure Resource you want to monitor and have it ready to get metrics for (i.e Service Bus, Azure Functions, VM etc.). In this walkthrough we are going to set up a Service Bus as an example. (ref : https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-create-namespace-portal)

1. Sign in to the Azure portal (https://portal.azure.com)
2. In the left navigation pane of the portal, select + Create a resource, select Integration, and then select Service Bus.

![Alt text](https://docs.microsoft.com/en-us/azure/includes/media/service-bus-create-namespace-portal/create-resource-service-bus-menu.png "Service Bus via Portal")

## Create AKS Cluster

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

1. [Install Helm](https://helm.sh/docs/using_helm/#installing-helm) if it isn't already set up
2. You'll use Helm to install Tiller, the server-side component of Helm. For clusters with RBAC (Role-Based Access Control), you'll need to set up a service account for Tiller:
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

* Create the service account and role binding with the kubectl apply command:
```bash
kubectl apply -f helm-rbac.yaml
```

* To deploy a basic Tiller into an AKS cluster, use the helm init command 
```bash
helm init --service-account tiller
```

## Deploy Promitor Helm Chart

```yaml
azureMetadata:
  tenantId: <azure-ad-app-id>
  subscriptionId: <azure-ad-app-key>
  resourceGroupName: <resource-group-name>
metricDefaults:
  aggregation:
    interval: 00:05:00
  scraping:
    schedule: "* * * * *"
metrics:
  - name: demo_queue_size
    description: "Amount of active messages of the <queue-name> queue"
    resourceType: ServiceBusQueue
    namespace: <service-bus-namespace>
    queueName: <queue-name>
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
        interval: 00:01:00
```

```bash
helm install ./charts/promitor-scraper \
  --set azureAuthentication.appId='<azure-ad-app-id>' \
  --set azureAuthentication.appKey='<azure-ad-app-key>' \
  --values /path/to/metric-declaration.yaml
```

should give you an output that includes a script similar to this one:

```bash
cat > promitor-scrape-config.yaml <<EOF
extraScrapeConfigs: |
  - job_name: punk-rattlesnake-promitor-scraper
    metrics_path: /prometheus/scrape
    static_configs:
      - targets:
        - punk-rattlesnake-promitor-scraper.default.svc.cluster.local:80
EOF
helm install stable/prometheus -f promitor-scrape-config.yaml
```

(You can see this output again at any time by running `helm status <release-name>`.)

Running those will create a Prometheus scraping configuration file & deploy Prometheus to your cluster with that scraping configuration in addition to the default. From there, run `kubectl port-forward svc/<prometheus-release-name>-prometheus-server 9090:80`. This will allow you to view the Prometheus server at http://localhost:9090. There, you should be able to query $demo_queue_size and see a result (once all pods are up & Promitor has scraped metrics at least once - run `kubectl get pods` to see the status of your pods). 

## Add load to the Queue

## See promitor scraping via port-forwarding

## Prometheus install

## Prometheus alert?

## Grafana install

## Grafana dashboard

[&larr; back](/)