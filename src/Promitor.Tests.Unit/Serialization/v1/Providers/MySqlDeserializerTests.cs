using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class MySqlDeserializerTests : ResourceDeserializerTest<MySqlDeserializer>
    {
        private readonly MySqlDeserializer _deserializer;

        public MySqlDeserializerTests()
        {
            _deserializer = new MySqlDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            YamlAssert.PropertySet<MySqlResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "serverName: promitor-db",
                "promitor-db",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<MySqlResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "serverName");
        }

        [Theory]
        [InlineData("Single", MySqlServerType.Single)]
        [InlineData("Flexible", MySqlServerType.Flexible)]
        public void Deserialize_ServerTypeSupplied_SetsServerName(string rawType, MySqlServerType expectedType)
        {
            YamlAssert.PropertySet<MySqlResourceV1, AzureResourceDefinitionV1, MySqlServerType>(
                _deserializer,
                $"type: {rawType}",
                expectedType,
                r => r.Type);
        }

        [Fact]
        public void Deserialize_ServerTypeNotSupplied_DefaultsToSingle()
        {
            YamlAssert.PropertySet<MySqlResourceV1, AzureResourceDefinitionV1, MySqlServerType>(
                _deserializer,
                "resourceGroupName: promitor-group",
                MySqlServerType.Single,
                r => r.Type);
        }

        [Fact]
        public void Deserialize_ServerTypeNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "type");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new MySqlDeserializer(Logger);
        }
    }
}
