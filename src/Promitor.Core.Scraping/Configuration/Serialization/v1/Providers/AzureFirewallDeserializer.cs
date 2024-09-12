using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class AzureFirewallDeserializer : ResourceDeserializer<AzureFirewallResourceV1>
    {
        public AzureFirewallDeserializer(ILogger<AzureFirewallDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AzureFirewallName)
                .IsRequired();
        }
    }
}
