using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class DataExplorerClusterDeserializer : ResourceDeserializer<DataExplorerClusterResourceV1>
    {
        public DataExplorerClusterDeserializer(ILogger<DataExplorerClusterDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ClusterName)
                .IsRequired();
        }
    }
}
