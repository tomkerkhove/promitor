﻿using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureResourceDeserializerFactory : IAzureResourceDeserializerFactory
    {
        private readonly IDeserializer<SecretV1> _secretDeserializer;
        private readonly ILoggerFactory _loggerFactory;

        public AzureResourceDeserializerFactory(IDeserializer<SecretV1> secretDeserializer, ILoggerFactory loggerFactory)
        {
            _secretDeserializer = secretDeserializer;
            _loggerFactory = loggerFactory;
        }

        public IDeserializer<AzureResourceDefinitionV1> GetDeserializerFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ApiManagement:
                    var apiManagementLogger = _loggerFactory.CreateLogger<ApiManagementDeserializer>();
                    return new ApiManagementDeserializer(apiManagementLogger);
                case ResourceType.AppPlan:
                    var appPlanLogger = _loggerFactory.CreateLogger<AppPlanDeserializer>();
                    return new AppPlanDeserializer(appPlanLogger);
                case ResourceType.BlobStorage:
                    var blobStorageLogger = _loggerFactory.CreateLogger<BlobStorageDeserializer>();
                    return new BlobStorageDeserializer(blobStorageLogger);
                case ResourceType.ContainerInstance:
                    var containerInstanceLogger = _loggerFactory.CreateLogger<ContainerInstanceDeserializer>();
                    return new ContainerInstanceDeserializer(containerInstanceLogger);
                case ResourceType.ContainerRegistry:
                    var containerRegistryLogger = _loggerFactory.CreateLogger<ContainerRegistryDeserializer>();
                    return new ContainerRegistryDeserializer(containerRegistryLogger);
                case ResourceType.CosmosDb:
                    var cosmosDbLogger = _loggerFactory.CreateLogger<CosmosDbDeserializer>();
                    return new CosmosDbDeserializer(cosmosDbLogger);
                case ResourceType.DeviceProvisioningService:
                    var deviceProvisioningServiceLogger = _loggerFactory.CreateLogger<DeviceProvisioningServiceDeserializer>();
                    return new DeviceProvisioningServiceDeserializer(deviceProvisioningServiceLogger);
                case ResourceType.EventHubs:
                    var eventHubsLogger = _loggerFactory.CreateLogger<EventHubsDeserializer>();
                    return new EventHubsDeserializer(eventHubsLogger);
                case ResourceType.ExpressRouteCircuits:
                    var expressRouteCircuitsLogger = _loggerFactory.CreateLogger<ExpressRouteCircuitsDeserializer>();
                    return new ExpressRouteCircuitDeserializer(expressRouteCircuitsLogger);
                case ResourceType.FileStorage:
                    var fileStorageLogger = _loggerFactory.CreateLogger<FileStorageDeserializer>();
                    return new FileStorageDeserializer(fileStorageLogger);
                case ResourceType.FunctionApp:
                    var functionAppLogger = _loggerFactory.CreateLogger<FunctionAppDeserializer>();
                    return new FunctionAppDeserializer(functionAppLogger);
                case ResourceType.Generic:
                    var genericLogger = _loggerFactory.CreateLogger<GenericResourceDeserializer>();
                    return new GenericResourceDeserializer(genericLogger);
                case ResourceType.IoTHub:
                    var iotHubLogger = _loggerFactory.CreateLogger<IoTHubDeserializer>();
                    return new IoTHubDeserializer(iotHubLogger);
                case ResourceType.KeyVault:
                    var keyVaultLogger = _loggerFactory.CreateLogger<KeyVaultDeserializer>();
                    return new KeyVaultDeserializer(keyVaultLogger);
                case ResourceType.LogicApp:
                    var logicAppLogger = _loggerFactory.CreateLogger<LogicAppDeserializer>();
                    return new LogicAppDeserializer(logicAppLogger);
                case ResourceType.NetworkInterface:
                    var networkLogger = _loggerFactory.CreateLogger<NetworkInterfaceDeserializer>();
                    return new NetworkInterfaceDeserializer(networkLogger);
                case ResourceType.PostgreSql:
                    var postgreSqlLogger = _loggerFactory.CreateLogger<PostgreSqlDeserializer>();
                    return new PostgreSqlDeserializer(postgreSqlLogger);
                case ResourceType.RedisCache:
                    var redisCacheLogger = _loggerFactory.CreateLogger<RedisCacheDeserializer>();
                    return new RedisCacheDeserializer(redisCacheLogger);
                case ResourceType.ServiceBusQueue:
                    var serviceBusLogger = _loggerFactory.CreateLogger<ServiceBusQueueDeserializer>();
                    return new ServiceBusQueueDeserializer(serviceBusLogger);
                case ResourceType.SqlDatabase:
                    var sqlDatabaseLogger = _loggerFactory.CreateLogger<SqlDatabaseDeserializer>();
                    return new SqlDatabaseDeserializer(sqlDatabaseLogger);
                case ResourceType.SqlManagedInstance:
                    var sqlManagedInstanceLogger = _loggerFactory.CreateLogger<SqlManagedInstanceDeserializer>();
                    return new SqlManagedInstanceDeserializer(sqlManagedInstanceLogger);
                case ResourceType.SqlServer:
                    var sqlServerLogger = _loggerFactory.CreateLogger<SqlServerDeserializer>();
                    return new SqlServerDeserializer(sqlServerLogger);
                case ResourceType.StorageAccount:
                    var storageAccountLogger = _loggerFactory.CreateLogger<StorageAccountDeserializer>();
                    return new StorageAccountDeserializer(storageAccountLogger);
                case ResourceType.StorageQueue:
                    var storageQueueLogger = _loggerFactory.CreateLogger<StorageQueueDeserializer>();
                    return new StorageQueueDeserializer(_secretDeserializer, storageQueueLogger);
                case ResourceType.VirtualMachine:
                    var virtualMachineLogger = _loggerFactory.CreateLogger<VirtualMachineDeserializer>();
                    return new VirtualMachineDeserializer(virtualMachineLogger);
                case ResourceType.VirtualMachineScaleSet:
                    var virtualMachineScaleSetLogger = _loggerFactory.CreateLogger<VirtualMachineScaleSetDeserializer>();
                    return new VirtualMachineScaleSetDeserializer(virtualMachineScaleSetLogger);
                case ResourceType.WebApp:
                    var webAppLogger = _loggerFactory.CreateLogger<WebAppDeserializer>();
                    return new WebAppDeserializer(webAppLogger);
                default:
                    throw new ArgumentOutOfRangeException($"Resource Type {resourceType} not supported.");
            }
        }
    }
}
