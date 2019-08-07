using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class V2SerializerTests
    {
        private readonly Mock<IDeserializer<AzureMetadataV2>> _metadataDeserializer;
        private readonly Mock<IDeserializer<MetricDefaultsV2>> _defaultsDeserializer;
        private readonly Mock<IDeserializer<List<MetricDefinitionV2>>> _metricsDeserializer;
        private readonly V2Serializer _serializer;

        public V2SerializerTests()
        {
            _metadataDeserializer = new Mock<IDeserializer<AzureMetadataV2>>();
            _defaultsDeserializer = new Mock<IDeserializer<MetricDefaultsV2>>();
            _metricsDeserializer = new Mock<IDeserializer<List<MetricDefinitionV2>>>();

            _serializer = new V2Serializer(
                _metadataDeserializer.Object, _defaultsDeserializer.Object, _metricsDeserializer.Object);
        }

        [Fact]
        public void InterpretYamlStream_NoVersionSpecified_ThrowsException()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("azureMetadata:");
            
            // Act
            var exception = Assert.Throws<Exception>(() => _serializer.InterpretYamlStream(yamlNode));

            // Assert
            Assert.Equal("No 'version' element was found in the metrics config", exception.Message);
        }

        [Fact]
        public void InterpretYamlStream_VersionSpecified_SetsCorrectVersion()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v2");

            // Act
            var builder = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Equal("v2", builder.Version);
        }

        [Fact]
        public void InterpretYamlStream_WrongVersionSpecified_ThrowsException()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v1");

            // Act
            var exception = Assert.Throws<Exception>(() => _serializer.InterpretYamlStream(yamlNode));

            // Assert
            Assert.Equal("A 'version' element with a value of 'v2' was expected but the value 'v1' was found", exception.Message);
        }

        [Fact]
        public void InterpretYamlStream_AzureMetadata_UsesMetadataDeserializer()
        {
            // Arrange
            const string config =
@"version: v2
azureMetadata:
  tenantId: 'abc-123'";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var azureMetadata = new AzureMetadataV2();
            _metadataDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlNode>())).Returns(azureMetadata);

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Same(azureMetadata, declaration.AzureMetadata);
        }

        [Fact]
        public void InterpretYamlStream_AzureMetadataNotSupplied_SetsMetadataNull()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode("version: v2");
            _metadataDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlNode>())).Returns(new AzureMetadataV2());

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Null(declaration.AzureMetadata);
        }

        [Fact]
        public void InterpretYamlStream_MetricDefaults_UsesDefaultsDeserializer()
        {
            // Arrange
            const string config =
                @"version: v2
metricDefaults:
  aggregation:
    interval: '00:05:00'";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var metricDefaults = new MetricDefaultsV2();
            _defaultsDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlNode>())).Returns(metricDefaults);

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Same(metricDefaults, declaration.MetricDefaults);
        }

        [Fact]
        public void InterpretYamlStream_MetricDefaultsNotSupplied_SetsDefaultsNull()
        {
            // Arrange
            const string config =
                @"version: v2";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            _defaultsDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlNode>())).Returns(new MetricDefaultsV2());

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Null(declaration.MetricDefaults);
        }

        [Fact]
        public void InterpretYamlStream_Metrics_UsesMetricsDeserializer()
        {
            // Arrange
            const string config =
                @"version: v2
metrics:
- name: promitor_metrics_total";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            var metrics = new List<MetricDefinitionV2>();
            _metricsDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlNode>())).Returns(metrics);

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Same(metrics, declaration.Metrics);
        }

        [Fact]
        public void InterpretYamlStream_Metric_SetsMetricsNull()
        {
            // Arrange
            const string config =
                @"version: v2";
            var yamlNode = YamlUtils.CreateYamlNode(config);
            _metricsDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlNode>())).Returns(new List<MetricDefinitionV2>());

            // Act
            var declaration = _serializer.InterpretYamlStream(yamlNode);

            // Assert
            Assert.Null(declaration.Metrics);
        }
    }
}
