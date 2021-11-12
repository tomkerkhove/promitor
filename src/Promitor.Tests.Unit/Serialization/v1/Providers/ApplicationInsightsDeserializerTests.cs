using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ApplicationInsightsDeserializerTests : ResourceDeserializerTest<ApplicationInsightsDeserializer>
    {
        private readonly ApplicationInsightsDeserializer _deserializer;

        public ApplicationInsightsDeserializerTests()
        {
            _deserializer = new ApplicationInsightsDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ApplicationInsightsNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<ApplicationInsightsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "name: promitor-application-insights",
                "promitor-application-insights",
                r => r.Name);
        }

        [Fact]
        public void Deserialize_ApplicationInsightsNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ApplicationInsightsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.Name);
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
                "name");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ApplicationInsightsDeserializer(Logger);
        }
    }
}
