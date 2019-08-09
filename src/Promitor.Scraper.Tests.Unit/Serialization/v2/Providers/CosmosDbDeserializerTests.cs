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
    public class CosmosDbDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly CosmosDbDeserializer _deserializer;

        public CosmosDbDeserializerTests()
        {
            _deserializer = new CosmosDbDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_DbNameSupplied_SetsDbName()
        {
            DeserializerTestHelpers.AssertPropertySet<CosmosDbResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "dbName: promitor-cosmos",
                "promitor-cosmos",
                c => c.DbName);
        }

        [Fact]
        public void Deserialize_DbNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<CosmosDbResourceV2, AzureResourceDefinitionV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.DbName);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new CosmosDbDeserializer(new Mock<ILogger>().Object);
        }
    }
}
