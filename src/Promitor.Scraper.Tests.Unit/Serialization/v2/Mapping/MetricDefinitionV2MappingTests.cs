using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Mapping
{
    [Category("Unit")]
    public class MetricDefinitionV2MappingTests
    {
        private readonly IMapper _mapper;

        public MetricDefinitionV2MappingTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Map_CanMapPrometheusMetricDefinition()
        {
            // Arrange
            var resource = new MetricDefinitionV2 {Name = "promitor_metric", Description = "Metric description"};

            // Act
            var definition = _mapper.Map<MetricDefinition>(resource);

            // Assert
            Assert.NotNull(definition.PrometheusMetricDefinition);
            Assert.Equal(resource.Name, definition.PrometheusMetricDefinition.Name);
            Assert.Equal(resource.Description, definition.PrometheusMetricDefinition.Description);
        }
    }
}
