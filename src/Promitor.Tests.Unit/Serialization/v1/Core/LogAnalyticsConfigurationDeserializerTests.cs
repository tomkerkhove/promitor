using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class LogAnalyticsConfigurationDeserializerTests : UnitTest
    {
        private readonly LogAnalyticsConfigurationDeserializer _deserializer;
        private readonly Mock<IDeserializer<AggregationV1>> _aggregationDeserializer;
        private readonly Mock<IErrorReporter> _errorReporter = new();

        public LogAnalyticsConfigurationDeserializerTests()
        {
            _aggregationDeserializer = new Mock<IDeserializer<AggregationV1>>();
           _deserializer = new LogAnalyticsConfigurationDeserializer(_aggregationDeserializer.Object, NullLogger<LogAnalyticsConfigurationDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_QuerySupplied_SetsQuery()
        {
            var yamlText = "query: AzureActivity | take 100";
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "AzureActivity | take 100",
                a => a.Query);

            YamlAssert.PropertyNull(
                _deserializer,
                yamlText,
                a => a.Aggregation);
        }

        [Fact]
        public void Deserialize_AggregationSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
                @"aggregation:
                    interval: 10:00:00:00";

            var node = YamlUtils.CreateYamlNode(yamlText);
            var aggregationNode = (YamlMappingNode)node.Children["aggregation"];

            var aggregation = new AggregationV1();
            _aggregationDeserializer.Setup(
                d => d.Deserialize(aggregationNode, _errorReporter.Object)).Returns(aggregation);

            // Act
            var config = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(aggregation, config.Aggregation);
        }

        [Fact]
        public void Deserialize_QueryNotSupplied_ReportsError()
        {
            var node = YamlUtils.CreateYamlNode("field: promitor");

            YamlAssert.ReportsErrorForProperty(_deserializer, node, "query");
        }
    }
}