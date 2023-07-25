using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureMetricConfigurationDeserializerTests : UnitTest
    {
        private readonly AzureMetricConfigurationDeserializer _deserializer;
        private readonly Mock<IDeserializer<MetricDimensionV1>> _dimensionDeserializer;
        private readonly Mock<IDeserializer<MetricAggregationV1>> _aggregationDeserializer;
        private readonly Mock<IErrorReporter> _errorReporter = new();

        public AzureMetricConfigurationDeserializerTests()
        {
            _dimensionDeserializer = new Mock<IDeserializer<MetricDimensionV1>>();
            _aggregationDeserializer = new Mock<IDeserializer<MetricAggregationV1>>();

            _deserializer = new AzureMetricConfigurationDeserializer(_dimensionDeserializer.Object,_aggregationDeserializer.Object, NullLogger<AzureMetricConfigurationDeserializer>.Instance);
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
        public void Deserialize_LimitSupplied_SetsLimit()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "limit: 10",
                10,
                a => a.Limit);
        }

        [Fact]
        public void Deserialize_LimitNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "resourceGroupName: promitor-group",
                a => a.Limit);
        }

        [Fact]
        public void Deserialize_MetricNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "metricName");
        }

        [Fact]
        public void Deserialize_AggregationSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
                @"aggregation:
    type: Average";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var aggregationNode = (YamlMappingNode)node.Children["aggregation"];

            var aggregation = new MetricAggregationV1();
            _aggregationDeserializer.Setup(
                d => d.Deserialize(aggregationNode, _errorReporter.Object)).Returns(aggregation);

            // Act
            var config = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(aggregation, config.Aggregation);
        }

        [Fact]
        public void Deserialize_DimensionSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
                @"dimension:
  name: EntityPath";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var dimensionNode = (YamlMappingNode)node.Children["dimension"];

            var dimension = new MetricDimensionV1();
            _dimensionDeserializer.Setup(d => d.Deserialize(dimensionNode, _errorReporter.Object)).Returns(dimension);

            // Act
            var config = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(dimension, config.Dimension);
        }

        [Fact]
        public void Deserialize_DimensionsSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
                @"dimensions:
  - name: EntityPath
  - name: Test";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var dimensionsNode = (YamlSequenceNode)node.Children["dimensions"];

            var dimensions = new List<MetricDimensionV1> { new(), new() };
            _dimensionDeserializer.Setup(d => d.Deserialize(dimensionsNode, _errorReporter.Object)).Returns(dimensions);

            // Act
            var config = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(dimensions, config.Dimensions);
        }

        [Fact]
        public void Deserialize_DimensionAndDimensionsSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
                @"dimension:
    name: EntityPath
dimensions:
    - name: EntityPath
    - name: EntityName";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var dimensionNode = (YamlMappingNode)node.Children["dimension"];
            var dimensionsNode = (YamlSequenceNode)node.Children["dimensions"];

            var dimension = new MetricDimensionV1();
            _dimensionDeserializer.Setup(d => d.Deserialize(dimensionNode, _errorReporter.Object)).Returns(dimension);

            var dimensions = new List<MetricDimensionV1> { new(), new() };
            _dimensionDeserializer.Setup(d => d.Deserialize(dimensionsNode, _errorReporter.Object)).Returns(dimensions);

            // Act
            var config = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Equal(dimension, config.Dimension);
            Assert.Same(dimensions, config.Dimensions);
        }

        [Fact]
        public void Deserialize_DimensionNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "metricName: ActiveMessages",
                c => c.Dimensions);
        }

        [Fact]
        public void Deserialize_AggregationNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "metricName: ActiveMessages",
                c => c.Aggregation);
        }

        [Fact]
        public void Deserialize_AggregationNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "aggregation");
        }
    }
}
