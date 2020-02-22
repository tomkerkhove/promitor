﻿using System;
using System.ComponentModel;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
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
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act
            var result = _deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(r => r.ReportError(node, It.Is<string>(m => m.Contains("type"))));
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
