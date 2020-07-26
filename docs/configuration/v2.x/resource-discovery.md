---
layout: default
title: Resource Discovery
---

This article covers an overview of all the knobs to configure resource discovery.

TODO: Resource discovery configuration

```yaml
azureLandscape:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptions:
  - 0329dd2a-59dc-4493-aa54-cb01cb027dc2
  - 0f9d7fea-99e8-4768-8672-06a28514f77e
resourceDiscoveryGroups:
- name: container-registry-landscape
  type: ContainerRegistry
- name: two-resource-group-scenario
  type: LogicApp
  criteria:
    include:
      resourceGroups:
      - promitor-testing-resource-discovery-eu
      - promitor-testing-resource-discovery-us
      subscriptions:
      - 0329dd2a-59dc-4493-aa54-cb01cb027dc2
      - 0f9d7fea-99e8-4768-8672-06a28514f77e
      tags:
        app: promitor
        region: europe
      regions:
      - northeurope
      - westeurope
```

## Supported Azure Services

Dynamic resource discovery is supported for the following scrapers:

- [Azure API Management](metrics/api-management)
- [Azure App Plan](metrics/app-plan)
- [Azure Cache for Redis](metrics/redis-cache)
- [Azure Container Instances](metrics/container-instances)
- [Azure Container Registry](metrics/container-registry)
- [Azure Cosmos DB](metrics/cosmos-db)
- [Azure Database for PostgreSQL](metrics/postgresql)
- [Azure Function App](metrics/function-app)
- [Azure IoT Hub](metrics/iot-hub)
- [Azure IoT Hub Device Provisioning Service (DPS)](metrics/iot-hub-device-provisioning-service)
- [Azure Key Vault](metrics/key-vault)
- [Azure Logic Apps](metrics/logic-apps)
- [Azure Network Interface](metrics/network-interface)
- [Azure SQL Database](metrics/sql-database)
- [Azure SQL Managed Instance](metrics/sql-managed-instance)
- [Azure Storage (Account)](metrics/storage-account)
- [Azure Virtual Machine](metrics/virtual-machine)
- [Azure Virtual Machine Scale Set (VMSS)](metrics/virtual-machine-scale-set)
- [Azure Web App](metrics/web-app)

[&larr; back](/)
