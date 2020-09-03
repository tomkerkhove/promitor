using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ApplicationGatewayDeserializer : ResourceDeserializer<ApplicationGatewayResourceV1>
    {
        public ApplicationGatewayDeserializer(ILogger<ApplicationGatewayDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ApplicationGatewayName)
                .IsRequired();
        }
    }
}
