using System;
using Promitor.Core.Scraping.Configuration.Serialization.Deserializers;

namespace Promitor.Core.Scraping.Factories
{
    internal static class MetricDeserializerFactory
    {
        internal static GenericAzureMetricDeserializer GetDeserializerFor(Configuration.Model.ResourceType resource)
        {
            switch (resource)
            {
                case Configuration.Model.ResourceType.Generic:
                    return new GenericAzureMetricDeserializer();
                case Configuration.Model.ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueMetricDeserializer();
                case Configuration.Model.ResourceType.StorageQueue:
                    return new StorageQueueMetricDeserializer();
                case Configuration.Model.ResourceType.ContainerInstance:
                    return new ContainerInstanceMetricDeserializer();
                case Configuration.Model.ResourceType.VirtualMachine:
                    return new VirtualMachineMetricDeserializer();
                case Configuration.Model.ResourceType.ContainerRegistry:
                    return new ContainerRegistryMetricDeserializer();
                case Configuration.Model.ResourceType.NetworkInterface:
                    return new NetworkInterfaceMetricDeserializer();
				case Configuration.Model.ResourceType.CosmosDb:
                    return new CosmosDbMetricDeserializer();
                case Configuration.Model.ResourceType.RedisCache:
                    return new RedisCacheMetricDeserializer();
            }

            throw new ArgumentOutOfRangeException($@"Resource Type {resource} not supported.");
        }
    }
}
