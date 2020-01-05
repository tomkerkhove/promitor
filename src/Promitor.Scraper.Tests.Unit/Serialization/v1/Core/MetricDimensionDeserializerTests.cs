using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class MetricDimensionDeserializerTests
    {
        private readonly MetricDimensionDeserializer _deserializer;

        public MetricDimensionDeserializerTests()
        {
            _deserializer = new MetricDimensionDeserializer(NullLogger<MetricDimensionDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_DimensionNameSupplied_SetsDimensionName()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "name: containerName",
                "containerName",
                a => a.Name);
        }

        [Fact]
        public void Deserialize_DimensionNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "resourceGroupName: promitor-group",
                a => a.Name);
        }
    }
}
