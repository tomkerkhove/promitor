using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class TrafficManagerProfileDeserializer : ResourceDeserializer<TrafficManagerProfileResourceV1>
    {
        public TrafficManagerProfileDeserializer(ILogger<TrafficManagerProfileDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ProfileName)
                .IsRequired();
        }
    }
}
