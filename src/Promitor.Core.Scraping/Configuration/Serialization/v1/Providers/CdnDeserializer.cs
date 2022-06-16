using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class CdnDeserializer : ResourceDeserializer<CdnResourceV1>
    {
        public CdnDeserializer(ILogger<CdnDeserializer> logger) : base(logger)
        {
            Map(resource => resource.CdnName)
                .IsRequired();
        }
    }
}
