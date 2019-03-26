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
            }

            throw new ArgumentOutOfRangeException($@"Resource Type {resource} not supported.");
        }
    }
}
