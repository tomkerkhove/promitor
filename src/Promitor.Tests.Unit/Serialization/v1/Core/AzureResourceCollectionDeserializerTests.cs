using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureResourceDiscoveryGroupDeserializerTests
    {
        private readonly AzureResourceDiscoveryGroupDeserializer _deserializer;

        public AzureResourceDiscoveryGroupDeserializerTests()
        {
            _deserializer = new AzureResourceDiscoveryGroupDeserializer(NullLogger<AzureResourceDiscoveryGroupDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_CollectionNameSupplied_SetsName()
        {
            // Arrange
            const string yamlText =
                @"name: sample-1";

            // Act & Assert
            YamlAssert.PropertySet(_deserializer, yamlText, "sample-1", d => d.Name);
        }

        [Fact]
        public void Deserialize_CollectionNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("field: promitor");

            // Act & Assert
            YamlAssert.ReportsErrorForProperty(_deserializer, node, "name");
        }
    }
}
