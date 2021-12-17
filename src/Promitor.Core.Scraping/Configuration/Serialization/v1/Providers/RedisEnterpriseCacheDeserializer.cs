using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class RedisEnterpriseCacheDeserializer : ResourceDeserializer<RedisEnterpriseCacheResourceV1>
    {
        public RedisEnterpriseCacheDeserializer(ILogger<RedisEnterpriseCacheDeserializer> logger) : base(logger)
        {
            Map(resource => resource.CacheName)
                .IsRequired();
        }
    }
}
