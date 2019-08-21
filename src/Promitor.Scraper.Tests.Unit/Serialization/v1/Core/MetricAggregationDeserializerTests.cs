using System;
using System.ComponentModel;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class MetricAggregationDeserializerTests
    {
        private readonly MetricAggregationDeserializer _deserializer;
        
        public MetricAggregationDeserializerTests()
        {
            _deserializer = new MetricAggregationDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_TypeSupplied_SetsType()
        {
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                "type: Maximum",
                AggregationType.Maximum,
                a => a.Type);
        }

        [Fact]
        public void Deserialize_TypeNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                "interval: 00:05:00",
                a => a.Type);
        }

        [Fact]
        public void Deserialize_IntervalSupplied_SetsInterval()
        {
            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                "interval: 00:07:00",
                TimeSpan.FromMinutes(7),
                a => a.Interval);
        }

        [Fact]
        public void Deserialize_IntervalNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                "type: Average",
                a => a.Interval);
        }
    }
}
