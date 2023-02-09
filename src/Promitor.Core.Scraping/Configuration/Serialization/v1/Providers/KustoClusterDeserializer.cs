using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class KustoClusterDeserializer : ResourceDeserializer<KustoClusterResourceV1>
    {
        public KustoClusterDeserializer(ILogger<KustoClusterDeserializer> logger) : base(logger)
        {
            Map(resource => resource.KustoClusterName)
                .IsRequired();
        }
    }
}
