using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class MariaDbDeserializerTests : ResourceDeserializerTest<MariaDbDeserializer>
    {
        private readonly MariaDbDeserializer _deserializer;

        public MariaDbDeserializerTests()
        {
            _deserializer = new MariaDbDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsName()
        {
            const string serverName = "promitor-cache";
            YamlAssert.PropertySet<MariaDbResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"serverName: {serverName}",
                serverName,
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<MariaDbResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_MariaDbNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "serverName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new MariaDbDeserializer(Logger);
        }
    }
}
