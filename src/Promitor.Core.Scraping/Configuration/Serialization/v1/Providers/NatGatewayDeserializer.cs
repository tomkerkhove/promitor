using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class NatGatewayDeserializer : ResourceDeserializer<NatGatewayResourceV1>
    {
        public NatGatewayDeserializer(ILogger<NatGatewayDeserializer> logger) : base(logger)
        {
            Map(resource => resource.NatGatewayName)
                .IsRequired();
        }
    }
}
