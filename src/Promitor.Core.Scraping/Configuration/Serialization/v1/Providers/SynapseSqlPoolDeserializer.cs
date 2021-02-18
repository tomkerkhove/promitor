using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class SynapseSqlPoolDeserializer : ResourceDeserializer<SynapseSqlPoolResourceV1>
    {
        public SynapseSqlPoolDeserializer(ILogger<SynapseSqlPoolDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WorkspaceName)
                .IsRequired();
            Map(resource => resource.PoolName)
                .IsRequired();
        }
    }
}