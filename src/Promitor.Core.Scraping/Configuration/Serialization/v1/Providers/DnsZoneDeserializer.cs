using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class DnsZoneDeserializer : ResourceDeserializer<DnsZoneResourceV1>
    {
        public DnsZoneDeserializer(ILogger<DnsZoneDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ZoneName)
                .IsRequired();
        }
    }
}