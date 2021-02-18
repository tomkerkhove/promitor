---
layout: default
title: Declaring resource discovery groups
---

Promitor Resource Discovery allows you to declare the Azure landscape to explore and define resource discovery groups
 in YAML.

Resource discovery groups represent a group of Azure resources of a given type that can be scraped by Promitor Scraper
 and supports an [extensive list of supported services](#supported-azure-services).

As part of the resource discovery group declaration, you can choose to filter resources by adding inclusion criteria
 that resources must comply with based on:

- **Subscription** - Defines a subset of subscriptions defined in the Azure landscape
- **Resource Group** - Defines a list of resource groups which contains the resources.
- **Tags** - Defines a list of [Azure tags](https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/tag-resources)
 with which the resources have to be annotated.
- **Regions** - Defines a list of Azure regions in which the regions the resources are located.

Here is an example of a full declaration:

```yaml
version: v1
azureLandscape:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptions:
  - SUBSCRIPTON-ID-ABC
  - SUBSCRIPTON-ID-DEF
  - SUBSCRIPTON-ID-GHI
  cloud: China
resourceDiscoveryGroups:
- name: container-registry-landscape
  type: ContainerRegistry
- name: filtered-logic-apps-landscape
  type: LogicApp
  criteria:
    include:
      subscriptions:
      - SUBSCRIPTON-ID-ABC
      - SUBSCRIPTON-ID-GHI
      resourceGroups:
      - promitor-resource-group-1
      - promitor-resource-group-2
      tags:
        app: promitor
        region: europe
      regions:
      - northeurope
      - westeurope
```

## Specification

As Promitor evolves we need to change the structure of our resource discovery declaration.

`version: {version}` - Version of declaration that is used. Allowed
values are `v1`. *(Required)*

### Azure Landscape

- `azureLandscape.tenantId` - The id of the Azure tenant that will be queried. *(Required)*
- `azureLandscape.subscriptions` - List of Azure subscriptions in the Azure tenant to discover resources in. *(Required)*
- `azureLandscape.cloud` - The name of the Azure cloud to use. Options are `Global` (default), `China`, `UsGov` & `Germany`.

### Resource Discovery Groups

Every resource discovery group that is being declared needs to define the following fields:

- `name` - Name of the resource discovery group which will be used in metrics dclaration of Promitor Scraper. *(Required)*
- `type` - Type of Azure resources that must be discovered, see ["Supported Azure Services](#supported-azure-services)
 for a full list of supported types. *(Required)*
- `criteria` - Criteria to fine-tune discovered resource.

#### Criteria

As of now, we only allow to define criteria that resources have to meet before they are included in the resource
 discovery group:

- `subscriptions` - A list of subscription(s) in which the resource is allowed to be located.
- `resourceGroups` - A list of resource group(s) in which the resource is allowed to be located.
- `tags` - A list of [Azure tags](https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/tag-resources)
 and the expected values with which the resources have to be annotated.
- `regions` - A list of Azure region(s) in which the resource is allowed to be located.

## Supported Azure Services

Dynamic resource discovery is supported for the following scrapers:

- [Azure API Management](metrics/api-management)
- [Azure Application Gateway](metrics/application-gateway)
- [Azure App Plan](metrics/app-plan)
- [Azure Automation](metrics/automation-account)
- [Azure Cache for Redis](metrics/redis-cache)
- [Azure Container Instances](metrics/container-instances)
- [Azure Container Registry](metrics/container-registry)
- [Azure Cosmos DB](metrics/cosmos-db)
- [Azure Database for PostgreSQL](metrics/postgresql)
- [Azure Event Hubs](metrics/event-hubs)
- [Azure Express Route Circuit](metrics/express-route-circuit)
- [Azure Front Door](metrics/front-door)
- [Azure Function App](metrics/function-app)
- [Azure IoT Hub](metrics/iot-hub)
- [Azure IoT Hub Device Provisioning Service (DPS)](metrics/iot-hub-device-provisioning-service)
- [Azure Key Vault](metrics/key-vault)
- [Azure Kubernetes Service](metrics/kubernetes)
- [Azure Logic Apps](metrics/logic-apps)
- [Azure Network Gateway](metrics/network-gateway)
- [Azure Network Interface](metrics/network-interface)
- [Azure Service Bus Namespace](metrics/service-bus-namespace)
- [Azure SQL Database](metrics/sql-database)
- [Azure SQL Elastic Pool](metrics/sql-elastic-pool)
- [Azure SQL Managed Instance](metrics/sql-managed-instance)
- [Azure Storage (Account)](metrics/storage-account)
- [Azure Synapse (Apache Spark pool)](metrics/synapse-apache-spark-pool)
- [Azure Synapse (SQL pool)](metrics/synapse-sql-pool)
- [Azure Synapse (Workspace)](metrics/synapse-workspace)
- [Azure Virtual Machine](metrics/virtual-machine)
- [Azure Virtual Machine Scale Set (VMSS)](metrics/virtual-machine-scale-set)
- [Azure Web App](metrics/web-app)

[&larr; back](/)
