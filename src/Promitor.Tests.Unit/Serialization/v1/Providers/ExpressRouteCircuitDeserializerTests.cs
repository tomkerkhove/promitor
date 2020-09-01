using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ExpressRouteCircuitDeserializerTests : ResourceDeserializerTest<ExpressRouteCircuitDeserializer>
    {
        private readonly ExpressRouteCircuitDeserializer _deserializer;

        public ExpressRouteCircuitDeserializerTests()
        {
            _deserializer = new ExpressRouteCircuitDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ExpressRouteCircuitNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<ExpressRouteCircuitResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "expressRouteCircuitName: promitor-express-route-circuit",
                "promitor-express-route-circuit",
                r => r.ExpressRouteCircuitName);
        }

        [Fact]
        public void Deserialize_ExpressRouteCircuitNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ExpressRouteCircuitResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ExpressRouteCircuitName);
        }

        [Fact]
        public void Deserialize_ExpressRouteCircuitNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "expressRouteCircuitName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ExpressRouteCircuitDeserializer(Logger);
        }
    }
}
