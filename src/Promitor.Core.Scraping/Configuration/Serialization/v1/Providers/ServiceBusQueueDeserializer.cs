using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ServiceBusQueueDeserializer : ResourceDeserializer<ServiceBusQueueResourceV1>
    {
        public ServiceBusQueueDeserializer(ILogger<ServiceBusQueueDeserializer> logger) : base(logger)
        {
            Map(resource => resource.Namespace)
                .IsRequired();
            Map(resource => resource.QueueName);
        }
    }
}