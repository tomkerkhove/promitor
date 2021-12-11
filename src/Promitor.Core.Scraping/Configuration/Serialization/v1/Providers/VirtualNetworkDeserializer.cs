using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualNetworkDeserializer : ResourceDeserializer<VirtualNetworkResourceV1>
    {
        public VirtualNetworkDeserializer(ILogger<VirtualNetworkDeserializer> logger) : base(logger)
        {
            Map(resource => resource.VirtualNetworkName)
                .IsRequired();
        }
    }
}
