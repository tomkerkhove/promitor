using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Mapping
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
            var resource = new GenericAzureMetricDefinitionV1 {Name = "promitor_metric"};

            // Act
            var definition = _mapper.Map<MetricDefinition>(resource);

            // Assert
            Assert.NotNull(definition.PrometheusMetricDefinition);
            Assert.Equal(resource.Name, definition.PrometheusMetricDefinition.Name);
        }

        [Fact]
        public void Map_MapsResourceType()
        {
            // Arrange
            var resource = new StorageQueueMetricDefinitionV1();

            // Act
            var definition = _mapper.Map<MetricDefinition>(resource);

            // Assert
            Assert.Equal(ResourceType.StorageQueue, definition.ResourceType);
        }

        [Fact]
        public void Map_MapsSingleResource()
        {
            // Arrange
            var resource = new RedisCacheMetricDefinitionV1();

            // Act
            var definition = _mapper.Map<MetricDefinition>(resource);

            // Assert
            var runtimeResource = Assert.Single(definition.Resources);
            Assert.IsType<RedisCacheResourceDefinition>(runtimeResource);
        }
    }
}
