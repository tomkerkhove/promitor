# Deploying Promitor, Prometheus, and Grafana on an AKS Cluster

## Introduction

In this walkthrough, we'll set up a basic monitoring solution with Promitor,
Prometheus, and Grafana.

In order to have a resource to monitor, we'll create a Service Bus queue and add
load to the queue with Service Bus Explorer.

We'll deploy Promitor, Prometheus, and Grafana to a Kubernetes cluster using Helm,
and explain how each of these services connects and how to see output.

We'll also walk through setting up basic Grafana dashboard to visualize the metrics
we're monitoring.

## Table of Contents

- **[Deploy Azure Infrastructure](#deploy-azure-infrastructure)**
  - [Create a Resource Group](#create-a-resource-group)
  - [Create a Service Principal](#create-a-service-principal)
  - [Create a Service Bus Namespace and Queue](#create-a-service-bus-namespace-and-queue)
  - [Create an AKS Cluster](#create-an-aks-cluster)
- **[Cluster Setup](#cluster-setup)**
  - [Get credentials](#get-credentials)
- **[Deploy Promitor and Prometheus](#deploy-promitor-and-prometheus)**
  - [Create a metrics declaration for Promitor](#create-a-metrics-declaration-for-promitor)
  - [Deploy Promitor to your cluster using Helm](#deploy-promitor-to-your-cluster-using-helm)
  - [Install Prometheus](#install-prometheus)
- **[Test and check output](#test-and-check-output)**
  - [Add load to the queue](#add-load-to-the-queue)
  - [See Promitor & Prometheus output via port-forwarding](#see-promitor-&-prometheus-output-via-port-forwarding)
- **[Visualization](#visualization)**
  - [Install Grafana](#install-grafana)
  - [Add Prometheus as a data source](#add-prometheus-as-a-data-source)
  - [Create a Grafana dashboard for queue metrics](#create-a-grafana-dashboard-for-queue-metrics)
  - [Creating a Kubernetes dashboard](#creating-a-kubernetes-dashboard)
- **[Delete resources](#delete-resources)**

## Prerequisites

- The [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
- [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/), the Kubernetes
  command-line tool. It can also be installed via the Azure CLI with `az aks install-cli`.
- [Helm](https://helm.sh/docs/using_helm/#installing-the-helm-client), a Kubernetes
  deployment manager
- [Service Bus Explorer](https://github.com/paolosalvatori/ServiceBusExplorer/releases)

## Deploy Azure Infrastructure

### Create a Resource Group

```bash
az group create --name PromitorRG --location eastus
```

Output:

```json
{
  "id": "/subscriptions/<guid-subscription-id>/resourceGroups/PromitorRG",
  "...": "..."
}
```

### Create a Service Principal

Use the resource group creation output to add a scope to your service principal:

```bash
az ad sp create-for-rbac \
  --role="Monitoring Reader" \
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

Save this output as we will use the app ID, tenant ID, and password later on.

### Create a Service Bus Namespace and Queue

First we'll need to create a namespace. Service Bus Namespaces need to be globally
unique, so we won't use a default name in these commands.

```bash
az servicebus namespace create \
  --resource-group PromitorRG \
  --name <service-bus-namespace> \
  --location eastus
```

We'll then create a queue in that namespace:

```bash
az servicebus queue create \
  --resource-group PromitorRG \
  --namespace-name <service-bus-namespace> \
  --name demo_queue
```

Finally, get the connection string for this Service Bus namespace for use later.

```bash
az servicebus namespace authorization-rule keys list \
  --resource-group PromitorRG \
  --namespace-name <service-bus-namespace> \
  --name RootManageSharedAccessKey \
  --query primaryConnectionString \
  --output tsv
```

### Create an AKS Cluster

Create a cluster with:

```bash
az aks create \
  --name PromitorCluster \
  --resource-group PromitorRG \
  --node-count 1 \
  --generate-ssh-keys
```

## Cluster Setup

### Get credentials

You can get your cluster's credentials with

```bash
az aks get-credentials \
  --name PromitorCluster \
  --resource-group PromitorRG
```

This will save these credentials to your kubeconfig file and set your new cluster
as your current context for all `kubectl` commands.

Verify your credentials and check that your cluster is up and running with
`kubectl get nodes`.

## Deploy Promitor and Prometheus

### Create a metrics declaration for Promitor

Before deploying Promitor, you'll need a values file with secrets & a metric declaration
file (these can also be the same file for ease of use). The yaml below will scrape
one metric, queue length, from the queue created above.

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
    resourceType: ServiceBusNamespace
    azureMetricConfiguration:
      metricName: ActiveMessages
      aggregation:
        type: Total
    resources:
      - namespace: <service-bus-namespace>
        queueName: demo_queue
```

### Deploy Promitor to your cluster using Helm

To deploy, we'll first add the Promitor chart repository to helm:

```bash
helm repo add promitor https://charts.promitor.io/
helm repo update
```

With this repository added, we can deploy Promitor:

```bash
helm install promitor-agent-scraper promitor/promitor-agent-scraper \
  --values your/path/to/metric-declaration.yaml
```

### Install Prometheus

**Note: If you're seeing errors installing Prometheus or Grafana from the Helm**
**chart repository, make sure you run `helm repo update` before digging into the**
**errors more. You might have an outdated copy of the chart.**

Running the deployment command from the previous section should give you an output
that includes a script similar to this one:

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

You can see this output again at any time by running `helm status promitor-agent-scraper`.

Running these commands will create a Prometheus scraping configuration file in your
current directory and deploy Prometheus to your cluster with that scraping configuration
in addition to the default.

## Test and check output

### Add load to the queue

Now we'll use Service Bus Explorer to add load to our Service Bus queue so there
are meaningful metrics for Promitor to pick up.

In Service Bus Explorer, you can connect to your namespace & queue using a connection
string. From there, right clicking on the queue in the side-bar should give you
an option to 'Send Message' - from there, use the 'Sender' tab of that window to
send bulk messages. Remember how many you send - you should see that number in the
Promitor & Prometheus output.

### See Promitor & Prometheus output via port-forwarding

Going back to your cluster, you should be able to see all Promitor & Prometheus
pods up and running with `kubectl get pods`.

You can also see the services which provide a stable endpoint at which to reach
the pods by running `kubectl get services`.

This should give you a list with output similar to:

| NAME | TYPE | CLUSTER-IP | EXTERNAL-IP | PORT(S) |
| ---- | ---- | ---------- | ----------- | ------- |
| promitor-agent-scraper | ClusterIP | 10.0.#.# | \<none\> | 80/TCP |
| \<prometheus-release-name\>-prometheus-server | ClusterIP | 10.0.#.# | \<none\> | 80/TCP |

Next to that, it should also list other services deployed by Prometheus.

Let's first look at the Promitor output!

Run `kubectl port-forward svc/promitor-agent-scraper 8080:80` and check
<http://localhost:8080/metrics>. You should see some information about your queue:

```bash
# HELP promitor_ratelimit_arm Indication how many calls are still available before Azure Resource Manager is going to throttle us.
# TYPE promitor_ratelimit_arm gauge
promitor_ratelimit_arm{tenant_id=<guid-tenant-id>,subscription_id=<guid-subscription-id>,app_id=<guid-sp-app-id>} 11998 1558116465529
# HELP demo_queue_size Amount of active messages of the 'demo_queue' queue
# TYPE demo_queue_size gauge
demo_queue_size 200 1558116465677
```

where 200 is the number of messages sent.

We can also look at the Prometheus server and check that it's pulling in metrics
from Promitor.

Cancel the previous port-forward command and run
`kubectl port-forward svc/<prometheus-release-name>-prometheus-server 8080:80`.

Now, if you check <http://localhost:8080>, you should be able to enter Prometheus
queries.

Query `demo_queue_size` and as long as all your pods are up and running and both
Promitor and Prometheus have scraped metrics at least once, you should see a value
that matches the number of messages in your queue.

## Visualization

### Install Grafana

Grafana's chart has a few default values you may not want long term - persistant
storage is disabled and admin username/password is randomly generated - but for
our sample the out-of-the-box install will work.

Run `helm install grafana stable/grafana` and you should see output that
includes this command:

```shell
kubectl get secret --namespace default grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo
```

Run this to get your Grafana password.

Now you can use `kubectl port-forward` again to log in to your Grafana dashboard.
`kubectl port-forward svc/grafana 8080:80` will make your dashboard available at
<http://localhost:8080>, and you can log in with username 'admin' and the password
you retrieved.

### Add Prometheus as a data source

After logging in, you should see an option to "Add a Data Source." Click that,
and choose the Prometheus source type (if it's not immediately visible, search
for it).

The only setting you should need to edit here is the URL, under the HTTP section.
Within your cluster, `http://<prometheus-release-name>-prometheus-server.default.svc.cluster.local`
should resolve to the Prometheus server service. (Default in that URL refers to
the namespace - if you installed in a namespace other than default, change that.)

Set your service's name as the Prometheus URL in Grafana, and save the data source.
It should tell you that the data source is working.

### Create a Grafana dashboard for queue metrics

First, we'll make a basic dashboard with the metric we set up in Promitor. Then
you can use a pre-built dashboard to show Prometheus' default Kubernetes metrics.

Go to the + button on the sidebar, and choose "Dashboard". To make a simple graph
showing your queue size, you can write `demo_queue_size` in the query field. Click
out of that input field and the graph should update.

To see more, you can go back to Service Bus Explorer and send or receive messages.
Your Grafana graph won't update immediately, but you should see results in a few
minutes.

In order to see results without manually refreshing, find the dropdown menu in
the top right corner that sets the time range of the graph. Here you can edit time
range and refresh rate.

Make sure to save your new dashboard before exiting the page.

### Creating a Kubernetes dashboard

Now we'll import a pre-created dashboard that shows Kubernetes metrics. There are
multiple available on Grafana Lab's dashboard site - try [6417](https://grafana.com/dashboards/6417).

To import a dashboard, click the + button on the sidebar and choose "Import." From
there, you can either load a JSON file or enter the dashbord ID: 6417.

Click "Load" and you will be given some configuration options. The only one that
needs to be set is the Prometheus data source. From there, you should be able to
create the dashboard and view metrics about your AKS cluster.

## Delete resources

To delete all the resources used in this tutorial, run `az group delete --name PromitorRG`.

[&larr; back](/)
