using System;
using System.ComponentModel;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class MetricAggregationDeserializerTests
    {
        private readonly MetricAggregationDeserializer _deserializer;

        public MetricAggregationDeserializerTests()
        {
            _deserializer = new MetricAggregationDeserializer(NullLogger<MetricAggregationDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_TypeSupplied_SetsType()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "type: Maximum",
                AggregationType.Maximum,
                a => a.Type);
        }

        [Fact]
        public void Deserialize_TypeNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "interval: 00:05:00",
                a => a.Type);
        }

        [Fact]
        public void Deserialize_TypeNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "type");
        }

        [Fact]
        public void Deserialize_IntervalSupplied_SetsInterval()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "interval: 00:07:00",
                TimeSpan.FromMinutes(7),
                a => a.Interval);
        }

        [Fact]
        public void Deserialize_IntervalNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "type: Average",
                a => a.Interval);
        }
    }
}
