using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class TrafficManagerDeserializer : ResourceDeserializer<TrafficManagerResourceV1>
    {
        public TrafficManagerDeserializer(ILogger<TrafficManagerDeserializer> logger) : base(logger)
        {
            Map(resource => resource.Name)
                .IsRequired();
        }
    }
}
