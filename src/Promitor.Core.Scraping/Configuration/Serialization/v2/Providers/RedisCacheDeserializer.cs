using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class RedisCacheDeserializer : ResourceDeserializer
    {
        private const string CacheNameTag = "cacheName";

        public RedisCacheDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new RedisCacheResourceV2
            {
                CacheName = GetString(node, CacheNameTag)
            };
        }
    }
}
