using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class MetricDefaultsDeserializerTests
    {
        private readonly MetricDefaultsDeserializer _deserializer;
        private readonly Mock<IDeserializer<AggregationV1>> _aggregationDeserializer;
        private readonly Mock<IDeserializer<ScrapingV1>> _scrapingDeserializer;

        public MetricDefaultsDeserializerTests()
        {
            _aggregationDeserializer = new Mock<IDeserializer<AggregationV1>>();
            _scrapingDeserializer = new Mock<IDeserializer<ScrapingV1>>();

            _deserializer = new MetricDefaultsDeserializer(
                _aggregationDeserializer.Object, _scrapingDeserializer.Object, NullLogger<MetricDefaultsDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_AggregationPresent_UsesAggregationDeserializer()
        {
            // Arrange
            const string yamlText =
@"metricDefaults:
    aggregation:
        interval: 00:05:00";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            var aggregationNode = (YamlMappingNode)node.Children["aggregation"];
            var aggregation = new AggregationV1();
            _aggregationDeserializer.Setup(d => d.Deserialize(aggregationNode)).Returns(aggregation);

            // Act
            var defaults = _deserializer.Deserialize(node);

            // Assert
            Assert.Same(aggregation, defaults.Aggregation);
        }

        [Fact]
        public void Deserialize_AggregationNotPresent_DoesNotUseDeserializer()
        {
            // Arrange
            const string yamlText =
@"metricDefaults:
    scraping:
        schedule: '0 * * ? * *'";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            // Act
            _deserializer.Deserialize(node);

            // Assert
            _aggregationDeserializer.Verify(d => d.Deserialize(It.IsAny<YamlMappingNode>()), Times.Never);
        }

        [Fact]
        public void Deserialize_ScrapingPresent_UsesScrapingDeserializer()
        {
            // Arrange
            const string yamlText =
@"metricDefaults:
    scraping:
        schedule: '0 * * ? * *'";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            var scrapingNode = (YamlMappingNode)node.Children["scraping"];
            var scraping = new ScrapingV1();
            _scrapingDeserializer.Setup(d => d.Deserialize(scrapingNode)).Returns(scraping);

            // Act
            var defaults = _deserializer.Deserialize(node);

            // Assert
            Assert.Same(scraping, defaults.Scraping);
        }

        [Fact]
        public void Deserialize_ScrapingNotPresent_DoesNotUseDeserializer()
        {
            // Arrange
            const string yamlText =
@"metricDefaults:
    aggregation:
        interval: '00:05:00'";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            // Act
            _deserializer.Deserialize(node);

            // Assert
            _scrapingDeserializer.Verify(d => d.Deserialize(It.IsAny<YamlMappingNode>()), Times.Never);
        }
    }
}
