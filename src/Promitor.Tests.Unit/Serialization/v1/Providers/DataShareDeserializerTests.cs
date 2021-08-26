using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class DataShareDeserializerTests : ResourceDeserializerTest<DataShareDeserializer>
    {
        private readonly DataShareDeserializer _deserializer;

        public DataShareDeserializerTests()
        {
            _deserializer = new DataShareDeserializer(Logger);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new DataShareDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            YamlAssert.PropertySet<DataShareResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "accountName: promitor-group",
                "promitor-group",
                c => c.AccountName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<DataShareResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-resource-group",
                c => c.AccountName);
        }

        [Fact]
        public void Deserialize_ShareNameSupplied_SetsShareName()
        {
            YamlAssert.PropertySet<DataShareResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "shareName: staging",
                "staging",
                r => r.ShareName);
        }

        [Fact]
        public void Deserialize_ShareNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<DataShareResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ShareName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "accountName");
        }
    }
}
