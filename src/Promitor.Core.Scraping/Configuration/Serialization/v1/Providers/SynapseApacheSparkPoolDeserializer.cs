using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class SynapseApacheSparkPoolDeserializer : ResourceDeserializer<SynapseApacheSparkPoolResourceV1>
    {
        public SynapseApacheSparkPoolDeserializer(ILogger<SynapseApacheSparkPoolDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WorkspaceName)
                .IsRequired();
            Map(resource => resource.PoolName)
                .IsRequired();
        }
    }
}