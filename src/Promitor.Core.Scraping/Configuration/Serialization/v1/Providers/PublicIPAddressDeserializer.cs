using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PublicIPAddressDeserializer : ResourceDeserializer<PublicIPAddressResourceV1>
    {
        public PublicIPAddressDeserializer(ILogger<PublicIPAddressDeserializer> logger) : base(logger)
        {
            Map(resource => resource.PublicIPAddressName)
                .IsRequired();
        }
    }
}
