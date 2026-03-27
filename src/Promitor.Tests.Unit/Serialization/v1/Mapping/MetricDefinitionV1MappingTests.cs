using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Mapping
{
    [Category("Unit")]
    public class MetricDefinitionV1MappingTests : UnitTest
    {
        private readonly V1ConfigurationMapper _mapper;

        public MetricDefinitionV1MappingTests()
        {
            _mapper = new V1ConfigurationMapper();
        }

        [Fact]
        public void Map_CanMapPrometheusMetricDefinition()
        {
            // Arrange
            var resource = new MetricDefinitionV1 {Name = "promitor_metric", Description = "Metric description"};

            // Act
            var definition = _mapper.MapMetricDefinition(resource);

            // Assert
            Assert.NotNull(definition.PrometheusMetricDefinition);
            Assert.Equal(resource.Name, definition.PrometheusMetricDefinition.Name);
            Assert.Equal(resource.Description, definition.PrometheusMetricDefinition.Description);
        }
    }
}
