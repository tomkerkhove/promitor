using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Mapping
{
    [Category("Unit")]
    public class MetricDefinitionV1MappingTests
    {
        private readonly IMapper _mapper;

        public MetricDefinitionV1MappingTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Map_CanMapPrometheusMetricDefinition()
        {
            // Arrange
            var resource = new MetricDefinitionV1 {Name = "promitor_metric", Description = "Metric description"};

            // Act
            var definition = _mapper.Map<MetricDefinition>(resource);

            // Assert
            Assert.NotNull(definition.PrometheusMetricDefinition);
            Assert.Equal(resource.Name, definition.PrometheusMetricDefinition.Name);
            Assert.Equal(resource.Description, definition.PrometheusMetricDefinition.Description);
        }
    }
}
