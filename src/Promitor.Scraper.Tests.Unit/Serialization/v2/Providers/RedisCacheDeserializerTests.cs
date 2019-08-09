using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    [Category("Unit")]
    public class RedisCacheDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly RedisCacheDeserializer _deserializer;

        public RedisCacheDeserializerTests()
        {
            _deserializer = new RedisCacheDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_CacheNameSupplied_SetsCacheName()
        {
            DeserializerTestHelpers.AssertPropertySet<RedisCacheResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "cacheName: promitor-cache",
                "promitor-cache",
                r => r.CacheName);
        }

        [Fact]
        public void Deserialize_CacheNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<RedisCacheResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.CacheName);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new RedisCacheDeserializer(new Mock<ILogger>().Object);
        }
    }
}
