using System;
using System.Collections.Generic;
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
    public class V1DeserializerTests
    {
        private readonly Mock<IDeserializer<AzureMetadataV1>> _metadataDeserializer;
        private readonly Mock<IDeserializer<MetricDefaultsV1>> _defaultsDeserializer;
        private readonly Mock<IDeserializer<MetricDefinitionV1>> _metricsDeserializer;
        private readonly V1Deserializer _deserializer;

        public V1DeserializerTests()
        {
            _metadataDeserializer = new Mock<IDeserializer<AzureMetadataV1>>();
            _defaultsDeserializer = new Mock<IDeserializer<MetricDefaultsV1>>();
            _metricsDeserializer = new Mock<IDeserializer<MetricDefinitionV1>>();

            _deserializer = new V1Deserializer(
                _metadataDeserializer.Object,
                _defaultsDeserializer.Object,
                _metricsDeserializer.Object,
                NullLogger<V1Deserializer>.Instance);
        }

        [Fact]
        public void Deserialize_NoVersionSpecified_ThrowsException()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("azureMetadata:");

            // Act
            var exception = Assert.Throws<Exception>(() => _deserializer.Deserialize(yamlNode));

            // Assert
            Assert.Equal("No 'version' element was found in the metrics config", exception.Message);
        }

        [Fact]
        public void Deserialize_VersionSpecified_SetsCorrectVersion()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v1");

            // Act
            var builder = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Equal("v1", builder.Version);
        }

        [Fact]
        public void Deserialize_WrongVersionSpecified_ThrowsException()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v2");

            // Act
            var exception = Assert.Throws<Exception>(() => _deserializer.Deserialize(yamlNode));

            // Assert
            Assert.Equal("A 'version' element with a value of 'v1' was expected but the value 'v2' was found", exception.Message);
        }

        [Fact]
        public void Deserialize_AzureMetadata_UsesMetadataDeserializer()
        {
            // Arrange
            const string config =
@"version: v1
azureMetadata:
  tenantId: 'abc-123'";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var azureMetadata = new AzureMetadataV1();
            _metadataDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(azureMetadata);

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Same(azureMetadata, declaration.AzureMetadata);
        }

        [Fact]
        public void Deserialize_AzureMetadataNotSupplied_SetsMetadataNull()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v1");
            _metadataDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(new AzureMetadataV1());

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Null(declaration.AzureMetadata);
        }

        [Fact]
        public void Deserialize_MetricDefaults_UsesDefaultsDeserializer()
        {
            // Arrange
            const string config =
                @"version: v1
metricDefaults:
  aggregation:
    interval: '00:05:00'";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var metricDefaults = new MetricDefaultsV1();
            _defaultsDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(metricDefaults);

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Same(metricDefaults, declaration.MetricDefaults);
        }

        [Fact]
        public void Deserialize_MetricDefaultsNotSupplied_SetsDefaultsNull()
        {
            // Arrange
            const string config =
                @"version: v1";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            _defaultsDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(new MetricDefaultsV1());

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Null(declaration.MetricDefaults);
        }

        [Fact]
        public void Deserialize_Metrics_UsesMetricsDeserializer()
        {
            // Arrange
            const string config =
                @"version: v1
metrics:
- name: promitor_metrics_total";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var metrics = new List<MetricDefinitionV1>();
            _metricsDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlSequenceNode>())).Returns(metrics);

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Same(metrics, declaration.Metrics);
        }

        [Fact]
        public void Deserialize_Metric_SetsMetricsNull()
        {
            // Arrange
            const string config =
                @"version: v1";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            _metricsDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlSequenceNode>())).Returns(new List<MetricDefinitionV1>());

            // Act
            var declaration = _deserializer.Deserialize(yamlNode);

            // Assert
            Assert.Null(declaration.Metrics);
        }
    }
}
