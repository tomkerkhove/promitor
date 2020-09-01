using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class ExpressRouteCircuitsDeserializerTests : ResourceDeserializerTest<ExpressRouteCircuitsDeserializer>
    {
        private readonly ExpressRouteCircuitsDeserializer _deserializer;

        public ExpressRouteCircuitsDeserializerTests()
        {
            _deserializer = new ExpressRouteCircuitsDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ExpressRouteCircuitsNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<ExpressRouteCircuitsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "expressRouteCircuitsName: promitor-express-route-circuit",
                "promitor-express-route-circuit",
                r => r.ExpressRouteCircuitsName);
        }

        [Fact]
        public void Deserialize_ExpressRouteCircuitsNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ExpressRouteCircuitsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ExpressRouteCircuitsName);
        }

        //[Fact (Skip = "fails..")]
        [Fact]
        public void Deserialize_ExpressRouteCircuitsNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "expressRouteCircuitsName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ExpressRouteCircuitsDeserializer(Logger);
        }
    }
}
