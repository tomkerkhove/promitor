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
    public class AzureMetricConfigurationDeserializerTests
    {
        private readonly AzureMetricConfigurationDeserializer _deserializer;
        private readonly Mock<IDeserializer<MetricAggregationV1>> _aggregationDeserializer;

        public AzureMetricConfigurationDeserializerTests()
        {
            _aggregationDeserializer = new Mock<IDeserializer<MetricAggregationV1>>();

            _deserializer = new AzureMetricConfigurationDeserializer(_aggregationDeserializer.Object, NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_MetricNameSupplied_SetsMetricName()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "metricName: ActiveMessages",
                "ActiveMessages",
                a => a.MetricName);
        }

        [Fact]
        public void Deserialize_MetricNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "resourceGroupName: promitor-group",
                a => a.MetricName);
        }

        [Fact]
        public void Deserialize_AggregationSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"aggregation:
    type: Average";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var aggregationNode = (YamlMappingNode) node.Children["aggregation"];

            var aggregation = new MetricAggregationV1();
            _aggregationDeserializer.Setup(d => d.Deserialize(aggregationNode)).Returns(aggregation);

            // Act
            var config = _deserializer.Deserialize(node);

            // Assert
            Assert.Same(aggregation, config.Aggregation);
        }

        [Fact]
        public void Deserialize_AggregationNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "metricName: ActiveMessages",
                c => c.Aggregation);
        }
    }
}
