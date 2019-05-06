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
```json
{ 
  "id": "/subscriptions/<guid-subscription-id>/resourceGroups/PromitorRG",
  "...": "..."
}
```

## Create a Service Principal

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

Save this output - the app ID, tenant ID, and password will be used later.

## Create a Service Bus Namespace and Queue

(From the [Service Bus Quickstart](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-cli))

Before we install Promitor on AKS to start getting the metrics you want out of Azure Monitor, we need to set up the Azure Resource you want to monitor and have it ready to get metrics for (i.e Service Bus, Azure Functions, VM etc.). In this walkthrough we are going to set up a Service Bus queue as an example.

First we'll need to create a namespace. Service Bus Namespaces need to be globally unique, so we won't use a default name in these commands.

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

**Note: If you're seeing errors installing Prometheus or Grafana from the Helm chart repository, make sure you run `helm repo update` before digging into the errors more. You might have an outdated copy of the chart.**

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

Now we'll add load to our Service Bus queue so there are meaningful metrics for Promitor to pick up. The easiest way to do this is via the [Service Bus Explorer](https://github.com/paolosalvatori/ServiceBusExplorer/releases). Download and unzip the latest release and run the executable inside.

In Service Bus Explorer, you can connect to your namespace & queue using a connection string. From there, right clicking on the queue in the side-bar should give you an option to 'Send Message' - from there, use the 'Sender' tab of that window to send bulk messages. Remember how many you send - you should see that number in the Promitor & Prometheus output.

## See Promitor & Prometheus output via port-forwarding

Going back to your cluster, you should be able to see all Promitor & Prometheus pods up and running with `kubectl get pods`. You can also see the services - these provide a stable endpoint at which to reach the pods - by running `kubectl get services`. This should give you a list with output similar to:

| NAME | TYPE | CLUSTER-IP | EXTERNAL-IP | PORT(S) |
| ---- | ---- | ---------- | ----------- | ------- |
| promitor-agent-scraper | ClusterIP | 10.0.#.# | \<none\> | 80/TCP |
| \<prometheus-release-name\>-prometheus-server | ClusterIP | 10.0.#.# | \<none\> | 80/TCP |

as well as listing the other services deployed by prometheus.

Let's first look at the Promitor output. Run `kubectl port-forward svc/promitor-agent-scraper 8080:80`. Then check http://localhost:8080/metrics - you should see some information about your queue:

```bash
# TODO sample output
```

We can also look at the Prometheus server and check that it's pulling in metrics from Promitor. Cancel the previous port-forward command and run `kubectl port-forward svc/<prometheus-release-name>-prometheus-server 8080:80`. Now, if you check http://localhost:8080, you should be able to enter Prometheus queries. Query `demo_queue_size` - as long as all your pods are up and running and both Promitor and Prometheus have scraped metrics at least once, you should see a value that matches the number of messages in your queue.

## Prometheus alert?

## Install Grafana

Grafana's chart has a few default values you may not want long term - persistant storage is disabled and admin username/password is randomly generated - but for our sample the out-of-the-box install will work. Run `helm install stable/grafana --name grafana` and you should see output that includes this command:

```
kubectl get secret --namespace default grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo
```

Run this to get your Grafana password.

Now you can use `kubectl port-forward` again to log in to your Grafana dashboard. `kubectl port-forward svc/grafana 8080:80` will make your dashboard available at http://localhost:8080, and you can log in with username 'admin' and the password you retrieved.

## Grafana dashboard

[&larr; back](/)