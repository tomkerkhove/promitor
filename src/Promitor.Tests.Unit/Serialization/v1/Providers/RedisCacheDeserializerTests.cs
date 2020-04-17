using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class RedisCacheDeserializerTests : ResourceDeserializerTest<RedisCacheDeserializer>
    {
        private readonly RedisCacheDeserializer _deserializer;

        public RedisCacheDeserializerTests()
        {
            _deserializer = new RedisCacheDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_CacheNameSupplied_SetsCacheName()
        {
            YamlAssert.PropertySet<RedisCacheResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "cacheName: promitor-cache",
                "promitor-cache",
                r => r.CacheName);
        }

        [Fact]
        public void Deserialize_CacheNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<RedisCacheResourceV1, AzureResourceDefinitionV1>(
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
            return new RedisCacheDeserializer(Logger);
        }
    }
}
