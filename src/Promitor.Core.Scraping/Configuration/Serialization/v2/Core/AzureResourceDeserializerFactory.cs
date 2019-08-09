using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class AzureResourceDeserializerFactory : IAzureResourceDeserializerFactory
    {
        private readonly IDeserializer<SecretV2> _secretDeserializer;
        private readonly ILogger _logger;

        public AzureResourceDeserializerFactory(IDeserializer<SecretV2> secretDeserializer, ILogger logger)
        {
            _secretDeserializer = secretDeserializer;
            _logger = logger;
        }

        public IDeserializer<AzureResourceDefinitionV2> GetDeserializerFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueDeserializer(_logger);
                case ResourceType.Generic:
                    return new GenericResourceDeserializer(_logger);
                case ResourceType.StorageQueue:
                    return new StorageQueueDeserializer(_secretDeserializer, _logger);
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceDeserializer(_logger);
                case ResourceType.VirtualMachine:
                    return new VirtualMachineDeserializer(_logger);
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryDeserializer(_logger);
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceDeserializer(_logger);
                case ResourceType.CosmosDb:
                    return new CosmosDbDeserializer(_logger);
                case ResourceType.RedisCache:
                    return new RedisCacheDeserializer(_logger);
                case ResourceType.PostgreSql:
                    return new PostgreSqlDeserializer(_logger);
                default:
                    throw new ArgumentOutOfRangeException($"Resource Type {resourceType} not supported.");
            }
        }
    }
}
