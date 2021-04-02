---
layout: default
title: Using Managed Identity with Promitor on Azure Kubernetes Service
---

## Introduction

This walkthrough will allow you to deploy Promitor that uses [Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
 on an Azure Kubernetes Service cluster to scrape Azure Service Bus metrics, using no-password authentication.

In order to achieve this, we will use the [AAD Pod Identity project](https://github.com/Azure/aad-pod-identity) to
 manage the identities and authentication.

> ⚠ This only works with Azure Kubernetes Service - Learn more about [Managed Identity in Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/use-managed-identity)
> in the official Microsoft documentation.

## Table of Contents

TODO: Regenerate before merging

- [Introduction](#introduction)
- [Table of Contents](#table-of-contents)
- [Prerequisites](#prerequisites)
- [Deploying the Azure Infrastructure](#deploying-the-azure-infrastructure)
  - [Preparing script](#preparing-script)
  - [Creating an Azure Resource Group](#creating-an-azure-resource-group)
  - [Creating an Azure Service Bus Namespace & Queue](#creating-an-azure-service-bus-namespace--queue)
  - [Creating an Azure Kubernetes Service Cluster](#creating-an-azure-kubernetes-service-cluster)
- [Setting up our cluster](#setting-up-our-cluster)
  - [Getting The Cluster Credentials](#getting-the-cluster-credentials)
  - [Get AKS Managed Identity and MC resource group](#get-aks-managed-identity-and-mc-resource-group)
- [AAD Pod Identity](#aad-pod-identity)
  - [Configure AKS Managed Identity for AAD Pod Identity](#configure-aks-managed-identity-for-aad-pod-identity)
  - [Install AAD Pod Identity](#install-aad-pod-identity)
  - [Create the Managed Identity for Promitor](#create-the-managed-identity-for-promitor)
  - [Bind your Managed Identity to your Pods, through AAD Pod Identity](#bind-your-managed-identity-to-your-pods-through-aad-pod-identity)
  - [Verifying the AAD Pod Identity installation](#verifying-the-aad-pod-identity-installation)
- [Deploying Promitor with Managed Identity](#deploying-promitor-with-managed-identity)
  - [Create a metrics declaration for Promitor](#create-a-metrics-declaration-for-promitor)
  - [Deploy Promitor to your cluster using Helm](#deploy-promitor-to-your-cluster-using-helm)
  - [Verifying the scraped output in Promitor](#verifying-the-scraped-output-in-promitor)
- [Cleaning up](#cleaning-up)

## Prerequisites

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest),
to be able to deploy resources through the command line.
- [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/), the Kubernetes
  command-line tool. It can also be installed via the Azure CLI with `az aks install-cli`.
- [Helm](https://helm.sh/docs/using_helm/#installing-the-helm-client), a Kubernetes
  deployment manager.
- [WSL](https://docs.microsoft.com/en-us/windows/wsl/), if you are using a Windows machine to deploy your whole solution.

## Deploying the Azure Infrastructure

### Preparing script

Since we are going to use a lot of bash scripts with different variables values,
it can be a good idea to parameterize everything.

Let's start by **exporting** all the values we need:

```bash
# SUBSCRIPTION_ID represents the Azure subscription id you will use to access your Azure resources
export SUBSCRIPTION_ID=<guid-subscription-id>

# RG_NAME represents the resource group name where your cluster will be deployed
export RG_NAME=PromitorWithManagedIdentityRG

# LOCATION represents the Azure region where your cluster will be deployed
export LOCATION=northeurope

# CLUSTER_NAME represents the name of your Azure Kubernetes Service Cluster
export CLUSTER_NAME=PromitorCluster

# AD_POD_IDENTITY_NAME represents the name of the Azure AD identity that will be assigned to Promitor.
# Be careful, should be lower case alphanumeric characters, '-' or '.'
export AD_POD_IDENTITY_NAME=promitor-identity

# As an example, we are going to use a service bus from where we want to grab some metrics, through Promitor
# SERVICE_BUS_NAMESPACE represents the name of your Azure Service Bus namespace.
# Be careful as Azure Service Bus Namespaces need to be globally unique.
export SERVICE_BUS_NAMESPACE=PromitorUniqueNameServiceBus

# SERVICE_BUS_QUEUE represents the name of you Azure Service Bus queue.
export SERVICE_BUS_QUEUE=demo_queue
```

### Creating an Azure Resource Group

First, let's create an Azure resource group in which we'll group all our resources:

```bash
$ az group create --name $RG_NAME --location $LOCATION
{
  "id": "/subscriptions/<guid-subscription-id>/resourceGroups/PromitorWithManagedIdentityRG",
  "...": "..."
}
```

### Creating an Azure Service Bus Namespace & Queue

First we'll need to create an Azure Service Bus namespace that provides metrics in Azure Monitor.

These metrics will be scraped by Promitor and made available to the configured metric sinks.

> 💡 Consider this to be an example of metrics that you'd want to use in Prometheus, StatsD, ...

Let's create the Azure Service Bus namespace:

```bash
$ az servicebus namespace create \
  --resource-group $RG_NAME \
  --name $SERVICE_BUS_NAMESPACE \
  --location $LOCATION
```

After that, we'll create a queue in that namespace:

```bash
$ az servicebus queue create \
  --resource-group $RG_NAME \
  --namespace-name $SERVICE_BUS_NAMESPACE \
  --name $SERVICE_BUS_QUEUE
```

### Creating an Azure Kubernetes Service Cluster

Create an Azure Kubernetes Service cluster that uses a system-assigned managed identity:

```bash
$ az aks create --resource-group $RG_NAME \
    --name $CLUSTER_NAME \
    --generate-ssh-keys \
    --node-count 1 \
    --enable-managed-identity

...
"servicePrincipalProfile": {
    "clientId": "msi"
  }
...
```

Once created, the output will indicate that it is using managed identity.

## Setting up our cluster

### Getting The Cluster Credentials

In order to interact with the cluster by using `kubectl`, we need to be able to authenticate to it.

You can get the credentials for your Kubernetes cluster using this command:

```bash
$ az aks get-credentials --name $CLUSTER_NAME --resource-group $RG_NAME
Merged "<cluster-name>" as current context in /home/tom/.kube/config
```

This saves the credentials in your kubeconfig file and uses it as your current context for all `kubectl` commands.

Verify that you can connect and your cluster is up and running :
```bash
$ kubectl get nodes
NAME                                STATUS   ROLES   AGE   VERSION
aks-agentpool-34594731-vmss000000   Ready    agent   15d   v1.19.7
aks-agentpool-34594731-vmss000001   Ready    agent   15d   v1.19.7
aks-agentpool-34594731-vmss000002   Ready    agent   15d   v1.19.7
```

### Get AKS Managed Identity and MC resource group

To be able to configure **aad-pod-identity**, we will need some information from your newly deployed **AKS** cluster:

- **MC Resource Group**: Resource group where the AKS internal resources have been deployed
- **AKS Managed Identity Id**: AKS Managed Identity created internally (since we used the `--enable-managed-identity`
 option) that is used by your AKS cluster to access Azure resources.

```bash
echo "Retrieving MC resource group"
export mc_aks_group=$(az aks show -g $RG_NAME -n $CLUSTER_NAME --query nodeResourceGroup -otsv)

echo "Retrieving cluster identity ID, which will be used for role assignment"
export mc_aks_mi_id="$(az aks show -g ${RG_NAME} -n ${CLUSTER_NAME} --query identityProfile.kubeletidentity.clientId -otsv)"
```

## AAD Pod Identity

### Configure AKS Managed Identity for AAD Pod Identity

If you want to know more about how to configure and manage your aad pod identities, check the official documentation: <https://azure.github.io/aad-pod-identity/docs/>

In this walkthrough we are going to configure the **AKS Managed Identity** to allow **aad-pod-identity** to access
 required resources. (More info: [AAD Pod Identity Role Assignements](https://azure.github.io/aad-pod-identity/docs/getting-started/role-assignment/#performing-role-assignments))

```bash
echo "Assigning role needed by Pod Identity:"
echo "Assigning 'Managed Identity Operator' role to ${mc_aks_mi_id} on resource group ${mc_aks_group}"
az role assignment create --role "Managed Identity Operator" --assignee "${mc_aks_mi_id}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${mc_aks_group}"

echo "Assigning 'Virtual Machine Contributor' role to ${mc_aks_mi_id} on resource group ${mc_aks_group}"
az role assignment create --role "Virtual Machine Contributor" --assignee "${mc_aks_mi_id}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${mc_aks_group}"

echo "Assigning 'Managed Identity Operator' role to ${mc_aks_mi_id} on resource group ${RG_NAME}"
az role assignment create --role "Managed Identity Operator" --assignee "${mc_aks_mi_id}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${RG_NAME}"
```

### Install AAD Pod Identity

To deploy AAD Pod Identity, we need to add the chart:

```bash
echo "Add helm repo for aad pod identity"
helm repo add aad-pod-identity https://raw.githubusercontent.com/Azure/aad-pod-identity/master/charts
helm repo update
```

Then we are good to go to deploy the chart:

```bash
echo "Install aad pod identity and allow use of kubenet"
helm install aad-pod-identity aad-pod-identity/aad-pod-identity --set nmi.allowNetworkPluginKubenet=true
```

### Create the Managed Identity for Promitor

We can basically use the already deployed and created managed identity used by AKS, but since we want to [separate the concerns](https://en.wikipedia.org/wiki/Separation_of_concerns),
we are going to create a new managed identity that will be the identity used later by all your Pods.

```bash
echo "Create identity $AD_POD_IDENTITY_NAME in resource group $RG_NAME"
az identity create -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME}
```

This identity will be used by **Promitor** to access **Azure Monitoring**.  
So far, we need to allow this identity to read data from **Azure Monitoring** through **RBAC Roles Assignements**:

> _**Warning:**_ It can take some times before the Identity is correctly propagated in Azure Active Directory.  
> So far if you encountered an error where the identity is not found, please wait 60 sec and retry

```bash
echo "Getting Azure Identity used by aad pod identity"
export AD_POD_IDENTITY_CLIENT_ID=$(az identity show -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME} --query "clientId" -o tsv)
export AD_POD_IDENTITY_RESOURCE_ID=$(az identity show -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME} --query "id" -o tsv)

echo "Assign role 'Monitoring Reader' to identity $AD_POD_IDENTITY_NAME in resource group $RG_NAME"
az role assignment create --role "Monitoring Reader" --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RG_NAME" --assignee "${AD_POD_IDENTITY_CLIENT_ID}"
```

_Note:_ You can check the role assignements using this command:

```bash
az role assignment list --assignee $AD_POD_IDENTITY_CLIENT_ID -g $RG_NAME | jq -r '.[].roleDefinitionName'
```

### Bind your Managed Identity to your Pods, through AAD Pod Identity

The Managed Identity is created, AAD Pod Identity is deployed, it's time to bind your identity to your Pods:

```bash
echo "Create an AzureIdentity in the AKS cluster that references the identity $AD_POD_IDENTITY_NAME"

cat <<EOF | kubectl apply -f -
apiVersion: "aadpodidentity.k8s.io/v1"
kind: AzureIdentity
metadata:
  name: ${AD_POD_IDENTITY_NAME}
spec:
  type: 0 # 0 - user assigned MSI, 1 - service principal
  resourceID: ${AD_POD_IDENTITY_RESOURCE_ID}
  clientID: ${AD_POD_IDENTITY_CLIENT_ID}
EOF


echo "Create an AzureIdentityBinding that reference the AzureIdentity created"

cat <<EOF | kubectl apply -f -
apiVersion: "aadpodidentity.k8s.io/v1"
kind: AzureIdentityBinding
metadata:
  name: ${AD_POD_IDENTITY_NAME}-binding
spec:
  azureIdentity: ${AD_POD_IDENTITY_NAME}
  selector: ${AD_POD_IDENTITY_NAME}
EOF
```

_Note:_ You can check the AAD Pod Identity deployed bindings, using this command:

```bash
kubectl get azureidentity
kubectl get azureidentitybinding
```

### Verifying the AAD Pod Identity installation

Before going further, you can check if your AAD Pod Identity is deployed and configured correctly:

```bash
echo "If you want to test the binding of identity, use this command"
kubectl run azure-cli -it --image=mcr.microsoft.com/azure-cli --labels=aadpodidbinding=$AD_POD_IDENTITY_NAME /bin/bash
```

> _**Warning:**_ It can take some times to Aad Pod Identity to bind the identity to your deployed container.
> If you encountered an error, relaunch the `az login -i --debug` command after 60 sec.

```bash
# Once you are log in the container, and have the bash command line available, try to login using the Managed Identity:
If you don t see a command prompt, try pressing enter.
bash-5.0# az login -i --debug
# You should have a line with 'MSI: token was retrieved. Now trying to initialize local accounts'"
```

If your Azure CLI container is able to retrieve some information from Azure
without having to log in with your credentials,
it means the CLI is using your Managed Identity, through the Pod Identity Binding.

You may have a result indicating you are log in, using a **System Assigned Identity**

```json
"user": {
    "assignedIdentityInfo": "MSI",
    "name": "systemAssignedIdentity",
}
```

## Deploying Promitor with Managed Identity

### Create a metrics declaration for Promitor

Before deploying Promitor, you'll need a values file with the managed identity option & a metric declaration
file (these can also be the same file for ease of use).  
The yaml below will scrape
one metric, queue length, from the queue created above.

```yaml
azureAuthentication:
  mode: SystemAssignedManagedIdentity
  identity:
    binding: <aad-pod-identity-name>
azureMetadata:
  tenantId: <guid-tenant-id>
  subscriptionId: <guid-subscription-id>
  resourceGroupName: <promitor-resource-group-id>
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
        queueName: <service-bus-queue>
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

### Verifying the scraped output in Promitor

You can check that Promitor is getting insights from you service bus, using the managed identity, with this commands:

```bash
# Get promitor pod
export POD_NAME=$(kubectl get pods --namespace default -l "app.kubernetes.io/instance=promitor-agent-scraper" -o jsonpath="{.items[0].metadata.name}")

kubectl port-forward --namespace default $POD_NAME 8080:88
```

Now browse to the address <http://127.0.0.1:8080/metrics> and check your metrics are scrapped:

``` html
# HELP demo_queue_size Amount of active messages of the 'demo_queue' queue
# TYPE demo_queue_size gauge
demo_queue_size{resource_group="ammdocs",subscription_id="xxxxx-xxxxx-xxxxx-xxxxxx-xxxxx",resource_uri="subscriptions/xxxxx-xxxxx-xxxxx-xxxxxx-xxxxx/resourceGroups/YOUR_RESOURCE_GROUP_NAME/providers/Microsoft.ServiceBus/namespaces/YOUR_SERVICE_BUS_NAMESPACE",instance_name="INSTANCE_NAME",entity_name="YOUR_SERVICE_BUS_QUEUE"} 0 1612952581417
```

## Cleaning up

To delete all the resources used in this tutorial, run `az group delete --name $RG_NAME`.

[&larr; back](/)
