using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ApplicationGatewayDeserializerTests : ResourceDeserializerTest<ApplicationGatewayDeserializer>
    {
        private readonly ApplicationGatewayDeserializer _deserializer;

        public ApplicationGatewayDeserializerTests()
        {
            _deserializer = new ApplicationGatewayDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ApplicationGatewayNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<ApplicationGatewayResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "applicationGatewayName: promitor-application-gateway",
                "promitor-application-gateway",
                r => r.ApplicationGatewayName);
        }

        [Fact]
        public void Deserialize_ApplicationGatewayNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ApplicationGatewayResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ApplicationGatewayName);
        }

        [Fact]
        public void Deserialize_ApplicationGatewayNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "applicationGatewayName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ApplicationGatewayDeserializer(Logger);
        }
    }
}
