using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureResourceDiscoveryGroupDeserializer : Deserializer<AzureResourceDiscoveryGroupDefinitionV1>
    {
        public AzureResourceDiscoveryGroupDeserializer(ILogger<AzureResourceDiscoveryGroupDeserializer> logger) : base(logger)
        {
            Map(metadata => metadata.Name)
                .IsRequired();
        }
    }
}
