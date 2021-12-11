using Promitor.Core.Contracts.ResourceTypes.Enums;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class PostgreSqlDeserializerTests : ResourceDeserializerTest<PostgreSqlDeserializer>
    {
        private readonly PostgreSqlDeserializer _deserializer;

        public PostgreSqlDeserializerTests()
        {
            _deserializer = new PostgreSqlDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            YamlAssert.PropertySet<PostgreSqlResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "serverName: promitor-db",
                "promitor-db",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PostgreSqlResourceV1, AzureResourceDefinitionV1>(
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
        [InlineData("Single", PostgreSqlServerType.Single)]
        [InlineData("Flexible", PostgreSqlServerType.Flexible)]
        [InlineData("Hyperscale", PostgreSqlServerType.Hyperscale)]
        [InlineData("Arc", PostgreSqlServerType.Arc)]
        public void Deserialize_ServerTypeSupplied_SetsServerName(string rawType, PostgreSqlServerType expectedType)
        {
            YamlAssert.PropertySet<PostgreSqlResourceV1, AzureResourceDefinitionV1, PostgreSqlServerType>(
                _deserializer,
                $"type: {rawType}",
                expectedType,
                r => r.Type);
        }

        [Fact]
        public void Deserialize_ServerTypeNotSupplied_DefaultsToSingle()
        {
            YamlAssert.PropertySet<PostgreSqlResourceV1, AzureResourceDefinitionV1, PostgreSqlServerType>(
                _deserializer,
                "resourceGroupName: promitor-group",
                PostgreSqlServerType.Single,
                r => r.Type);
        }

        [Fact]
        public void Deserialize_ServerTypeNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsNoErrorForProperty(
                _deserializer,
                node,
                "type");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PostgreSqlDeserializer(Logger);
        }
    }
}
