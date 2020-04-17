using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class CosmosDbDeserializerTests : ResourceDeserializerTest<CosmosDbDeserializer>
    {
        private readonly CosmosDbDeserializer _deserializer;

        public CosmosDbDeserializerTests()
        {
            _deserializer = new CosmosDbDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_DbNameSupplied_SetsDbName()
        {
            YamlAssert.PropertySet<CosmosDbResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "dbName: promitor-cosmos",
                "promitor-cosmos",
                c => c.DbName);
        }

        [Fact]
        public void Deserialize_DbNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<CosmosDbResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.DbName);
        }

        [Fact]
        public void Deserialize_DbNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "dbName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new CosmosDbDeserializer(Logger);
        }
    }
}
