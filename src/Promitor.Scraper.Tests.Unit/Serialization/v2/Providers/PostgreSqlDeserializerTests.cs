using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    public class PostgreSqlDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly PostgreSqlDeserializer _deserializer;

        public PostgreSqlDeserializerTests()
        {
            _deserializer = new PostgreSqlDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            DeserializerTestHelpers.AssertPropertySet<PostgreSqlResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "serverName: promitor-db",
                "promitor-db",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<PostgreSqlResourceV2, AzureResourceDefinitionV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ServerName);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new PostgreSqlDeserializer(new Mock<ILogger>().Object);
        }
    }
}
