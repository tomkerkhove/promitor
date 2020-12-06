using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ServiceBusNamespaceDeserializer : ResourceDeserializer<ServiceBusNamespaceResourceV1>
    {
        public ServiceBusNamespaceDeserializer(ILogger<ServiceBusNamespaceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.Namespace)
                .IsRequired();
            Map(resource => resource.QueueName);
            Map(resource => resource.TopicName);
        }
    }
}