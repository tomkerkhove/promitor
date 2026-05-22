using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PrivateLinkServiceDeserializer : ResourceDeserializer<PrivateLinkServiceResourceV1>
    {
        public PrivateLinkServiceDeserializer(ILogger<PrivateLinkServiceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.PrivateLinkServiceName)
                .IsRequired();
        }
    }
}
