# Discovery

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-No-red.svg)

**Promitor Resource Discovery** provides a way to discover the resources for our Scraper agent to dynamically scrape resources.

Next to that, it provides a variety of system metrics that provides information concerning your Azure landscape.

## Subscription

Our `promitor_azure_landscape_subscription_info` metrics provides an overview of all the Azure subscriptions that
 Promitor is able to discover in your Azure Landscape.

It provides the following tags with more information:

- `tenant_id` - Id of the Azure tenant
- `subscription_name` - Name of the Azure subscription
- `subscription_id` - Id of the Azure subscription
- `state` - Indication of the state of the subscription ([docs](https://docs.microsoft.com/en-us/azure/cost-management-billing/manage/subscription-states))
- `spending_limit` - Indication whether or not there is a spending limit
- `quota_id` - Id of the Azure subscription used to manage quotas
- `authorization` - Type of authorization that is being used

```prometheus
# HELP promitor_azure_landscape_subscription_info Provides information concerning the Azure subscriptions in the landscape that Promitor has access to.
# TYPE promitor_azure_landscape_subscription_info gauge
promitor_azure_landscape_subscription_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_name="Windows Azure MSDN - Visual Studio Ultimate",subscription_id="0329dd2a-59dc-4493-aa54-cb01cb027dc2",state="Enabled",spending_limit="On",quota_id="MSDN_2014-09-01",authorization="RoleBased"} 1 1628779903451
promitor_azure_landscape_subscription_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_name="Visual Studio Enterprise",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",state="Enabled",spending_limit="Off",quota_id="Sponsored_2016-01-01",authorization="RoleBased"} 1 1628779903451
```

## Resource Groups

Our `promitor_azure_landscape_resource_group_info` metrics provides an overview of all the Azure resource groups that
 Promitor is able to discover in your Azure Landscape across all your subscriptions.

It provides the following tags with more information:

- `tenant_id` - Id of the Azure tenant
- `subscription_id` - Id of the Azure subscription
- `resource_group_name` - Name of the Azure resource group
- `region` - Region in which the resource group is located
- `provisioning_state` - State of the resource group
- `managed_by` - Id of the Azure resource managing this resource group, for example an Azure Kubernetes Service cluster.

```prometheus
# HELP promitor_azure_landscape_resource_group_info Provides information concerning the Azure resource groups in the landscape that Promitor has access to.
# TYPE promitor_azure_landscape_resource_group_info gauge
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="NetworkWatcherRG",region="westeurope",provisioning_state="Succeeded",managed_by="n/a"} 1 1628779903423
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="promitor-testing-resource-discovery-eu",region="westeurope",provisioning_state="Succeeded",managed_by="n/a"} 1 1628779903423
promitor_azure_landscape_resource_group_info{tenant_id="e0372f7f-a362-47fb-9631-74a5c4ba8bbf",subscription_id="0f9d7fea-99e8-4768-8672-06a28514f77e",resource_group_name="MC_keda-demos_keda-demos_westeurope",region="westeurope",provisioning_state="Succeeded",managed_by="/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourcegroups/keda-demos/providers/Microsoft.ContainerService/managedClusters/keda-demos"} 1 1628779903423
```
