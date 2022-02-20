# Using Managed Identity with Promitor on Azure Kubernetes Service

## Introduction

This walkthrough will allow you to deploy Promitor that uses [Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
 on an Azure Kubernetes Service cluster to scrape Azure Service Bus metrics, using no-password authentication.

In order to achieve this, we will use the [AAD Pod Identity project](https://github.com/Azure/aad-pod-identity) to
 manage the identities and authentication.

> ⚠ This only works with Azure Kubernetes Service - Learn more about [Managed Identity in Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/use-managed-identity)
> in the official Microsoft documentation.

## Table of Contents

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
  - [Get Azure Kubernetes Service managed identity & cluster resource group](#get-azure-kubernetes-service-managed-identity--cluster-resource-group)
- [Getting our cluster ready to use AAD Pod Identity](#getting-our-cluster-ready-to-use-aad-pod-identity)
  - [Granting our cluster's managed identity required permissions in Azure](#granting-our-clusters-managed-identity-required-permissions-in-azure)
  - [Installing AAD Pod Identity](#installing-aad-pod-identity)
  - [Creating a user-assigned managed identity for Promitor](#creating-a-user-assigned-managed-identity-for-promitor)
  - [Bind your Managed Identity to our Pods, through AAD Pod Identity](#bind-your-managed-identity-to-our-pods-through-aad-pod-identity)
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
export SUBSCRIPTION_ID=<subscription-id>

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
  "id": "/subscriptions/<subscription-id>/resourceGroups/<resource-group-name>",
  "location": "<location-id>",
  "managedBy": null,
  "name": "<resource-group-name>",
  "properties": {
    "provisioningState": "Succeeded"
  },
  "tags": null,
  "type": "Microsoft.Resources/resourceGroups"
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

### Get Azure Kubernetes Service managed identity & cluster resource group

To be able to configure AAD Pod Identity component, we need information from our new Azure Kubernetes Service cluster:

First, we need to get the **name of the cluster resource group** where the AKS internal resources have been deployed.

```bash
echo "Retrieving cluster resource group"
export aks_rg_name=$(az aks show -g $RG_NAME -n $CLUSTER_NAME --query nodeResourceGroup -otsv)
```

This is a generated resource group that typically uses `MC_{resource-group}_{cluster-name}_{region}`, for example `MC_promitor-landscape_promitor_westeurope`.

Second, we need the **identity of our cluster**. This is the system-assigned identity of our cluster that is used to
 access Azure resources.

```bash
echo "Retrieving cluster identity ID, which will be used for role assignment"
export aks_mi_identity="$(az aks show -g ${RG_NAME} -n ${CLUSTER_NAME} --query identityProfile.kubeletidentity.clientId -otsv)"
```

This identity is managed by Azure, since we have used the `--enable-managed-identity` option during cluster creation.

## Getting our cluster ready to use AAD Pod Identity

### Granting our cluster's managed identity required permissions in Azure

In this walkthrough we are going to configure the **managed identity for our cluster** to allow AAD Pod Identity to access
 required resources in Azure.

> Learn more about the [the required role assignments for AAD Pod Identity](https://azure.github.io/aad-pod-identity/docs/getting-started/role-assignment/#performing-role-assignments)
> or have a general overview in the [official documentation](https://azure.github.io/aad-pod-identity/).

In order to be able to use AAD Pod Identity, our cluster needs to be able to:

- Manage identities in our application & cluster resource group
- Manage the virtual machines that are part of the cluster, to use the assigned identity

You can easily do this as following:

```bash
echo "Assigning 'Managed Identity Operator' role to ${aks_mi_identity} on resource group ${aks_rg_name}"
az role assignment create --role "Managed Identity Operator" --assignee "${aks_mi_identity}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${aks_rg_name}"

echo "Assigning 'Virtual Machine Contributor' role to ${aks_mi_identity} on resource group ${aks_rg_name}"
az role assignment create --role "Virtual Machine Contributor" --assignee "${aks_mi_identity}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${aks_rg_name}"

echo "Assigning 'Managed Identity Operator' role to ${aks_mi_identity} on resource group ${RG_NAME}"
az role assignment create --role "Managed Identity Operator" --assignee "${aks_mi_identity}" --scope "/subscriptions/${SUBSCRIPTION_ID}/resourcegroups/${RG_NAME}"
```

### Installing AAD Pod Identity

To deploy AAD Pod Identity, we need to add the Helm chart repository:

```bash
$ helm repo add aad-pod-identity https://raw.githubusercontent.com/Azure/aad-pod-identity/master/charts
"aad-pod-identity" has been added to your repositories
```

Update all your Helm repositories to use the latest and greatest:

```bash
$ helm repo update
Hang tight while we grab the latest from your chart repositories...
...Successfully got an update from the "aad-pod-identity" chart repository
Update Complete. ⎈Happy Helming!⎈
```

Lastly, install the Helm chart into your cluster:

```bash
helm install aad-pod-identity aad-pod-identity/aad-pod-identity --set nmi.allowNetworkPluginKubenet=true
```

### Creating a user-assigned managed identity for Promitor

In order to let Promitor to authenticate to Azure, we have two options:

1. Re-ue the managed identity of our cluster, or (syste-assigned)
2. Create a new identity that we will assign to our Promitor pods (user-assigned)

In order to [separate our concerns](https://en.wikipedia.org/wiki/Separation_of_concerns), we will create a new
 identity for it:

```bash
echo "Create identity $AD_POD_IDENTITY_NAME in resource group $RG_NAME"
az identity create -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME}
```

Our new identity will be used by **Promitor** to access **Azure Monitor** to get metrics by using its assigned
 **RBAC roles assignements**:

First, we will get the client & resource id of our identity:

```bash
export AD_POD_IDENTITY_CLIENT_ID=$(az identity show -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME} --query "clientId" -o tsv)
export AD_POD_IDENTITY_RESOURCE_ID=$(az identity show -g ${RG_NAME} -n ${AD_POD_IDENTITY_NAME} --query "id" -o tsv)
```

Next, we will assign the `Monitoring Reader` role to our identity on our resource group to scrape our Azure Service Bus namespace:

```bash
$ az role assignment create --role "Monitoring Reader" --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RG_NAME" --assignee "${AD_POD_IDENTITY_CLIENT_ID}"
{
  "canDelegate": null,
  "condition": null,
  "conditionVersion": null,
  "description": null,
  "id": "/subscriptions/<subscription-id>/resourceGroups/<resource-group-name>/providers/Microsoft.Authorization/roleAssignments/92b1566a-2346-43f7-a093-1fd5871d4de8",
  "name": "92b1566a-2346-43f7-a093-1fd5871d4de8",
  "principalId": "<promitor-identity-id>",
  "principalType": "ServicePrincipal",
  "resourceGroup": "<resource-group-name>",
  "roleDefinitionId": "/subscriptions/<subscription-id>/providers/Microsoft.Authorization/roleDefinitions/43d0d8ad-25c7-4714-9337-8ba259a9fe05",
  "scope": "/subscriptions/<subscription-id>/resourceGroups/<resource-group-name>",
  "type": "Microsoft.Authorization/roleAssignments"
}
```

In order to verify our role assignement, you can use this command:

```bash
az role assignment list --assignee $AD_POD_IDENTITY_CLIENT_ID -g $RG_NAME | jq -r '.[].roleDefinitionName'
```

> 💡 _It can take some times before the identity is correctly propagated in Azure Active Directory._
> _So far if you encountered an error where the identity is not found, please wait 60 sec and retry_

### Bind your Managed Identity to our Pods, through AAD Pod Identity

Now that our identity is created, we can tell AAD Pod Identity that we want to bind our Azure AD identity to our pods:

First, we will define the identity that we want to assign:

```bash
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
```

Next, we want to define to what pods we want to link it:

```bash
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

You can verify that the resources were created successfully as following:

```bash
kubectl get azureidentity
kubectl get azureidentitybinding
```

### Verifying the AAD Pod Identity installation

Before going further, we will check if our AAD Pod Identity is deployed & configured correctly.

We will spin up a pod and use the Azure CLI to authenticate:

```bash
kubectl run azure-cli -it --image=mcr.microsoft.com/azure-cli --labels=aadpodidbinding=$AD_POD_IDENTITY_NAME /bin/bash
```

Note that we are adding `aadpodidbinding` as a label, which is linking the pod to the AAD Pod Identity binding
 that we have just created.

> _**Warning:**_ It can take some times to Aad Pod Identity to bind the identity to your deployed container.
> If you encountered an error, relaunch the `az login -i --debug` command after 60 sec.

Next, we will run `az login -i --debug` to see the different steps it takes:

```bash
# Once you are log in the container, and have the bash command line available, try to login using the Managed Identity:
# If you don't see a command prompt, try pressing enter.
$ bash-5.0# az login -i --debug
msrestazure.azure_active_directory: MSI: Retrieving a token from http://169.254.169.254/metadata/identity/oauth2/token, with payload {'resource': 'https://management.core.windows.net/', 'api-version': '2018-02-01'}
msrestazure.azure_active_directory: MSI: Token retrieved
cli.azure.cli.core._profile: MSI: token was retrieved. Now trying to initialize local accounts...
...
[
  {
    "environmentName": "AzureCloud",
    "homeTenantId": "e0372f7f-a362-47fb-9631-74a5c4ba8bbf",
    "id": "0f9d7fea-99e8-4768-8672-06a28514f77e",
    "isDefault": true,
    "managedByTenants": [
      {
        "tenantId": "2f4a9838-26b7-47ee-be60-ccc1fdec5953"
      }
    ],
    "name": "Visual Studio Enterprise",
    "state": "Enabled",
    "tenantId": "e0372f7f-a362-47fb-9631-74a5c4ba8bbf",
    "user": {
      "assignedIdentityInfo": "MSI",
      "name": "systemAssignedIdentity",
      "type": "servicePrincipal"
    }
  }
]
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

Before deploying Promitor, we will create a values file for our Helm deployment.

In the configuration, we define what Azure resource that we want to scrape and that we want to use managed identity.

Here is an example:

```yaml
azureAuthentication:
  mode: SystemAssignedManagedIdentity
  identity:
    binding: <aad-pod-identity-name>              # <- This is the value of AD_POD_IDENTITY_NAME environment variable
azureMetadata:
  tenantId: <tenant-id>
  subscriptionId: <subscription-id>               # <- This is the value of SUBSCRIPTION_ID environment variable
  resourceGroupName: <promitor-resource-group-id> # <- This is the value of RG_NAME environment variable
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
      - namespace: <service-bus-namespace>        # <- This is the value of SERVICE_BUS_NAMESPACE environment variable
        queueName: <service-bus-queue>            # <- This is the value of SERVICE_BUS_QUEUE_NAME environment variable
```

### Deploy Promitor to your cluster using Helm

To deploy, we'll first add the Promitor chart repository to helm:

```bash
helm repo add promitor https://charts.promitor.io/
helm repo update
```

With this repository added, we can deploy Promitor:

```bash
helm install promitor-agent-scraper promitor/promitor-agent-scraper --values your/path/to/metric-declaration.yaml
```

### Verifying the scraped output in Promitor

You can check that Promitor is getting insights from you Azure Service Bus queue, using the managed identity, with this commands.

First, we get the name of the Promitor Scraper pod:

```bash
# Get promitor pod
export POD_NAME=$(kubectl get pods --namespace default -l "app.kubernetes.io/instance=promitor-agent-scraper" -o jsonpath="{.items[0].metadata.name}")
```

Next, we add port forwarding from our pod to our local machine:

```bash
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
