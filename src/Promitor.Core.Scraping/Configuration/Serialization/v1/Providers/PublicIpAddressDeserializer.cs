using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PublicIpAddressDeserializer : ResourceDeserializer<PublicIpAddressResourceV1>
    {
        public PublicIpAddressDeserializer(ILogger<PublicIpAddressDeserializer> logger) : base(logger)
        {
            Map(resource => resource.PublicIpAddressName)
                .IsRequired();
        }
    }
}
