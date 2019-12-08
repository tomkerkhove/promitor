using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class RedisCacheDeserializer : ResourceDeserializer<RedisCacheResourceV1>
    {
        private const string CacheNameTag = "cacheName";

        public RedisCacheDeserializer(ILogger<RedisCacheDeserializer> logger) : base(logger)
        {
        }

        protected override RedisCacheResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var cacheName = node.GetString(CacheNameTag);

            return new RedisCacheResourceV1
            {
                CacheName = cacheName
            };
        }
    }
}
