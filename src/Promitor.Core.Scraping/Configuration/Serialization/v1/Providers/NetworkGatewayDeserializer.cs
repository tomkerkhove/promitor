using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class NetworkGatewayDeserializer : ResourceDeserializer<NetworkGatewayResourceV1>
    {
        public NetworkGatewayDeserializer(ILogger<NetworkGatewayDeserializer> logger) : base(logger)
        {
            Map(resource => resource.NetworkGatewayName)
                .IsRequired();
        }
    }
}
