using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class MongoClusterDeserializer : ResourceDeserializer<MongoClusterResourceV1>
    {
        public MongoClusterDeserializer(ILogger<MongoClusterDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ClusterName)
                .IsRequired();
        }
    }
}