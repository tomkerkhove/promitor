using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class RedisEnterpriseCacheDeserializerTests : ResourceDeserializerTest<RedisEnterpriseCacheDeserializer>
    {
        private readonly RedisEnterpriseCacheDeserializer _deserializer;

        public RedisEnterpriseCacheDeserializerTests()
        {
            _deserializer = new RedisEnterpriseCacheDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_CacheNameSupplied_SetsCacheName()
        {
            const string cacheName = "promitor-cache";
            YamlAssert.PropertySet<RedisEnterpriseCacheResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"cacheName: {cacheName}",
                cacheName,
                r => r.CacheName);
        }

        [Fact]
        public void Deserialize_CacheNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<RedisEnterpriseCacheResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.CacheName);
        }

        [Fact]
        public void Deserialize_CacheNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "cacheName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new RedisEnterpriseCacheDeserializer(Logger);
        }
    }
}
