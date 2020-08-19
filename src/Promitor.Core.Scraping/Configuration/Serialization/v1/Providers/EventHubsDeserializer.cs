using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class EventHubsDeserializer : ResourceDeserializer<EventHubsResourceV1>
    {
        public EventHubsDeserializer(ILogger<EventHubsDeserializer> logger) : base(logger)
        {
            Map(resource => resource.Namespace)
                .IsRequired();
            Map(resource => resource.TopicName);
        }
    }
}