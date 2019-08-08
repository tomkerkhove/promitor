using System;
using System.ComponentModel;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class MetricDefaultsDeserializerTests
    {
        private readonly MetricDefaultsDeserializer _deserializer;
        private readonly Mock<IDeserializer<AggregationV2>> _aggregationDeserializer;
        private readonly Mock<IDeserializer<ScrapingV2>> _scrapingDeserializer;

        public MetricDefaultsDeserializerTests()
        {
            _aggregationDeserializer = new Mock<IDeserializer<AggregationV2>>();
            _scrapingDeserializer = new Mock<IDeserializer<ScrapingV2>>();

            _deserializer = new MetricDefaultsDeserializer(_aggregationDeserializer.Object, _scrapingDeserializer.Object);
        }

        [Fact]
        public void Deserialize_NodeNull_ThrowsException()
        {
            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => _deserializer.Deserialize(null));
        }

        [Fact]
        public void Deserialize_NodeWrongType_ThrowsException()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("version: v1").Children["version"];

            // Act / Assert
            Assert.Throws<ArgumentException>(() => _deserializer.Deserialize(node));
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

            var aggregationNode = node.Children["aggregation"];
            var aggregation = new AggregationV2();
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
            var node = YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            // Act
            _deserializer.Deserialize(node);

            // Assert
            _aggregationDeserializer.Verify(d => d.Deserialize(It.IsAny<YamlNode>()), Times.Never);
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

            var scrapingNode = node.Children["scraping"];
            var scraping = new ScrapingV2();
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
            var node = YamlUtils.CreateYamlNode(yamlText).Children["metricDefaults"];

            // Act
            _deserializer.Deserialize(node);

            // Assert
            _scrapingDeserializer.Verify(d => d.Deserialize(It.IsAny<YamlNode>()), Times.Never);
        }
    }
}
