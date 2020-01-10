using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
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
                case ResourceType.ServiceBusQueue:
                    var serviceBusLogger = _loggerFactory.CreateLogger<ServiceBusQueueDeserializer>();
                    return new ServiceBusQueueDeserializer(serviceBusLogger);
                case ResourceType.Generic:
                    var genericLogger = _loggerFactory.CreateLogger<GenericResourceDeserializer>();
                    return new GenericResourceDeserializer(genericLogger);
                case ResourceType.StorageQueue:
                    var storageQueueLogger = _loggerFactory.CreateLogger<StorageQueueDeserializer>();
                    return new StorageQueueDeserializer(_secretDeserializer, storageQueueLogger);
                case ResourceType.ContainerInstance:
                    var containerInstanceLogger = _loggerFactory.CreateLogger<ContainerInstanceDeserializer>();
                    return new ContainerInstanceDeserializer(containerInstanceLogger);
                case ResourceType.VirtualMachine:
                    var virtualMachineLogger = _loggerFactory.CreateLogger<VirtualMachineDeserializer>();
                    return new VirtualMachineDeserializer(virtualMachineLogger);
                case ResourceType.ContainerRegistry:
                    var containerRegistryLogger = _loggerFactory.CreateLogger<ContainerRegistryDeserializer>();
                    return new ContainerRegistryDeserializer(containerRegistryLogger);
                case ResourceType.NetworkInterface:
                    var networkLogger = _loggerFactory.CreateLogger<NetworkInterfaceDeserializer>();
                    return new NetworkInterfaceDeserializer(networkLogger);
                case ResourceType.CosmosDb:
                    var cosmosDbLogger = _loggerFactory.CreateLogger<CosmosDbDeserializer>();
                    return new CosmosDbDeserializer(cosmosDbLogger);
                case ResourceType.RedisCache:
                    var redisCacheLogger = _loggerFactory.CreateLogger<RedisCacheDeserializer>();
                    return new RedisCacheDeserializer(redisCacheLogger);
                case ResourceType.PostgreSql:
                    var postgreSqlLogger = _loggerFactory.CreateLogger<PostgreSqlDeserializer>();
                    return new PostgreSqlDeserializer(postgreSqlLogger);
                case ResourceType.SqlDatabase:
                    var sqlDatabaseLogger = _loggerFactory.CreateLogger<SqlDatabaseDeserializer>();
                    return new SqlDatabaseDeserializer(sqlDatabaseLogger);
                case ResourceType.SqlManagedInstance:
                    var sqlManagedInstanceLogger = _loggerFactory.CreateLogger<SqlManagedInstanceDeserializer>();
                    return new SqlManagedInstanceDeserializer(sqlManagedInstanceLogger);
                case ResourceType.VirtualMachineScaleSet:
                    var virtualMachineScaleSetLogger = _loggerFactory.CreateLogger<VirtualMachineScaleSetDeserializer>();
                    return new VirtualMachineScaleSetDeserializer(virtualMachineScaleSetLogger);
                case ResourceType.AppPlan:
                    var appPlanLogger = _loggerFactory.CreateLogger<AppPlanDeserializer>();
                    return new AppPlanDeserializer(appPlanLogger);
                case ResourceType.WebApp:
                    var webAppLogger = _loggerFactory.CreateLogger<WebAppDeserializer>();
                    return new WebAppDeserializer(webAppLogger);
                case ResourceType.FunctionApp:
                    var functionAppLogger = _loggerFactory.CreateLogger<FunctionAppDeserializer>();
                    return new FunctionAppDeserializer(functionAppLogger);
                default:
                    throw new ArgumentOutOfRangeException($"Resource Type {resourceType} not supported.");
            }
        }
    }
}
