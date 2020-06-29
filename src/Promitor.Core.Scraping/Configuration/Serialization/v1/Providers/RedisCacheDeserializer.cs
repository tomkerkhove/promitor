using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class RedisCacheDeserializer : ResourceDeserializer<RedisCacheResourceV1>
    {
        public RedisCacheDeserializer(ILogger<RedisCacheDeserializer> logger) : base(logger)
        {
            Map(resource => resource.CacheName)
                .IsRequired();
        }
    }
}
